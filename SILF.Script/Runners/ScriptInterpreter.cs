using SILF.Script.Builders;
using System.Data;

namespace SILF.Script.Runners;

internal class ScriptInterpreter
{


    /// <summary>
    /// Interprete de líneas. 
    /// </summary>
    /// <param name="instance">Instancia de SILF.</param>
    /// <param name="line">Linea.</param>
    public static List<Eval> Interprete(Instance instance, Context context, ObjectContext classContext, FuncContext funcContext, string line, short level = 0)
    {

        // Si la app esta detenida
        if (!instance.IsRunning || funcContext.IsReturning)
            return [];


        // Preparador
        line = line.Normalize().Trim();

        // Si esta vacío
        if (string.IsNullOrWhiteSpace(line))
            return [];

        // Separar por punto.
        var separar = Actions.Blocks.Separar(line, '.');

        // Es una variable
        var (uType, uName, uSuccess) = Validations.Fields.IsNotValuableVar(instance, line);

        // Definición de constante
        if (Expressions.Fields.IsConst(line, out var constante))
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateConst(instance, context, funcContext, classContext, constante.Name, constante.Expression);

            // Respuesta
            return [new(true)];

        }



        // Definición de variable
        else if (Expressions.Fields.IsVar(line, out var variable))
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, funcContext, classContext, variable.Name, variable.Type, variable.Expression);

            // Respuesta
            return [new(true)];

        }

        // Foreach
        else if (line.StartsWith("?f"))
        {


            line = line.Remove(0, 2).Trim();

            var id = int.Parse(line);


            var @for = instance.Structures.Where(t => t.Id == id && t is FunctionBuilder.ForStructure).FirstOrDefault() as FunctionBuilder.ForStructure;

            if (@for == null)
            {
                return [];
            }


            var eval = MicroRunner.Runner(instance, context, funcContext, classContext, @for.Expression, 1);

            if (eval.Count != 1 || eval[0].Object.Tipo != new Tipo(Library.List))
            {
                return [];
            }




            var ll = eval[0].Object.GetValue() as List<SILFObjectBase>;


            foreach (var ee in ll ?? [])
            {

                var cons = new Context()
                {
                    BaseContext = context
                };

                cons.SetField(new()
                {
                    Instance = instance,
                    IsAssigned = true,
                    Isolation = Isolation.Read,
                    Name = @for.Name,
                    Tipo = ee.Tipo,
                    Value = ee
                });

                foreach (var l in @for.Lines)
                    Interprete(instance, cons, classContext, funcContext, l, 0);

            }


            return [];

        }

        // If
        else if (line.StartsWith("?i"))
        {
            line = line.Remove(0, 2).Trim();

            var id = int.Parse(line);





            if (instance.Structures.Where(t => t.Id == id && t is FunctionBuilder.IfStructure).FirstOrDefault() is not FunctionBuilder.IfStructure @if)
            {
                return [];
            }


            var eval = MicroRunner.Runner(instance, context, funcContext, classContext, @if.Expression, 1);



            if (eval.Count != 1 || eval[0].Object.Tipo != new Tipo(Library.Bool))
            {
                return [];
            }




            var ll = eval[0].Object.GetValue();

            if (ll is bool n && n == true)
            {

                var cons = new Context()
                {
                    BaseContext = context
                };

                foreach (var l in @if.Lines)
                    Interprete(instance, cons, classContext, funcContext, l, 0);

            }
            return [];


        }



        else if (separar == null || separar.Count <= 0)
            return [new(true)];

        // Es una asignación
        else if (Expressions.Fields.IsNew(line, out var type, out string valuesEx))
        {

            var z = instance.Library.Get(type ?? "");

            return [new(z)];

        }


        // Es una asignación
        else if (Validations.Fields.IsAssignment(line, out var nombre, out var operador, out var expresión))
        {


            var x = Actions.Blocks.Separar(nombre, '.');


            if (x.Count > 1)
            {
                Asignar(x, instance, context, funcContext, classContext, expresión);
                return [];
            }


            // Obtiene el campo
            IEstablish? field = context[nombre] ?? (from p in classContext.SILFObjectBase.Properties
                                                    where p.Name == nombre.Trim()
                                                    select p).FirstOrDefault() as IEstablish;

            // Si no existe
            if (field == null)
            {


                instance.WriteError("SC017", $"No existe el campo / propiedad '{nombre}'.");

                return [new(Objects.SILFNullObject.Create(), true)];
            }



            // Tipo esperado
            Tipo? presentType = field.Tipo;

            // Evaluación de las expresiones
            List<Eval> evaluations = MicroRunner.Runner(instance, context, funcContext, classContext, expresión, 1);

            if (evaluations.Count != 1)
            {
                instance.WriteError("SC009", $"La asignación no puede tener tener mas de 1 (un) bloque.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            // Result
            var evaluation = evaluations[0];

            // Si no son compatibles
            if (!Types.IsCompatible(instance, presentType, evaluation.Object.Tipo))
            {
                instance.WriteError("SC011", $"No se puede convertir <{evaluation.Object.Tipo}> en <{presentType!.Value.Description}>");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            // Asigna el valor
            field.Establish(evaluation.Object);

            return [new(Objects.SILFNullObject.Create(), true)];
        }


        // Es una asignación
        else if (Validations.Fields.IsPipe(line, out var evaluator, out var receptor))
        {

            // Evaluador.
            var evaluateParams = MicroRunner.Runner(instance, context, funcContext, classContext, evaluator, 1);


            var sep = Actions.Blocks.Separar(receptor, '.');

            if (sep.Count > 1)
            {
                SILFObjectBase? bs = null;

                for (int i = 0; i < sep.Count; i++)
                {

                    var bloque = sep[i];

                    if (bs == null)
                    {
                        List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, classContext, bloque.Value, 1);

                        if (evals.Count > 0)
                            bs = evals[0].Object;
                    }


                    else
                    {


                        // Validar si es función.
                        bool isFunction = Expressions.Functions.IsFunction(bloque.Value, out string name, out string @params);

                        name = (isFunction) ? name : bloque.Value;

                        if (isFunction || i + 1 == sep.Count)
                        {




                            // Funciones definidas por el usuario.
                            IFunction? function = (from F in instance.Library.GetFunctions(bs.Tipo.Description)
                                                   where F.Name == name
                                                   select F).FirstOrDefault();

                            // Si la función no existe.
                            if (function == null)
                            {
                                instance.WriteError("SC019", $"No se encontró el método '{nombre}' en el tipo '{bs.Tipo.Description}'");
                                return [new(true)];
                            }




                            var paramsExec = (i + 1 == sep.Count) ? evaluateParams : MicroRunner.Runner(instance, context, funcContext, classContext, @params, 1);



                            var mapping = new List<ParameterValue>
                    {
                        new(string.Empty, bs)
                    };

                            foreach (var param in paramsExec)
                            {
                                mapping.Add(new("", param.Object));
                            }





                            bool valid = Actions.Parameters.BuildParams(instance, function, mapping);

                            if (!valid)
                            {
                                return [new(true)];

                            }


                            FuncContext funcResult = function.Run(instance, mapping, classContext);


                            bs = instance.Library.Get(funcResult?.Value?.Tipo.Description ?? "null");


                            if (funcResult != null && funcResult.Value != null)
                            {
                                bs.SetValue(funcResult.Value.Value);
                            }
                            else
                            {
                                bs = SILFNullObject.Create();
                            }



                            // Nueva evaluación.

                            continue;

                        }



                    }


                }

                return [new(bs ?? SILFNullObject.Create())];
            }




        }


        // propagación.
        else if (level == 1 && line.StartsWith(".."))
        {

            // Line.
            line = line.Remove(0, 2);

            // Valor.
            var values = MicroRunner.Runner(instance, context, funcContext, classContext, line, level);


            if (values.Count != 1)
            {
                instance.WriteError("NOTDOCUMENTED", $"La expresión '{line.Trim()}' debe devolver solo 1 valor.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            var value = values[0];


            if (value.Object.Tipo != new Tipo(Library.List))
            {
                instance.WriteError("SC023", $"El operador de propagación no se puede usar en '{line.Trim()}' porque no es una lista.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }


            var lista = value.Object.GetValue() as List<SILFObjectBase>;


            List<Eval> final = [];
            foreach (var e in lista ?? [])
            {

                if (e is SILFObjectBase obj)
                    final.Add(new(obj));

            }

            return final;

        }


        else if (separar.Count > 1)
        {

            var fin = TryExec(separar, instance, context, funcContext, classContext);

            return fin;

        }


        // Definición de variable
        else if (uSuccess && level == 0)
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, funcContext, classContext, uName, uType, null);

            // Respuesta
            return [new(true)];

        }

        // Es numero
        else if (Options.IsNumber(line))
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.Number);

            // Si es modo PRE.
            if (instance.Environment == Environments.PreRun)
                return [new(@object)];

            // Convertir el numero.
            _ = decimal.TryParse(line, out decimal value);

            // Establecer el valor.
            @object.SetValue(value);

            // Retornar.
            return [new(@object)];

        }

        // Devuelve la cadena de string
        else if (Options.IsInterpoladString(line) && level == 1)
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.String);

            line = line.Remove(0, 2);
            line = line.Reverse().Remove(0, 1);
            line = line.Reverse();


            var se = Actions.Strings.SepararPorLlaves(line);

            string final = "";

            foreach (var group in se)
            {
                if (!group.StartsWith("{"))
                {
                    final += group;
                    continue;
                }

                // Interpretar
                string grupo = group;
                grupo = grupo.Remove(0, 1);
                grupo = grupo.Reverse().Remove(0, 1);
                grupo = grupo.Reverse();


                List<Eval> evaluations = MicroRunner.Runner(instance, context, funcContext, classContext, grupo, 1);

                if (evaluations.Count != 1)
                {
                    instance.WriteError("SC009", $"No se puede obtener el valor de la expresión '{grupo}'");
                    return [new(true)];
                }

                if (instance.Environment != Environments.PreRun)
                {
                    final += evaluations[0].Object.GetValue();
                }

            }


            @object.SetValue(final);

            return [new(@object)];

        }

        // Devuelve la cadena de string
        else if (Options.IsString(line) && level == 1)
        {
            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.String);

            if (instance.Environment == Environments.PreRun)
                return [new Eval(@object)];



            line = line.Remove(0, 1);
            line = line.Reverse().Remove(0, 1);
            line = line.Reverse();

            @object.SetValue(line);

            return [new(@object)];

        }

        // Devuelve la cadena de string
        else if (Options.IsComplexNumber(line) && level == 1)
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.Number);

            line = line.Remove(0, 2);
            line = line.Reverse().Remove(0, 1);
            line = line.Reverse();

            _ = decimal.TryParse(line, out decimal final);

            @object.SetValue(final);

            return [new Eval(@object)];
        }

        // Devuelve la cadena de string
        else if (Options.IsLotNumber(line) && level == 1)
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.LotNumber);

            line = line.Remove(0, 2);
            line = line.Reverse().Remove(0, 1);
            line = line.Reverse();


            @object.SetValue(line);

            return [new Eval(@object)];
        }

        // Es Booleano
        else if (Options.IsBool(line))
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.Bool);

            bool final = false;
            if (line == "true")
                bool.TryParse(line, out final);

            else if (line == "false")
                bool.TryParse(line, out final);


            @object.SetValue(final);

            return [new(@object)];

        }

        // propagación.
        else if (line.EndsWith("++"))
        {

            // Line.
            line = line.Reverse().Remove(0, 2).Reverse();

            var getValue = context[line.Trim()];

            if (getValue == null)
            {
                instance.WriteError("SC017", $"No existe el elemento '{line.Trim()}'");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            else if (!getValue.IsAssigned)
            {
                instance.WriteError("SC020", $"La variable '{line.Trim()}' no ha sido asignada.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            else if (getValue.Tipo != new Tipo(Library.Number))
            {
                instance.WriteError("", $"El operador ++ no puede ser usado en '{line.Trim()}' porque no es del tipo number.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }


            getValue.Value.SetValue((decimal)getValue.Value.Value + 1);

            return [new(getValue.Value)];

        }

        // propagación.
        else if (line.EndsWith("--"))
        {

            // Line.
            line = line.Reverse().Remove(0, 2).Reverse();

            var getValue = context[line.Trim()];

            if (getValue == null)
            {
                instance.WriteError("SC017", $"No existe el elemento '{line.Trim()}'");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            else if (!getValue.IsAssigned)
            {
                instance.WriteError("SC020", $"La variable '{line.Trim()}' no ha sido asignada.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            else if (getValue.Tipo != new Tipo(Library.Number))
            {
                instance.WriteError("", $"El operador -- no puede ser usado en '{line.Trim()}' porque no es del tipo number.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }


            getValue.Value.SetValue((decimal)getValue.Value.Value - 1);

            return [new(getValue.Value)];

        }

        // Elementos (Variables, constantes, propiedades)
        else if (level == 1 && (!line.EndsWith(')') && !line.EndsWith(']')))
        {

            var getValue = context[line.Trim()];

            if (getValue == null)
            {

                // Ehe 
                var prop = ExectProp(instance, classContext.SILFObjectBase, line.Trim());

                if (prop != null)
                {
                    return [prop];
                }

                instance.WriteError("SC017", $"No existe el elemento '{line.Trim()}'");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            else if (!getValue.IsAssigned)
            {
                instance.WriteError("SC020", $"La variable '{line.Trim()}' no ha sido asignada.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            return [new(getValue.Value)];

        }

        // Paréntesis
        else if (line.StartsWith('(') && line.EndsWith(')'))
        {
            line = line.Remove(0, 1);
            line = line.Reverse().Remove(0, 1);
            line = line.Reverse();

            var results = MicroRunner.Runner(instance, context, funcContext, classContext, line, 1);

            if (results.Count != 1)
                return [new(true)];

            return [results[0]];
        }

        // Lista.
        else if (line.StartsWith('[') && line.EndsWith(']'))
        {

            if (instance.Environment == Environments.PreRun)
            {
                return [new() { Object = instance.Library.Get(Library.List) }];
            }


            line = line.Remove(0, 1);
            line = line.Reverse().Remove(0, 1);
            line = line.Reverse();

            var results = MicroRunner.Runner(instance, context, funcContext, classContext, line, 1);

            var list = instance.Library.Get(Library.List);

            List<SILFObjectBase> a = [];

            foreach (var result in results)
            {
                a.Add(result.Object);
            }

            list.SetValue(a);

            return [new(list)];

        }

        // Ejecutar funciones
        else if (Expressions.Functions.IsFunction(line, out nombre, out string @params))
        {


            if (nombre == "print")
            {

                List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, classContext, @params, 1);

                foreach (var eval in evals)
                {
                    instance.Write(eval.Object.GetValue()?.ToString() ?? "");
                }

                return [new(Objects.SILFNullObject.Create(), true)];

            }


            //else if (nombre == "typeof")
            //{


            //    var f = context[@params.Trim()];

            //    string tipoDes = f?.Tipo.Description ?? "null";

            //    var mm = SILFStringObject.Create();
            //    mm.SetValue($"<{tipoDes}>");

            //    return [new(mm)];

            //}



            IFunction? function = (from F in instance.Functions
                                   where F.Name == nombre
                                   select F).FirstOrDefault();


            // Mapear los parámetros.
            List<ParameterValue> mapParams = [];


            // Si la función no existe.
            if (function == null)
            {


                function = (from F in classContext.SILFObjectBase.Functions
                            where F.Name == nombre
                            select F).FirstOrDefault();


                if (function != null)
                {
                    mapParams.Add(new ParameterValue(string.Empty, classContext.SILFObjectBase));
                }
                else
                {
                    instance.WriteError("SC019", $"No se encontró la función '{nombre}'");
                    return [new(true)];
                }

            }




            var paramsExec = MicroRunner.Runner(instance, context, funcContext, classContext, @params, 1);



            foreach (var param in paramsExec)
            {
                mapParams.Add(new("", param.Object));
            }

            bool valid = Actions.Parameters.BuildParams(instance, function, mapParams);

            if (!valid)
                return [new(true)];


            if (instance.Environment == Environments.PreRun)
            {
                return [new(instance.Library.Get(function.Type.Value.Description ?? "null"))];
            }


            FuncContext funcResult = function.Run(instance, mapParams, classContext);


            // Si la función no retorno nada o es void
            if (funcResult.IsVoid || !funcResult.IsReturning)
                return [new(true)];


            // Nueva evaluación.
            return [new Eval(funcResult.Value)];

        }

        // Ejecutar funciones
        else if (Expressions.Functions.IsIndex(line, out nombre, out @params))
        {

            var getValue = context[nombre];

            if (getValue == null)
            {
                instance.WriteError("SC017", $"No existe el elemento '{line.Trim()}'");
                return [new(Objects.SILFNullObject.Create(), true)];
            }


            if (getValue.Tipo != new Tipo(Library.List))
            {
                instance.WriteError("SC023", $"La característica de índice no esta disponible para el tipo <{getValue.Tipo}>.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }

            if (!getValue.IsAssigned)
            {
                instance.WriteError("SC020", $"La variable '{line.Trim()}' no ha sido asignada.");
                return [new(Objects.SILFNullObject.Create(), true)];
            }



            string command = $"{nombre}.get({@params})";

            var paramsExec = MicroRunner.Runner(instance, context, funcContext, classContext, command, 1);

            // Nueva evaluación.
            return [new Eval(paramsExec[0].Object)];

        }

        // return.
        else if (line.Trim().StartsWith("return"))
        {

            // Caso del return [
            line = line.Remove(0, "return".Length);

            // Si el tipo esperado es void y hay una expresión
            if (funcContext.IsVoid && line.Trim() != "")
            {
                funcContext.IsReturning = true;
                instance.WriteError("SC022", $"return void no puede tener expresiones");
                return [new(true)];
            }

            // Si el tipo el void
            else if (funcContext.IsVoid)
            {
                funcContext.IsReturning = true;
                return [new(true)];
            }

            // Evalúa
            List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, classContext, line, 1);

            if (evals.Count != 1)
            {
                instance.WriteError("SC009", $"La función '{funcContext.Name}' del tipo <{funcContext.WaitType}> no puede retornar 2 bloques de código.");
                return [new(true)];
            }

            var eval = evals[0];

            // Cambia los estados.
            funcContext.IsReturning = true;

            // Tipo es incompatible
            if (!Types.IsCompatible(instance, funcContext.WaitType, eval.Object.Tipo))
            {
                instance.WriteError("SC011", $"La función '{funcContext.Name}' del tipo <{funcContext.WaitType}> no puede retornar valores del tipo <{eval.Object.Tipo}>.");
                return [new(true)];
            }

            // Valores
            funcContext.Value.SetValue(eval.Object.GetValue());
            funcContext.Value.Tipo = eval.Object.Tipo;
            return [new(true)];

        }


        instance.WriteError("SC021", $"Expression invalida '{line}' en modo '{level}'");
        return [new(true)];

    }










    public static List<Eval> TryExec(IEnumerable<CodeBlock> blocks, Instance instance, Context context, FuncContext funcContext, ObjectContext objectContext)
    {

        // Objeto base.
        SILFObjectBase? @base = null;

        // Recorrer los bloques.
        foreach (CodeBlock block in blocks)
        {

            // Si no existe el objeto base.
            if (@base == null)
            {
                // Evaluar expresión.
                List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, objectContext, block.Value, 1);

                // Evaluar.
                if (evals.Count != 1)
                {
                    instance.WriteError("NO-DOCUMENTES", "No documentado");
                    return [];
                }

                @base = evals[0].Object;

                continue;

            }


            // Validar si es función.
            bool isFunction = Expressions.Functions.IsFunction(block.Value, out string name, out string @params);

            // Si es una función.
            if (isFunction)
            {

                // Funciones definidas por el usuario.
                IFunction? function = (from F in @base.Functions
                                       where F.Name == name
                                       select F).FirstOrDefault();

                // Si la función no existe.
                if (function == null)
                {
                    instance.WriteError("SC019", $"No se encontró el método '{name}' en el tipo '{@base.Tipo}'");
                    return [new(true)];
                }

                // Ejecutar el método.
                var resultFinal = ExecMethod(function, @params, instance, context, funcContext, ObjectContext.GenerateContext(@base));

                // Base.
                @base = instance.Library.Get(resultFinal.Object.Tipo.Description);

                @base.SetValue(resultFinal.Object.GetValue());


                continue;

            }


            // Propiedades definidas por el usuario.
            IProperty? property = (from F in @base.Properties
                                   where F.Name == block.Value
                                   select F).FirstOrDefault();

            // Si la función no existe.
            if (property == null)
            {
                instance.WriteError("SC019", $"No se encontró la propiedad '{name}' en el tipo '{@base.Tipo}'");
                return [new(true)];
            }


            property.Parent = @base;
            var result = property.GetValue(instance);

            @base = instance.Library.Get(result.Tipo.Description);
            @base.SetValue(result.Value);


            continue;

        }

        return [new(@base ?? SILFNullObject.Create())];

    }



    static Eval ExecMethod(IFunction function, string parameters, Instance instance, Context context, FuncContext funcContext, ObjectContext objectContext)
    {

        // Parámetros.
        var paramsExec = MicroRunner.Runner(instance, context, funcContext, objectContext, parameters, 1);

        // Mapear los parámetros.
        List<ParameterValue> mapParams = [new ParameterValue(string.Empty, objectContext.SILFObjectBase)];

        // Recorrer los parámetros.
        foreach (var param in paramsExec)
            mapParams.Add(new("", param.Object));

        // Validar la integridad de los parámetros.
        bool valid = Actions.Parameters.BuildParams(instance, function, mapParams);

        // Si no es valido.
        if (!valid)
            return new(true);



        // Resultado.
        FuncContext funcResult = function.Run(instance, mapParams, objectContext);

        // Obtener el objeto final.
        return new(funcResult.Value);

    }




    static Eval ExectProp(Instance instance, SILFObjectBase @base, string name)
    {

        // Propiedades definidas por el usuario.
        IProperty? property = (from F in @base.Properties
                               where F.Name == name
                               select F).FirstOrDefault();

        // Si la función no existe.
        if (property == null)
        {
            instance.WriteError("SC019", $"No se encontró la propiedad '{name}' en el tipo '{@base.Tipo}'");
            return new(true);
        }


        property.Parent = @base;
        var result = property.GetValue(instance);

        return new(result);
    }




    public static void Asignar(IEnumerable<CodeBlock> blocks, Instance instance, Context context, FuncContext funcContext, ObjectContext objectContext, string expression)
    {

        // Objeto base.
        SILFObjectBase? @base = null;

        // Recorrer los bloques.
        for (int i = 0; i < blocks.Count(); i++)
        {


            CodeBlock block = blocks.ElementAt(i);


            // Si no existe el objeto base.
            if (@base == null)
            {
                // Evaluar expresión.
                List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, objectContext, block.Value, 1);

                // Evaluar.
                if (evals.Count != 1)
                {
                    instance.WriteError("NO-DOCUMENTES", "No documentado");
                }

                @base = evals[0].Object;

                continue;

            }


            bool isFunction = Expressions.Functions.IsFunction(block.Value, out string name, out string @params);

            // Si es una función.
            if (isFunction)
            {

                // Funciones definidas por el usuario.
                IFunction? function = (from F in @base.Functions
                                       where F.Name == name
                                       select F).FirstOrDefault();

                // Si la función no existe.
                if (function == null)
                {
                    instance.WriteError("SC019", $"No se encontró el método '{name}' en el tipo '{@base.Tipo}'");
                }

                // Ejecutar el método.
                var resultFinal = ExecMethod(function, @params, instance, context, funcContext, ObjectContext.GenerateContext(@base));

                // Base.
                @base = instance.Library.Get(resultFinal.Object.Tipo.Description);

                @base.SetValue(resultFinal.Object.GetValue());

                continue;

            }


            // Propiedades definidas por el usuario.
            IProperty? property = (from F in @base.Properties
                                   where F.Name == block.Value
                                   select F).FirstOrDefault();

            // Si la función no existe.
            if (property == null)
            {
                instance.WriteError("SC019", $"No se encontró la propiedad '{name}' en el tipo '{@base.Tipo}'");
                continue;
            }



            if (i + 1 == blocks.Count())
            {

                // Evaluación de las expresiones
                List<Eval> evaluations = MicroRunner.Runner(instance, context, funcContext, objectContext, expression, 1);

                property.Establish(evaluations[0].Object);

                continue;
            }


            property.Parent = @base;
            var result = property.GetValue(instance);

            @base = instance.Library.Get(result.Tipo.Description);
            @base.SetValue(result.Value);


            continue;

        }

    }



}
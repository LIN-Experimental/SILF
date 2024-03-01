using SILF.Script.Objects;

namespace SILF.Script.Runners;


internal class ScriptInterpreter
{


    /// <summary>
    /// Interprete de líneas.
    /// </summary>
    /// <param name="instance">Instancia de SILF.</param>
    /// <param name="line">Linea.</param>
    public static Eval Interprete(Instance instance, Context context, FuncContext funcContext, string line, short level = 0)
    {

        // Si la app esta detenida
        if (!instance.IsRunning || funcContext.IsReturning)
            return new(false);


        // Preparador
        line = line.Normalize().Trim();

        // Si esta vacío
        if (string.IsNullOrWhiteSpace(line))
            return new(true);


        // Separar por punto.
        var separar = Actions.Blocks.Separar(line, '.');




        // Es una variable
        var isUnsigned = Fields.IsNotValuableVar(instance, line);

        // Definición de constante
        if (Expressions.Fields.IsConst(line, out var constante))
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateConst(instance, context, funcContext, constante.Name, constante.Expression);

            // Respuesta
            return new(true);

        }



        // Definición de variable
        else if (Expressions.Fields.IsVar(line, out var variable))
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, funcContext, variable.Name, variable.Type, variable.Expression);

            // Respuesta
            return new(true);

        }


        else if (separar == null || separar.Count <= 0)
            return new(true);



        else if (separar.Count > 1)
        {
            SILFObjectBase? bs = null;
            foreach (var bloque in separar)
            {

                if (bs == null)
                {
                    List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, bloque.Value, 1);
                    bs = evals[0].Object;
                }


                else
                {


                    Expressions.Functions.IsFunction(bloque.Value, out string nombre, out string @params);



                    // Funciones definidas por el usuario.
                    IFunction? function = (from F in bs.Functions
                                           where F.Name == nombre
                                           select F).FirstOrDefault();

                    // Si la función no existe.
                    if (function == null)
                    {
                        //instance.WriteError($"No se encontró la función '{nombre}'");
                        return new(true);
                    }




                    var paramsExec = MicroRunner.Runner(instance, context, funcContext, @params, level);



                    var mapping = new List<ParameterValue>
                    {
                        new(string.Empty, new Tipo(bs.Tipo.Description), bs.GetValue())
                    };

                    foreach (var param in paramsExec)
                    {
                        mapping.Add(new("", param.Object.Tipo, param.Object.GetValue()));
                    }

                    bool valid = Actions.Parameters.BuildParams(instance, function, mapping);

                    if (!valid)
                    {
                        return new(true);

                    }


                    FuncContext funcResult = function.Run(instance, mapping);


                    bs = instance.Library.Get(funcResult.Value.Tipo.Description);
                    bs.SetValue(funcResult.Value.Value);

                    // Nueva evaluación.
                }


            }

            return new(bs ?? SILFNullObject.Create());

        }


        // Definición de variable
        else if (isUnsigned.success)
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, funcContext, isUnsigned.name, isUnsigned.type, null);

            // Respuesta
            return new(true);

        }

        // Es numero
        else if (Options.IsNumber(line))
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.Number);

            // Si es modo PRE.
            if (instance.Environment == Environments.PreRun)
                return new(@object);

            // Convertir el numero.
            _ = decimal.TryParse(line, out decimal value);

            // Establecer el valor.
            @object.SetValue(value);

            // Retornar.
            return new(@object);

        }

        // Devuelve la cadena de string
        else if (Options.IsInterpoladString(line) && level == 1)
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.String);

            line = line.Remove(0, 2);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);


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
                grupo = Microsoft.VisualBasic.Strings.StrReverse(grupo).Remove(0, 1);
                grupo = Microsoft.VisualBasic.Strings.StrReverse(grupo);


                List<Eval> evaluations = MicroRunner.Runner(instance, context, funcContext, grupo, 1);

                if (evaluations.Count != 1)
                {
                    instance.WriteError($"No se puede obtener el valor de la expresión '{grupo}'");
                    return new(true);
                }

                if (instance.Environment != Environments.PreRun)
                {
                    final += evaluations[0].Object.GetValue();
                }

            }


            @object.SetValue(final);

            return new(@object);

        }

        // Devuelve la cadena de string
        else if (Options.IsString(line) && level == 1)
        {
            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.String);

            if (instance.Environment == Environments.PreRun)
                return new Eval(@object);
            


            line = line.Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);

            @object.SetValue(line);

            return new(@object);

        }

        // Devuelve la cadena de string
        else if (Options.IsComplexNumber(line) && level == 1)
        {

            // Obtener un nuevo objeto.
            var @object = instance.Library.Get(Library.Number);

            line = line.Remove(0, 2);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);

            _=decimal.TryParse(line, out decimal final);

            @object.SetValue(final);
            
            return new Eval(@object);
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

            return new(@object);

        }

        // Es una asignación
        else if (Fields.IsAssignment(line, out var nombre, out var operador, out var expresión))
        {

            // Obtiene el campo
            Field? field = context[nombre];

            // Si no existe
            if (field == null)
            {
                instance.WriteError($"No existe el campo '{nombre}'.");
                return new(Objects.SILFNullObject.Create(), true);
            }

            // Si no se puede sobrescribir
            if (field.Isolation != Isolation.ReadAndWrite & field.Isolation != Isolation.Write)
            {
                instance.WriteError($"El campo '{nombre}' no se puede se puede sobrescribir.");
                return new(Objects.SILFNullObject.Create(), true);
            }

            // Tipo esperado
            Tipo presentType = field.Tipo;

            // Evaluación de las expresiones
            List<Eval> evaluations = MicroRunner.Runner(instance, context, funcContext, expresión, 1);

            if (evaluations.Count != 1)
            {
                instance.WriteError($"La asignación no puede tener tener mas de 1 (un) bloque.");
                return new(Objects.SILFNullObject.Create(), true);
            }

            // Result
            var evaluation = evaluations[0];

            // Si no son compatibles
            if (!Types.IsCompatible(instance, presentType, evaluation.Object.Tipo))
            {
                instance.WriteError($"No se puede convertir <{evaluation.Object.Tipo}> en <{presentType.Description}>");
                return new(Objects.SILFNullObject.Create(), true);
            }

            // Asigna el valor
            field.Value = evaluation.Object;
            field.IsAssigned = true;

            return new(Objects.SILFNullObject.Create(), true);
        }

        // Elementos (Variables, constantes)
        else if (level == 1 && !line.EndsWith(')'))
        {

            var getValue = context[line.Trim()];

            if (getValue == null)
            {
                instance.WriteError($"No existe el elemento '{line.Trim()}'");
                return new(Objects.SILFNullObject.Create(), true);
            }

            else if (!getValue.IsAssigned)
            {
                instance.WriteError($"La variable '{line.Trim()}' no ha sido asignada.");
                return new(Objects.SILFNullObject.Create(), true);
            }


            return new(getValue.Value);

        }

        // Paréntesis
        else if (line.StartsWith('(') && line.EndsWith(')'))
        {
            line = line.Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);

            var results = MicroRunner.Runner(instance, context, funcContext, line, 1);

            if (results.Count != 1)
                return new(true);

            return results[0];
        }

        // Ejecutar funciones
        else if (Expressions.Functions.IsFunction(line, out nombre, out string @params))
        {


            if (nombre == "print")
            {

                List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, @params, 1);

                foreach (var eval in evals)
                {
                    instance.Write(eval.Object.GetValue()?.ToString() ?? "");
                }

                return new(Objects.SILFNullObject.Create(), true);

            }

            else if (nombre == "typeof")
            {


                var f = context[@params.Trim()];

                string tipoDes = f?.Tipo.Description ?? "null";

                return new(new SILFStringObject()
                {
                    Value = $"<{tipoDes}>"
                });

            }




            // Funciones definidas por el usuario.
            IFunction? function = (from F in instance.Functions
                                   where F.Name == nombre
                                   select F).FirstOrDefault();

            // Si la función no existe.
            if (function == null)
            {
                instance.WriteError($"No se encontró la función '{nombre}'");
                return new(true);
            }




            var paramsExec = MicroRunner.Runner(instance, context, funcContext, @params, 1);



            var mapping = new List<ParameterValue>();

            foreach (var param in paramsExec)
            {
                mapping.Add(new("", param.Object.Tipo, param.Object.GetValue()));
            }

            bool valid = Actions.Parameters.BuildParams(instance, function, mapping);

            if (!valid)
                return new(true);




            FuncContext funcResult = function.Run(instance, mapping);


            // Si la función no retorno nada o es void
            if (funcResult.IsVoid || !funcResult.IsReturning)
                return new(true);


            // Si es Pre-Run
            if (instance.Environment == Environments.PreRun)
            {

                return new Eval();

            }

            // Nueva evaluación.
            return new Eval(funcResult.Value);

        }

        // Return.
        else if (line.Split(" ")[0] == "return")
        {

            // Caso del return
            line = line.Remove(0, "return".Length);

            // Si el tipo esperado es void y hay una expresión
            if (funcContext.IsVoid && line.Trim() != "")
            {
                funcContext.IsReturning = true;
                instance.WriteError($"Return void no puede tener expresiones");
                return new(true);
            }

            // Si el tipo el void
            else if (funcContext.IsVoid)
            {
                funcContext.IsReturning = true;
                return new(true);
            }

            // Evalúa
            List<Eval> evals = MicroRunner.Runner(instance, context, funcContext, line, 1);

            if (evals.Count != 1)
            {
                instance.WriteError($"La función '{funcContext.Name}' del tipo <{funcContext.WaitType}> no puede retornar 2 bloques de código.");
                return new(true);
            }

            var eval = evals[0];

            // Cambia los estados.
            funcContext.IsReturning = true;

            // Tipo es incompatible
            if (!Types.IsCompatible(instance, funcContext.WaitType, eval.Object.Tipo))
            {
                instance.WriteError($"La función '{funcContext.Name}' del tipo <{funcContext.WaitType}> no puede retornar valores del tipo <{eval.Object.Tipo}>.");
                return new(true);
            }

            // Valores
            funcContext.Value.SetValue(eval.Object.GetValue());
            funcContext.Value.Tipo = eval.Object.Tipo;
            return new(true);

        }



        instance.WriteError($"Expression invalida '{line}' en modo '{level}'");
        return new(true);

    }


}
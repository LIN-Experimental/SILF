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


        else if (line.Split(" ")[0] == "previous" && level == 1)
        {

            // Caso del previous.
            line = line.Remove(0, "previous".Length);

            // Nombre de la variable.
            var valuable = line.Trim();

            // Obtiene el valor.
            try
            {

                var @var = context[valuable];

                if (@var == null)
                {
                    instance.WriteError($"No existe el elemento '{valuable}' en este contexto.");
                    return new("", new(), true);
                }


                var value = @var.Values.SkipLast(1).LastOrDefault();

                if (value != null)
                    return new Eval(value.Element, value.Tipo);

                else
                    return new Eval(var.Value.Element, var.Value.Tipo);

            }
            catch (Exception)
            {
                instance.WriteError($"Errores al obtener el historial de '{valuable}'.");
            }
        }

        else if (line.Split(" ")[0] == "clear" && level == 0)
        {

            line = line.Remove(0, "clear".Length);

            var valuable = line.Trim();

            try
            {

                var var = context[valuable];

                if (var == null)
                {
                    instance.WriteError($"No existe el elemento '{valuable}' en este contexto.");
                    return new("", new(), true);
                }

                var.Values.RemoveRange(0, var.Values.Count - 1);

                return new(true);

            }
            catch (Exception)
            {
                instance.WriteError($"Errores al obtener el historial de '{valuable}'.");
            }
        }

        // Definición de variable
        else if (Expressions.Fields.IsVar(line, out var variable))
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, funcContext, variable.Name, variable.Type, variable.Expression);

            // Respuesta
            return new(true);

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
            var numberType = instance.Tipos.Where(T => T.Description == "number").FirstOrDefault();
            return new Eval(instance.Environment == Environments.PreRun ? "0" : decimal.Parse(line).ToString(), numberType);
        }

        // Devuelve la cadena de string
        else if (Options.IsString(line) && level == 1)
        {

            var tipo = instance.Tipos.Where(T => T.Description == "string").FirstOrDefault();
            if (instance.Environment == Environments.PreRun)
            {
                return new Eval("", tipo);
            }


            line = line.Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);


            return new Eval(line, tipo);
        }

        // Es Booleano
        else if (Options.IsBool(line))
        {
            var boolType = instance.Tipos.Where(T => T.Description == "bool").FirstOrDefault();

            if (line == "true")
                line = "1";
            else if (line == "false")
                line = "0";

            return new Eval(line, boolType);
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
                return new("", new(), true);
            }

            // Si no se puede sobrescribir
            if (field.Isolation != Isolation.ReadAndWrite & field.Isolation != Isolation.Write)
            {
                instance.WriteError($"El campo '{nombre}' no se puede se puede sobrescribir.");
                return new("", new(), true);
            }

            // Tipo esperado
            Tipo presentType = field.Tipo;

            // Evaluación de las expresiones
            List<Eval> evaluations = MicroRunner.Runner(instance, context, funcContext, expresión, 1);

            if (evaluations.Count != 1)
            {
                instance.WriteError($"La asignación no puede tener tener mas de 1 (un) bloque.");
                return new("", new(), true);
            }

            // Result
            var evaluation = evaluations[0];

            // Si no son compatibles
            if (!Types.IsCompatible(instance, presentType, evaluation.Tipo))
            {
                instance.WriteError($"No se puede convertir <{evaluation.Tipo}> en <{presentType.Description}>");
                return new("", new(), true);
            }

            // Asigna el valor
            field.Value = new(evaluation.Value, evaluation.Tipo.Value);
            field.IsAssigned = true;

            return new("", new(), true);
        }

        // Elementos (Variables, constantes)
        else if (level == 1 && !line.EndsWith(')'))
        {

            var getValue = context[line.Trim()];

            if (getValue == null)
            {
                instance.WriteError($"No existe el elemento '{line.Trim()}'");
                return new("", new(), true);
            }

            else if (!getValue.IsAssigned)
            {
                instance.WriteError($"La variable '{line.Trim()}' no ha sido asignada.");
                return new("", new(), true);
            }


            return new Eval(false)
            {
                Tipo = (instance.Environment == Environments.PreRun) ? getValue.Tipo : getValue.Value.Tipo,
                Value = (instance.Environment == Environments.PreRun) ? "" : getValue.Value.Element,
            };

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
                    instance.Write(eval.Value.ToString() ?? "");
                }

                return new("", new(), true);

            }

            else if (nombre == "type")
            {

                var evals = MicroRunner.Runner(instance, context, funcContext, @params, 1);

                if (evals.Count != 1)
                {
                    instance.WriteError("Param requerido");
                    return new();
                }

                if (!(evals[0].Tipo == null))
                {
                    instance.WriteError("NN");
                    return new();
                }


                string tipoDes = evals[0].Tipo.Value.Description;
                return new($"<{tipoDes}>", new("string"));

            }

            else if (nombre == "typeof")
            {


                var f = context[@params.Trim()];

                string tipoDes = f?.Tipo.Description ?? "null";
                return new($"<{tipoDes}>", new("string"));

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

            // Si es Pre-Run
            if (instance.Environment == Environments.PreRun)
            {
                return new("", function.Type, (!function.Type.HasValue));
            }


            var paramsExec = MicroRunner.Runner(instance, context, funcContext, @params, 1);





            var mapping = new List<ParameterValue>();

            foreach (var param in paramsExec)
            {
                mapping.Add(new("", param.Tipo.Value, param.Value));
            }

            bool valid = Actions.Parameters.BuildParams(instance, function, mapping);

            if (!valid)
                return new(true);




            FuncContext funcResult = function.Run(instance, mapping);


            // Si la función no retorno nada o es void
            if (funcResult.IsVoid || !funcResult.IsReturning)
                return new(true);


            // Nueva evaluación.
            return new Eval(funcResult.Value.Element, funcResult.Value.Tipo);

        }


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
            if (!Types.IsCompatible(instance, funcContext.WaitType, eval.Tipo))
            {
                instance.WriteError($"La función '{funcContext.Name}' del tipo <{funcContext.WaitType}> no puede retornar valores del tipo <{eval.Tipo}>.");
                return new(true);
            }

            // Valores
            funcContext.Value.Element = eval.Value;
            funcContext.Value.Tipo = eval.Tipo.Value;
            return new(true);

        }









        instance.WriteError($"Expression invalida '{line}' en modo '{level}'");
        return new(true);

    }


}
using SILF.Script.Validations;

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
            return new("", new(), true);

        // Es una variable
        var isVar = Fields.IsVar(line);

        // Es una variable
        var isUnsigned = Fields.IsNotValuableVar(instance, line);

        var isConst = Fields.IsConst(line);

        // Definición de variable
        if (isConst.success)
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateConst(instance, context, funcContext, isConst.name, isConst.expresion);

            // Respuesta
            return new(true);

        }

        // Definición de variable
        else if (isVar.success)
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, funcContext, isVar.name, isVar.type, isVar.expresion);

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
            Eval evaluation = MicroRunner.Runner(instance, context, funcContext, expresión, 1);

            // Si no son compatibles
            if (!Types.IsCompatible(instance, presentType, evaluation.Tipo))
            {
                instance.WriteError($"No se puede convertir <{evaluation.Tipo.Description}> en <{presentType.Description}>");
                return new("", new(), true);
            }

            // Asigna el valor
            field.Value = new(evaluation.Value, evaluation.Tipo);
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

            var result = MicroRunner.Runner(instance, context, funcContext, line, 1);
            return result;
        }

        // Ejecutar funciones
        else if (Fields.IsFunction(line, out nombre, out string @params))
        {


            if (nombre == "print")
            {

                Eval eval = MicroRunner.Runner(instance, context, funcContext, @params, 1);

                instance.Write(eval.Value.ToString());
                return new("", new(), true);

            }

            else if (nombre == "type")
            {

                var eval = MicroRunner.Runner(instance, context, funcContext, @params, 1);

                string tipodes = eval.Tipo.Description;
                return new($"<{tipodes}>", new("string"));

            }

            else if (nombre == "typeof")
            {


                var f = context[@params.Trim()];

                string tipodes = f?.Tipo.Description ?? "null";
                return new($"<{tipodes}>", new("string"));

            }



            // Funciones definidas por el usuario.
            Function? function = (from F in instance.Functions
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
                return new("", function.Type, (function.Type.Description == "" || function.Type.Description == null));
            }

            // Contextos nuevos.
            Context newContext = new();
            FuncContext newFuncContext = FuncContext.GenerateContext(function);

            // Interprete de líneas.
            foreach (var codeLine in function.CodeLines)
                Interprete(instance, newContext, newFuncContext, codeLine, 0);

            // Si la función no retorno nada o es void
            if (newFuncContext.IsVoid || !newFuncContext.IsReturning)
                return new(true);


            // Nueva evaluación.
            return new Eval(newFuncContext.Value.Element, newFuncContext.Value.Tipo);

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
            Eval eval = MicroRunner.Runner(instance, context, funcContext, line, 1);

            // Cambia los estados.
            funcContext.IsReturning = true;

            // Tipo es incompatible
            if (!Types.IsCompatible(instance, funcContext.WaitType, eval.Tipo))
            {
                instance.WriteError($"La función '{funcContext.Name}' del tipo <{funcContext.WaitType.Description}> no puede retornar valores del tipo <{eval.Tipo.Description}>.");
                return new(true);
            }

            // Valores
            funcContext.Value.Element = eval.Value;
            funcContext.Value.Tipo = eval.Tipo;
            return new(true);

        }

        instance.WriteError($"Expression invalida '{line}' en modo '{level}'");
        return new(true);

    }


}
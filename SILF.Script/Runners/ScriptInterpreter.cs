using SILF.Script.Validations;

namespace SILF.Script.Runners;


internal class ScriptInterpreter
{


    /// <summary>
    /// Interprete de líneas.
    /// </summary>
    /// <param name="instance">Instancia de SILF.</param>
    /// <param name="line">Linea.</param>
    public static Eval Interprete(Instance instance, Context context, string line, short level = 0)
    {

        // Preparador
        line = line.Normalize().Trim();

        // Si esta vacio
        if (string.IsNullOrWhiteSpace(line))
            return new("", new(), true);

        // Es una variable
        var isVar = Fields.IsVar(line);
        var isConst = Fields.IsConst(line);

        // Definición de variable
        if (isConst.success)
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateConst(instance, context, isConst.name, isConst.expresion);

            // Respuesta
            return new("", new(), true);

        }

        // Definición de variable
        else if (isVar.success)
        {

            // Crea la constante
            bool canCreate = Actions.Fields.CreateVar(instance, context, isVar.name, isVar.type, isVar.expresion);

            // Respuesta
            return new("", new(), true);

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
            return new Eval(line, boolType);
        }

        // Es asignación
        else if (Fields.IsAssignment(line, out var nombre, out var operador, out var expresión))
        {

            var field = context.GetField(nombre);

            if (field == null)
            {
                instance.WriteError($"No existe el campo '{nombre}'.");
                return new("", new(), true);
            }


            if (field.Isolation != Isolation.ReadAndWrite & field.Isolation != Isolation.Write)
            {
                instance.WriteError($"El campo '{nombre}' no se puede se puede sobrescribir.");
                return new("", new(), true);
            }

            var eval = MicroRunner.Runner(instance, context, expresión, 1);
            if (field.Tipo != eval.Tipo)
            {
                instance.WriteError($"No se puede convertir <{eval.Tipo.Description}> en <{field.Tipo.Description}>");
                return new("", new(), true);
            }

            field.Value = eval.Value;

            return new("", new(), true);
        }

        // Elementos (Variables, constantes)
        else if (level == 1 && !line.EndsWith(')'))
        {

            var getValue = context.GetField(line.Trim());

            if (getValue == null)
            {
                instance.WriteError($"No existe el elemento '{line.Trim()}'");
                return new("", new(), true);
            }

            return new Eval((instance.Environment == Environments.PreRun) ? "" : getValue.Value, getValue.Tipo, false);

        }

        // Paréntesis
        else if (line.StartsWith('(') && line.EndsWith(')'))
        {
            line = line.Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);

            var result = MicroRunner.Runner(instance, context, line, 1);
            return result;
        }

        // Ejecutar funciones
        else if (Fields.IsFunction(line, out nombre, out string @params))
        {


            if (nombre == "print")
            {

                var eval = MicroRunner.Runner(instance, context, @params, 1);

                instance.Write(eval.Value.ToString());
                return new("", new(), true);

            }

            else if (nombre == "type")
            {

                var eval = MicroRunner.Runner(instance, context, @params, 1);

                string tipodes = eval.Tipo.Description;
                return new($"<{tipodes}>", new("string"));

            }


            instance.WriteWarning($"Ejecutando '{nombre}' con '{@params}'");

        }


        instance.WriteError($"Expression invalida '{line}' en modo '{level}'");
        return new("", new(), true);

    }


}
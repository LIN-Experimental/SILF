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

        // Es una variable
        var isVar = Actions.Fields.IsVar(line);

        // Definición de variable
        if (isVar.success)
        {

            if (level != 0)
            {
                instance.WriteError("No puedes declarar variables en este contexto.");
                return new("", new(), true);
            }

            var value = MicroRunner.Runner(instance, context, isVar.expresion, 1);

            if (value.IsVoid)
            {
                instance.WriteError("El resultado de la expresión es void.");
                return new("", new(), true);
            }


            var type = instance.Tipos.Where(T => T.Description == isVar.type).FirstOrDefault();
            var field = new Field(isVar.name, value.Value, type, Isolation.ReadAndWrite);

            var can = context.SetField(field);

            if (!can)
                instance.WriteError("Variable duplicada.");

            return new("", new(), true);

        }

        // Es numero
        else if (Actions.Options.IsNumber(line))
        {
            var numberType = instance.Tipos.Where(T => T.Description == "number").FirstOrDefault();
            return new Eval(line, numberType);
        }

        //Devuelve la cadena de string
        else if (line.StartsWith('"') && line.EndsWith('"') && level == 1)
        {

        }

        // Es Booleano
        else if (Actions.Options.IsBool(line))
        {
            var boolType = instance.Tipos.Where(T => T.Description == "bool").FirstOrDefault();
            return new Eval(line, boolType);
        }

        // Es asignación
        else if (Actions.Fields.IsAssignment(line, out var nombre, out var operador, out var expresión))
        {

            var field = context.GetField(nombre);

            if (field == null)
            {
                instance.WriteError("No existe la variable.");
                return new("", new(), true);
            }

            var eval = Interprete(instance, context, expresión, 1);


            field.Value = eval.Value;

            return new("", new(), true);
        }


        else if (level == 1 && !line.EndsWith(')'))
        {

            var getValue = context.GetField(line.Trim());

            if (getValue == null)
                return new("", new(), true);

            return new Eval(getValue.Value, getValue.Tipo, false);


        }

        // 
        else if (line.StartsWith('(') && line.EndsWith(')'))
        {
            line = line.Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line).Remove(0, 1);
            line = Microsoft.VisualBasic.Strings.StrReverse(line);

            var result = MicroRunner.Runner(instance, context, line, 1);
            return result;
        }



        // Ejecutar funciones
        else if (Actions.Fields.IsFunction(line, out nombre, out string @params))
        {
            instance.WriteWarning($"Ejecutando '{nombre}' con '{@params}'");

            if (nombre == "print")
            {

                var eval = MicroRunner.Runner(instance, context, @params, 1);

                instance.Write(eval.Value.ToString() + $"<{eval.Tipo.Description}>");
                return new("", new(), true);

            }


        }



        instance.WriteError("Expression invalida");
        return new("", new(), true);
    }


}
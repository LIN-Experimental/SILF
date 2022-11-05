using static System.Runtime.InteropServices.JavaScript.JSType;

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

            var value = Interprete(instance, context, isVar.expresion);

            if (value.IsVoid)
            {
                instance.WriteError("El resultado de la expresión es void.");
                return new("",new(), true);
            }


            var type = instance.Tipos.Where(T => T.tipo == isVar.type).FirstOrDefault();
            var field = new Field(isVar.name, value.Value, type, Isolation.ReadAndWrite);

            var can = context.SetField(field);

            if (!can)
                instance.WriteError("Variable duplicada.");

        }

        // Es numero
        else if (Actions.Options.IsNumber(line))
        {
            var numberType = instance.Tipos.Where(T => T.tipo == "number").FirstOrDefault();
            return new Eval(line, numberType);
        }

        //Devuelve la cadena de string
        else if (line.StartsWith('"') && line.EndsWith('"') && level == 1)
        {

        }

        // Es Booleano
        else if (Actions.Options.IsBool(line))
        {
            var boolType = instance.Tipos.Where(T => T.tipo == "bool").FirstOrDefault();
            return new Eval(line, boolType);
        }

        // Es asignación
        else if (Actions.Fields.IsAssing(line, out var nombre, out var operador, out var expresion))
        {

            var field = context.GetField(nombre);

            if (field == null)
            {
                instance.WriteError("No existe la variable.");
                return new("", new(), true);
            }

            var eval = Interprete(instance, context, expresion, 1);


            field.Value = eval.Value;

            return new("", new(), true);
        }





        instance.WriteError("Expression invalida");
        return new("", new(), true);
    }


}
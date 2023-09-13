using SILF.Script.Runners;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SILF.Script.Actions;


internal class Fields
{


    private static bool IsValidName(string nombre)
    {
        // Comprueba si la cadena está vacía o es nula.
        if (string.IsNullOrEmpty(nombre))
        {
            return false;
        }

        // Comprueba si el nombre contiene caracteres inválidos.
        if (nombre.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
        {
            return false;
        }

        // Comprueba si el nombre es una palabra clave de C#.
        if (EsPalabraClaveCSharp(nombre))
        {
            return false;
        }

        // Si todas las comprobaciones pasan, el nombre es válido.
        return true;
    }


    static bool EsPalabraClaveCSharp(string nombre)
    {
        // Lista de palabras clave de C# (puedes ampliarla según sea necesario).
        string[] palabrasClave = { "function", "let", "const" };

        // Comprueba si el nombre está en la lista de palabras clave.
        return palabrasClave.Contains(nombre);
    }


    public static Tipo? GetTipo(Instance instance, string tipo)
    {
        var type = instance.Tipos.Where(T => T.Description == tipo);

        if (type.Any())
            return type.ElementAt(0);

        return null;
    }




    /// <summary>
    /// Crear nueva variable
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="context">Contexto</param>
    /// <param name="expression">Expression</param>
    public static bool CreateVar(Instance instance, Context context, string name, string type, string expression)
    {

        // Validar Nombre
        bool isValidName = IsValidName(name);

        if (!isValidName)
        {
            instance.WriteError($"El nombre '{name}' es invalido.");
            return false;
        }

        // Validar Tipo
        Tipo? tipo = null;

        // Tipo explicito
        if (type != "let")
        {

            // 
            tipo = GetTipo(instance, type);

            if (tipo == null)
            {
                instance.WriteError($"El tipo '{type}' es invalido.");
                return false;
            }

        }

        // Obtiene el valor
        var value = MicroRunner.Runner(instance, context, expression, 1);

        if (value.IsVoid)
        {
            instance.WriteError($"El valor de la variable '{name}' no puede ser void");
            return false;
        }

        if (value.Tipo != tipo && tipo != null)
        {
            instance.WriteError($"El tipo <{value.Tipo.Description}> no puede ser convertido en <{tipo.Value.Description}>.");
            return false;
        }

        var field = new Field(name, (instance.Environment == Environments.PreRun) ? "" : value.Value, value.Tipo, Isolation.ReadAndWrite);

        var can = context.SetField(field);

        if (!can)
        {
            instance.WriteError("campo duplicada.");
            return false;
        }


        return true;

    }



    /// <summary>
    /// Crear nueva constante
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="context">Contexto</param>
    /// <param name="expression">Expression</param>
    public static bool CreateConst(Instance instance, Context context, string name, string expression)
    {

        // Validar Nombre
        bool isValidName = IsValidName(name);

        if (!isValidName)
        {
            instance.WriteError($"El nombre '{name}' es invalido.");
            return false;
        }


        // Obtiene el valor
        var value = MicroRunner.Runner(instance, context, expression, 1);

        if (value.IsVoid)
        {
            instance.WriteError($"El valor de la constante '{name}' no puede ser void");
            return false;
        }


        var field = new Field(name, (instance.Environment == Environments.PreRun) ? "" : value.Value, value.Tipo, Isolation.Read);

        var can = context.SetField(field);

        if (!can)
        {
            instance.WriteError("campo duplicada.");
            return false;
        }


        return true;


    }



}

namespace SILF.Script.Validations;

internal class Fields
{

    /// <summary>
    /// Una expresión es la declaración de una variable.
    /// </summary>
    /// <param name="line">Expresión</param>
    public static (string type, string name, bool success) IsNotValuableVar(Instance instance, string line)
    {
        string patron = @"\b(grupo|valor)\s+(\w+)\s+(?<!\S)\b";

        var coincidencia = Regex.Match(line, patron);

        if (coincidencia.Success)
        {
            string tipo = coincidencia.Groups[1].Value;


            var type = instance.Library.Exist(tipo);

            if (type == null)
                return ("", "", false);

            string nombre = coincidencia.Groups[2].Value;
            return (tipo, nombre, true);
        }

        return (string.Empty, string.Empty, false);

    }


    /// <summary>
    /// Una expresión es la asignación a una variable
    /// </summary>
    /// <param name="line">Expresión</param>
    public static bool IsAssignment(string line, out string nombre, out string operador, out string expression)
    {
        string patron = @"^(.+)\s*=\s*(.+)$"; // Patrón para buscar asignaciones de valores

        Match coincidencia = Regex.Match(line, patron);

        if (coincidencia.Success)
        {
            nombre = coincidencia.Groups[1].Value;
            expression = coincidencia.Groups[2].Value;
            operador = "=";
            return true;
        }

        nombre = "";
        operador = "";
        expression = "";
        return false;

    }


    /// <summary>
    /// Una expresión es la asignación a una variable
    /// </summary>
    /// <param name="line">Expresión</param>
    public static bool IsPipe(string line, out string evaluador, out string receptor)
    {
        string patron = @"^(.+)\s*\|>\s*(.+)$"; // Patrón para buscar asignaciones de valores

        Match coincidencia = Regex.Match(line, patron);

        if (coincidencia.Success)
        {
            evaluador = coincidencia.Groups[1].Value;
            receptor = coincidencia.Groups[2].Value;
            return true;
        }

        evaluador = "";
        receptor = "";
        return false;

    }


}
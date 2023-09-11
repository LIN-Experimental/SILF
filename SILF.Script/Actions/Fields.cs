namespace SILF.Script.Actions;


internal class Fields
{


    /// <summary>
    /// Es una variable
    /// </summary>
    /// <param name="line">Linea a evaluar</param>
    public static (string type, string name, string expresion, bool success) IsVar(string line)
    {
        string patron = @"(\w+)\s+(\w+)\s*=\s*(.+)";

        var coincidencia = Regex.Match(line, patron);

        if (coincidencia.Success)
        {
            string tipo = coincidencia.Groups[1].Value;
            string nombre = coincidencia.Groups[2].Value;
            string valor = coincidencia.Groups[3].Value;
            return (tipo, nombre, valor, true);
        }

        return (string.Empty, string.Empty, string.Empty, false);

    }



    public static bool IsAssignment(string line, out string nombre, out string operador, out string expression)
    {
        string patron = @"\b(\w+)\s+(\?\?=|\=)\s*([^;]+);"; // Patrón para buscar asignaciones de valores

        Match coincidencia = Regex.Match(line, patron);

        if (coincidencia.Success)
        {
            nombre = coincidencia.Groups[1].Value.Trim();
            operador = coincidencia.Groups[2].Value.Trim();
            expression = coincidencia.Groups[3].Value.Trim();
            return true;
        }

        nombre = "";
        operador = "";
        expression = "";
        return false;

    }




    public static bool IsFunction(string line,out string name, out string parámetros)
    {
        string patron = @"(\w+)\((.*)\)";

        var coincidencia = Regex.Match(line, patron);

        name = "";
        parámetros = "";

        if (coincidencia.Success)
        {
             name = coincidencia.Groups[1].Value;
             parámetros = coincidencia.Groups[2].Value;
            return true;
        }

        return false;

    }






}
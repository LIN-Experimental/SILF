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



    public static bool IsAssing(string line, out string nombre, out string operador, out string expresion)
    {
        string patron = @"\b(\w+)\s+(\?\?=|\=)\s*([^;]+);"; // Patrón para buscar asignaciones de valores

        Match coincidencia = Regex.Match(line, patron);

        if (coincidencia.Success)
        {
            nombre = coincidencia.Groups[1].Value;
            operador = coincidencia.Groups[2].Value;
            expresion = coincidencia.Groups[3].Value.Trim();
            return true;
        }

        nombre = "";
        operador = "";
        expresion = "";
        return false;

    }
}
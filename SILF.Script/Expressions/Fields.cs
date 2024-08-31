namespace SILF.Script.Expressions;


internal class Fields
{

    /// <summary>
    /// Evalúa si una expresión es una variable.
    /// </summary>
    /// <param name="line">Linea.</param>
    /// <param name="fieldResult">Resultado de field</param>
    public static bool IsVar(string line, out FieldResult fieldResult)
    {

        // Patron regex
        string patron = @"(\w+)\s+(\w+)\s*=\s*(.+)";

        // Coincidencia
        var coincidencia = Regex.Match(line, patron);

        // Si es correcto
        if (coincidencia.Success)
        {
            fieldResult = new FieldResult()
            {
                Type = coincidencia.Groups[1].Value,
                Name = coincidencia.Groups[2].Value,
                Expression = coincidencia.Groups[3].Value,
                Success = true
            };
            return true;
        }

        // Valores vacíos
        fieldResult = new FieldResult()
        {
            Type = string.Empty,
            Name = string.Empty,
            Expression = string.Empty,
            Success = false
        };

        // Incorrecto
        return false;
    }



    /// <summary>
    /// Evalúa si una expresión es una constante.
    /// </summary>
    /// <param name="line">Linea</param>
    /// <param name="fieldResult">Resultado de field</param>
    public static bool IsConst(string line, out FieldResult fieldResult)
    {

        // Patron regex
        string patron = @"const\s+(\w+)\s*=\s*(.+)";

        // Coincidencia
        var coincidencia = Regex.Match(line, patron);

        // Si es correcto
        if (coincidencia.Success)
        {
            fieldResult = new()
            {
                Name = coincidencia.Groups[1].Value,
                Expression = coincidencia.Groups[2].Value,
                Success = true
            };
            return true;
        }

        // Retorna valor negativo
        fieldResult = new(false);

        return false;

    }



    public static bool IsNew(string line, out string type, out string values)
    {

        string pattern = @"^new\s+(\w+)\((.*?)\)$";

        Match match = Regex.Match(line, pattern);

        if (match.Success)
        {
            string typeName = match.Groups[1].Value;
            type = typeName;
            values = match.Groups[2].Value;
            return true;
        }

        type = "";
        values = "";
        return false;

    }


}
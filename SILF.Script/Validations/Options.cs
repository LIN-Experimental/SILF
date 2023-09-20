namespace SILF.Script.Validations;


internal static class Options
{


    /// <summary>
    /// Es un valor numérico
    /// </summary>
    /// <param name="expression">Expresión</param>
    public static bool IsNumber(string expression)
    {
        bool isNumber = decimal.TryParse(expression, out _);

        return isNumber;

    }



    /// <summary>
    /// Es un valor booleano
    /// </summary>
    /// <param name="expression">Expresión</param>
    public static bool IsBool(string expression)
    {

        string[] values = { "false", "true" };

        return values.Contains(expression);

    }



    /// <summary>
    /// Una expresión es un string valido
    /// </summary>
    /// <param name="input">Expresión</param>
    public static bool IsString(string input)
    {
        string pattern = @"^""[^""]*""$";

        return Regex.IsMatch(input, pattern);
    }



    /// <summary>
    /// Devuelve si el nombre es valido
    /// </summary>
    /// <param name="nombre">Nombre a validar</param>
    public static bool IsValidName(string nombre)
    {
        // Comprueba si la cadena está vacía o es nula.
        if (string.IsNullOrEmpty(nombre))
            return false;
        
        // Comprueba si el nombre contiene caracteres inválidos.
        if (nombre.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            return false;
        
        // Comprueba si el nombre es una palabra clave de C#.
        if (EsPalabraClave(nombre))
            return false;
        
        // Si todas las comprobaciones pasan, el nombre es válido.
        return true;
    }



    /// <summary>
    /// Si un valor es una palabra clave
    /// </summary>
    /// <param name="nombre">Texto a validar</param>
    public static bool EsPalabraClave(string nombre)
    {
        // Lista de palabras clave de C# (puedes ampliarla según sea necesario).
        string[] palabrasClave = { "function", "let", "const" };

        // Comprueba si el nombre está en la lista de palabras clave.
        return palabrasClave.Contains(nombre);
    }


}
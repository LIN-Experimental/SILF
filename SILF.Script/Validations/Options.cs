namespace SILF.Script.Validations;


internal class Options
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



}
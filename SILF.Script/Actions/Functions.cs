namespace SILF.Script.Actions;


internal class Functions
{


    /// <summary>
    /// Si una línea es la definición de una función.
    /// </summary>
    /// <param name="input">Entrada.</param>
    /// <param name="tipo">Salida del tipo</param>
    /// <param name="nombre">Salida del nombre</param>
    /// <param name="parameters">Parametros</param>
    public static bool Match(string input, out string tipo, out string nombre, out List<string> parameters)
    {

        tipo = "";
        nombre = "";
        parameters = new();
        string pattern = @"function\s+(?<tipoRetorno>\w+)\s+(?<nombre>\w+)\s*\((?<parametros>.+?)?\)";

        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            tipo = match.Groups["tipoRetorno"].Value;
            nombre = match.Groups["nombre"].Value;
            string parametersCrude = match.Groups["parametros"].Value;

            // Dividir los parámetros en una lista si es necesario
            parameters = [.. parametersCrude.Split([','])];
            return true;
        }
        return false;

    }


}
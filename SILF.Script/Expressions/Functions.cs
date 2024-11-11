namespace SILF.Script.Expressions;

internal class Functions
{

    /// <summary>
    /// Una expresión es la llamada a una función
    /// </summary>
    /// <param name="line">Expresión</param>
    public static bool IsFunction(string line, out string name, out string parámetros)
    {
        try
        {
            line = line.Trim();

            name = "";
            parámetros = "";

            var i = line.IndexOf('(');

            if (i <= 0)
                return false;

            name = line[..i];

            if (!Options.IsValidName(name))
                return false;

            line = line.Remove(0, i);

            line = line[1..(line.Length - 1)];

            parámetros = line;

            return true;
        }
        catch
        {
            name = "";
            parámetros = "";
            return false;
        }


    }


    /// <summary>
    /// Es una expresión de llamada a un índice.
    /// </summary>
    /// <param name="line">Linea.</param>
    /// <param name="name">Nombre de la lista.</param>
    /// <param name="parámetros">Parámetros (índice para la lista.)</param>
    public static bool IsIndex(string line, out string name, out string parámetros)
    {
        try
        {
            line = line.Trim();

            name = "";
            parámetros = "";

            var i = line.IndexOf('[');

            if (i <= 0)
                return false;

            name = line[..i];

            if (!Options.IsValidName(name))
                return false;

            line = line.Remove(0, i);

            line = line[1..(line.Length - 1)];

            parámetros = line;

            return true;
        }
        catch
        {
            name = "";
            parámetros = "";
            return false;
        }
    }


}
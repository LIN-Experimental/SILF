namespace SILF.Script.Utilities;


internal static class StringExtends
{


    /// <summary>
    /// Reversar una cadena.
    /// </summary>
    /// <param name="cadena">Cadena.</param>
    public static string Reverse(this string cadena)
    {

        char[] caracteres = cadena.ToCharArray();
        Array.Reverse(caracteres);
        return  new string(caracteres);

    }



    /// <summary>
    /// Reversar una cadena.
    /// </summary>
    /// <param name="cadena">Cadena.</param>
    public static string Sub(this string cadena, int i, int count)
    {
        try
        {
          return  cadena.Substring(i, count);
        }
        catch
        {
        }
        return "";

    }



}
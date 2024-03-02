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



}
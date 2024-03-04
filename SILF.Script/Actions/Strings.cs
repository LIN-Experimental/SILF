namespace SILF.Script.Actions;


public class Strings
{


    /// <summary>
    /// Separar una cadena interpolada con llaves.
    /// </summary>
    /// <param name="cadena">Cadena.</param>
    public static IEnumerable<string> SepararPorLlaves(string cadena)
    {

        // Variables.
        List<string> result = [];

        // Elementos.
        int counter = 0;
        bool isCommand = false;

        StringBuilder command = new();
        StringBuilder value = new();

        // Recorrer caracteres.
        foreach (char @char in cadena)
        {

            // Caracter de abierta.
            if (@char == '{')
            {
                // Aumentar.
                counter++;

                if (!isCommand)
                {
                    isCommand = true;
                    result.Add(value.ToString());
                    value = new();
                }
            }

            // Caracter de cerrado.
            else if (@char == '}')
            {
                // Decrementar escape.
                counter--;
                if (isCommand && counter == 0)
                {
                    isCommand = false;
                    result.Add(command.ToString() + "}");
                    command = new();
                    continue;
                }
            }

            // Es un comando.
            if (isCommand)
                command.Append(@char);

            else
                value.Append(@char);

        }


        // Es un comando.
        if (isCommand)
            result.Add(command.ToString());

        // Valor.
        else
            result.Add(value.ToString());

        // Retornar.
        return result;

    }


}

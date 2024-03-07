namespace SILF.Script.Actions;


internal class Blocks
{

    /// <summary>
    /// Separar por bloques una expresión.
    /// </summary>
    /// <param name="value">expresión a separar</param>
    /// <param name="char">char de separación</param>
    public static List<CodeBlock> Separar(string value, char @char = ',')
    {

        // Valor nulo.
        if (value == null)
            return new();

        // CodeBlocks.
        List<CodeBlock> codeBlocks = new();

        // Preparación.
        value = value.Normalize().Trim();

        // Separación de los bloques.
        {

            int counter = 0;
            bool isString = false;
            string? fragment = null;

            // Separador por bloques
            foreach (char carácter in value)
            {
                bool BD2 = false;

                // "{" y "("
                if ((carácter == '{' || carácter == '(' || carácter == '[') && !isString)
                    counter += 1;

                // "}" y ")"
                else if ((carácter == '}' | carácter == ')' | carácter == ']') && !isString)
                    counter -= 1;

                // Entrada o salida de proceso de string
                else if (carácter == '"' || carácter == '\'')
                    isString = !isString;

                // Nuevo bloque 
                else if (carácter == @char && !isString && counter == 0)
                {
                    if (fragment != null)
                        codeBlocks.Add(new(fragment.Trim()));

                    fragment = null;
                    BD2 = true;
                }

                // Inserción del carácter
                if (BD2 == false)
                {
                    if (fragment == null)
                        fragment = carácter.ToString();
                    else
                        fragment += carácter;
                }

            }

            {
                if (fragment != null)
                    codeBlocks.Add(new(fragment.Trim()));
            }


        }


        // Codeblocks
        return codeBlocks;
    }



    /// <summary>
    /// Obtiene los operadores.
    /// </summary>
    /// <param name="value">valor</param>
    public static List<Bloque> GetOperators(string value, Instance instance)
    {

        // Valor nulo.
        if (value == null)
            return new();

        // CodeBlocks.
        List<Bloque> codeBlocks = new();

        // Preparación.
        value = value.Normalize().Trim();

        List<string> operadoresErroneos = ["|>", "++", "--"];

        // Separación de los bloques.
        {

            int counter = 0;
            bool isString = false;
            string? fragment = null;

            char[] operators = ['<', '>', '!', '=', '+', '-', '/', '*'];

            // Separador por bloques
            for (int i = 0; i < value.Length; i++)
            {
                char carácter = value[i];

                bool BD2 = false;

                // "{" y "("
                if ((carácter == '{' || carácter == '(' || carácter == '[') && !isString)
                    counter += 1;

                // "}" y ")"
                else if ((carácter == '}' | carácter == ')' | carácter == ']') && !isString)
                    counter -= 1;

                // Entrada o salida de proceso de string
                else if (carácter == '"' || carácter == '\'')
                    isString = !isString;

                // Verificación de operadores erróneos
                else if (operadoresErroneos.Contains(value.Sub(i, 2)) && !isString && counter == 0)
                {
                    i++;

                    // Inserción del carácter
                    if (BD2 == false)
                    {
                        if (fragment == null)
                            fragment = carácter.ToString();
                        else
                            fragment += carácter;
                    }

                    carácter = value[i];
                }

                //Nuevo bloque 
                else if ((operators.Contains(carácter)) & !isString & counter == 0)
                {
                    string @operator = carácter.ToString();
                    if (value.ElementAtOrDefault(i + 1) == '=')
                    {
                        i++;
                        @operator += '=';
                    }

                    if (fragment != null)
                        codeBlocks.Add(new()
                        {
                            IsOperator = false,
                            Value = fragment
                        });

                    fragment = null;
                    codeBlocks.Add(new()
                    {
                        IsOperator = true,
                        Value = @operator
                    });

                    continue;

                }

                // Inserción del carácter
                if (BD2 == false)
                {
                    if (fragment == null)
                        fragment = carácter.ToString();
                    else
                        fragment += carácter;
                }

            }

            {
                if (fragment != null)
                    codeBlocks.Add(new()
                    {
                        IsOperator = false,
                        Value = fragment.Trim()
                    });

            }


        }

        // Codeblocks
        return codeBlocks;
    }


}



internal class Bloque
{

    public bool IsOperator { get; set; }
    public string Value { get; set; } = string.Empty;

}
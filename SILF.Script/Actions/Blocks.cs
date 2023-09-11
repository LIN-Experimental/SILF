﻿namespace SILF.Script.Actions;


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
                if ((carácter == '{' || carácter == '(') && !isString)
                    counter += 1;

                // "}" y ")"
                else if ((carácter == '}' | carácter == ')') && !isString)
                        counter -= 1;
                
                // Entrada o salida de proceso de string
                else if (carácter == '"')
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

}
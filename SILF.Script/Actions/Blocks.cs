namespace SILF.Script.Actions;


internal class Blocks
{

    /// <summary>
    /// Separar por bloques una expresión.
    /// </summary>
    /// <param name="value">expresión a separar</param>
    /// <param name="char">char de separación</param>
    public static SubBloqueList Separar(string value, char @char = ',')
    {


















        if (value == null)
            return new();


        SubBloqueList Bloque = new();
        {
            value = value.Trim();
            SubBloque ss = new();

            int c = 0;
            bool isString = false;
            string? frame = null;

            //Separador por bloques
            foreach (char E in value)
            {
                bool BD2 = false;

                // "{" y "("
                if (E == '{' | E == '(')
                {
                    if (isString == false)
                        c += 1;
                }

                // "}" y ")"
                else if (E == '}' | E == ')')
                {
                    if (isString == false)
                        c -= 1;
                }

                // Entrada o salida de proseso de string
                else if (E == '"')
                {
                    if (isString == true)
                        isString = false;
                    else
                        isString = true;
                }

                //Concatenacion 
                else if (E == '+' & isString == false & c == 0)
                {
                    if (frame != null)
                        ss.Valores.Add(frame.Trim());

                    frame = null;
                    BD2 = true;
                }

                //Nuevo bloque 
                else if (E == @char & isString == false & c == 0)
                {
                    if (frame != null)
                        ss.Valores.Add(frame.Trim());

                    if (ss.Valores.Count > 0)
                        Bloque.Bloques.Add(ss);

                    frame = null;
                    ss = new SubBloque();
                    BD2 = true;
                }

                //Insercion del caracter
                if (BD2 == false)
                {
                    if (frame == null)
                        frame = E.ToString();
                    else
                        frame += E;
                }

            }

            {
                if (frame != null)
                    ss.Valores.Add(frame.Trim());

                if (ss.Valores.Count > 0)
                    Bloque.Bloques.Add(ss);
            }

        }

        return Bloque;
    }

}
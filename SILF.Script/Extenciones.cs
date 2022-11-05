using System.Globalization;



namespace SILF.Script
{

    public class FirsIndex
    {
        public readonly char? caracter = null;
        public readonly int index = 0;

        public FirsIndex(char? caracter, int index)
        {
            this.caracter = caracter;
            this.index = index;
        }
    }



    public static class ExtensionMethodsStrings
    {


        public static FirsIndex FirsIndex(this string value, char a, char b)
        {
            int f1 = value.IndexOf(a);
            int f2 = value.IndexOf(b);

            if (f1 == -1 & f2 == -1)
                return new(null, 0);

            if (f1 == -1)
                f1 = int.MaxValue;
            if (f2 == -1)
                f2 = int.MaxValue;


            if (f2 < f1)
                return new(b, f2);
            else
                return new(a, f1);

        }




        /// <summary>
        /// Devuelve la cadena al reves
        /// </summary>
        /// <returns></returns>
        public static string Reverse(this string Expression)
        {
            if (Expression == null)
            {
                return "";
            }
            int length = Expression.Length;
            if (length == 0)
            {
                return "";
            }

            checked
            {
                int num = length - 1;
                for (int i = 0; i <= num; i++)
                {
                    UnicodeCategory unicodeCategory = char.GetUnicodeCategory(Expression[i]);
                    if (unicodeCategory == UnicodeCategory.Surrogate || unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.EnclosingMark)
                    {
                        return Expression.InternalStrReverse(i, length);
                    }
                }

                char[] array = Expression.ToCharArray();
                Array.Reverse(array);
                return new string(array);
            }
        }


        private static string InternalStrReverse(this string Expression, int SrcIndex, int Length)
        {
            StringBuilder stringBuilder = new StringBuilder(Length);
            stringBuilder.Length = Length;
            TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(Expression, SrcIndex);
            if (!textElementEnumerator.MoveNext())
            {
                return "";
            }

            int i = 0;
            checked
            {
                int num = Length - 1;
                for (; i < SrcIndex; i++)
                {
                    stringBuilder[num] = Expression[i];
                    num--;
                }

                int num2 = textElementEnumerator.ElementIndex;
                while (num >= 0)
                {
                    SrcIndex = num2;
                    num2 = !textElementEnumerator.MoveNext() ? Length : textElementEnumerator.ElementIndex;
                    for (i = num2 - 1; i >= SrcIndex; i--)
                    {
                        stringBuilder[num] = Expression[i];
                        num--;
                    }
                }

                return stringBuilder.ToString();
            }
        }



        /// <summary>
        /// Devuelve los datos que esten entre la primera aparicion de <paramref name="D1"/> y la ultima aparicion de <paramref name="D2"/>
        /// </summary>
        /// <param name="From"></param>
        /// <param name="D1"></param>
        /// <param name="D2"></param>
        /// <returns></returns>
        public static string ExtractFrom(this string From, string D1, string D2)
        {


            if (From == null)
                return "";

            if (D1 == null)
                return "";

            if (D2 == null)
                return "";

            if (From.Contains(D1) == false & From.Contains(D2) == false)
                return "";

            string E1 = From;
            E1 = E1.Remove(0, E1.IndexOf(D1) + D1.Length);
            E1 = E1.Reverse();
            string E2 = E1;
            E2 = E2.Remove(0, E2.IndexOf(D2) + D2.Length);
            E2 = E2.Reverse();
            return E2;
        }


        public static string ExtractFrom(this string From, string D1, string D2, dynamic x)
        {


            if (From == null)
                return null;

            if (D1 == null)
                return null;

            if (D2 == null)
                return null;


            if (From.Contains(D1) == false & From.Contains(D2) == false)
                return null; ;

            string E1 = From;
            E1 = E1.Remove(0, E1.IndexOf(D1) + D1.Length);
            E1 = E1.Reverse();
            string E2 = E1;
            E2 = E2.Remove(0, E2.IndexOf(D2) + D2.Length);
            E2 = E2.Reverse();
            return E2;
        }



        /// <summary>
        ///  Devuelve los datos que esten entre la primera aparicion de <paramref name="D1"/> y la primera aparicion de <paramref name="D2"/>
        /// </summary>
        /// <param name="From"></param>
        /// <param name="D1"></param>
        /// <param name="D2"></param>
        /// <returns></returns>
        public static string ExtractFrom2(this string From, string D1, string D2)
        {
            if (From == null)
                return "";

            if (D1 == null)
                return "";

            if (D2 == null)
                return "";

            string NOMME = From;
            NOMME = NOMME.Remove(0, NOMME.IndexOf(D1) + D1.Length);
            var Funct = NOMME.Substring(0, NOMME.IndexOf(D2));
            return Funct;

        }

        public static string ExtractFrom2(this string From, string D1, string D2, dynamic nul)
        {
            if (From == null)
                return null;

            if (D1 == null)
                return null;

            if (D2 == null)
                return null;

            if (From.Contains(D1) & From.Contains(D2))
            {
                string NOMME = From;
                NOMME = NOMME.Remove(0, NOMME.IndexOf(D1) + D1.Length);
                var Funct = NOMME.Substring(0, NOMME.IndexOf(D2));
                return Funct;
            }

            return null;



        }



        /// <summary>
        /// Cuenta cuantos de un caracter especifico hay en la cadena
        /// </summary>
        /// <param name="cadena"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static double CountSpecificChar(this string cadena, char ch)
        {
            if (cadena == null)
                return 0;

            double X = 0;
            foreach (char d in cadena)
            {
                if (d == ch)
                    X += 1;
            }
            return X;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string cadena)
        {
            double se;
            bool aux = double.TryParse(cadena, out se);

            return aux;
        }



        public static bool LowStartsWith(this string cadena, string com)
        {
            if (cadena == null)
                return false;

            if (cadena.ToLower().StartsWith(com))
                return true;

            return false;

        }


        public static bool IsColeccionType(this string cadena)
        {
            return IsValid.IsColeccionType(cadena);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static double ToDouble(this string cadena)
        {

            double se;
            bool aux = double.TryParse(cadena, out se);

            if (aux == false)
                return 0;
            return se;

        }


        public static int ToInt32(this string cadena)
        {
            int se;
            bool aux = int.TryParse(cadena, out se);

            if (aux == false)
                return 0;
            return se;

        }



        public static bool IsValidIndex(this List<string> lista, int i)
        {

            int len = lista.Count();

            if (len == 0)
                return false;

            else
            {
                if (i <= len - 1 & i >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }


        public static string ValidAndGet(this List<string> lista, int i)
        {

            if (lista.IsValidIndex(i))
            {
                return lista[i];
            }

            return "";

        }


    }


    public static class MetodI
    {



        public static string index(this string Value, int i)
        {
            if (Value.Length == 0) return "";

            if (i <= Value.Length - 1)
            {
                return Value[i].ToString();
            }
            else
            {
                return "";
            }
        }


        public static bool IsNumericType(this string Value)
        {
            if (Value == "int" | Value == "float")
            {
                return true;
            }

            return false;
        }

        public static List<string> Sepa(this string Value)
        {


            {
                string r = "";
                bool isString = false;

                foreach (char E in Value)
                {

                    if (E == '"')
                    {
                        if (isString == true)
                            isString = false;
                        else
                            isString = true;
                    }


                    if (E == ' ' & isString == false)
                    {
                        continue;
                    }

                    r += E;


                }

                Value = r;
            }


            List<string> Total = new();
            Total.Add(""); Total.Add(""); Total.Add("");

            {
                Value = Value.Trim();
                int c = 0;
                int co = 0;
                int i = -1;
                bool isString = false;
                bool isRecojer = false;
                string frame = null;

                try
                {
                    //Separador por bloques
                    foreach (char E in Value)
                    {
                        i += 1;
                        // bool BD2 = false;

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

                        //Separador
                        else if ((E == '=' | E == '!') & isString == false & c == 0)
                        {

                            if (co > 0)
                            {
                                co--;
                                continue;
                            }
                            else if (isRecojer == false)
                            {

                                string O = E.ToString();

                                if (Value[i + 1] == '=')
                                {
                                    co += 1;
                                    O += "=";
                                }

                                if (frame != null)
                                {
                                    Total[0] = frame;
                                    Total[1] = O;
                                }

                                frame = null;

                                isRecojer = true;
                                continue;
                            }


                        }

                        //Insercion del caracter

                        if (frame == null)
                            frame = E.ToString();
                        else
                            frame += E;


                    }

                }
                catch { }


                {
                    Total[2] = frame ?? "";
                }


            }



            return Total;

        }



    }



}
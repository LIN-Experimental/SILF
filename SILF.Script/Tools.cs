namespace SILF.Script
{

    /// <summary>
    /// Modulo de acceso a variables y obtencion de las mismas
    /// </summary>
    internal static partial class Mvars
    {


        /// <summary>
        /// Obtiene el indice donde se encuentra un elemento en la lista de accesos
        /// </summary>
        /// <param name="Varname">Nombre de la variable</param>
        /// <param name="Acceso">Lista de accesos</param>
        /// <returns></returns>
        public static long GetVarItem(string Varname, List<ID> Acceso)
        {
            if (Varname == null)
                return -1;

            foreach (var LINEA in Acceso)
            {
                if ((LINEA.Nombre ?? "") == (Varname.ToLower() ?? ""))
                    return LINEA.i;
            }

            return -1;
        }


        /// <summary>
        /// Obtiene el dato de una variable
        /// </summary>
        /// <param name="item">Indice donde esta alojada la variable</param>
        /// <param name="Base">Heap donde se encuentra</param>
        /// <returns></returns>
        public static string GetVar(int item, List<Objeto> Base)
        {
            string? value = Base[item].Valor;
            return value ?? "";
        }




        // Define el tipo de una variable, IMPORTANTE: RESERVADO AL LENGUAJE
        public static object WhatTypeIS(string contenido, bool contains)
        {
            int Strings = 0;
            int Ints = 0;
            int chars = 0;
            int Floats = 0;
            int Bools = 0;


            try
            {
                if (contains == true)
                {
                    Strings += 1;
                }

                if (contenido.IsNumeric())
                {
                    Ints += 1;
                }
                else
                {
                    Strings += 1;
                }

                if (contenido.IsNumeric() & (contenido.Contains(".") | contenido.Contains(",")))
                {
                    Ints = 0;
                    Floats += 1;
                }
                else if (Ints == 0)
                {
                    Strings += 1;
                }

                if (contenido.Contains(IsValid.ValorFalse) | contenido.Contains(IsValid.ValorTrue))
                {
                    Bools += 1;
                }

                if (contenido.Length == 1)
                {
                    if (Ints == 0)
                    {
                        chars += 1;
                    }
                }


                if (Strings > Ints)
                {
                    Ints = 0;
                }


                if (Ints > 0 & Strings == 0 & Bools == 0 & Floats == 0 & chars == 0)
                {
                    return "int";
                }

                if (Floats > 0 & Strings == 0 & Bools == 0 & Ints == 0 & chars == 0)
                {
                    return "float";
                }

                if (chars > 0 & Floats == 0 & Bools == 0 & Ints == 0)
                {
                    return "char";
                }

                if (Strings > 0 & Floats == 0 & Bools == 0 & Ints == 0 & chars == 0)
                {
                    return "string";
                }

                if (Bools > 0 & ((contenido ?? "") == IsValid.ValorFalse | (contenido ?? "") == IsValid.ValorTrue))
                {
                    return "bool";
                }
            }





            catch (Exception ex)
            {

            }
            return "obj";
        }



    }



    internal static partial class Tipos
    {


        /// <summary>
        /// Comprueba si el valor si pertence a un tipo superficialmenete
        /// </summary>
        /// <param name="value">Valor a confirmar</param>
        /// <returns></returns>
        public static bool IsString(string value)
        {
            return true;
        }


        /// <summary>
        /// Comprueba si el valor si pertence a un tipo superficialmenete
        /// </summary>
        /// <param name="value">Valor a confirmar</param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            int aux;
            bool aux2 = int.TryParse(value, out aux);

            if (aux2 == true)
                return true;
            else
                return false;
        }



        /// <summary>
        /// Comprueba si el valor si pertence a un tipo superficialmenete
        /// </summary>
        /// <param name="value">Valor a confirmar</param>
        /// <returns></returns>
        public static bool IsFloat(string value)
        {
            double aux;
            bool aux2 = double.TryParse(value, out aux);

            if (aux2 == true)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Comprueba si el valor si pertence a un tipo superficialmenete
        /// </summary>
        /// <param name="value">Valor a confirmar</param>
        /// <returns></returns>
        public static bool IsBool(string value)
        {
            if (value == "" | value == IsValid.ValorFalse | value == IsValid.ValorTrue)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Comprueba si el valor si pertence a un tipo superficialmenete
        /// </summary>
        /// <param name="value">Valor a confirmar</param>
        /// <returns></returns>
        public static bool IsChar(string value)
        {
            if (value.Length == 1 | value.Length == 0)
                return true;
            else
                return false;
        }


    }

}
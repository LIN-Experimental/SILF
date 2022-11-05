namespace SILF.Script
{



    /// <summary>
    /// Determina la cantidad de seguridad que debe tener el compilador y el interpretador
    /// </summary>
    public enum Flex : byte
    {
        /// <summary>
        /// Nula: No comprueba el tipo y solo los concatena
        /// </summary>
        Nulo,

        /// <summary>
        /// Flexible: Aunque no sea el mismo tipo trata de castearlo y si es posible concatena
        /// </summary>
        FlexibleField,

        /// <summary>
        /// Segura: Solo concatena los valores que tienen el mismo tipo
        /// </summary>
        Seguro

    }


    /// <summary>
    /// Determina que tipo de valor debe devolver, Si el Dato (ByVal) o la ubicacion (ByRef)
    /// </summary>
    public enum ByQ : byte
    {
        /// <summary>
        /// Devuelve la ubicacion de un elemento en base al Heap (Base)
        /// Almacena la direccion de memoria del objeto
        /// </summary>
        ByRefer,

        /// <summary>
        /// Devuelve el valor o dato de un elemento
        /// Almacena el dato del objeto
        /// </summary>
        Byvalue
    }


    public class Operador
    {

        public StringBuilder Despu = new();
        public StringBuilder Antes = new();
        public string oper = "";

        public Operador(StringBuilder Despu, StringBuilder Antes, string oper)
        {
            this.Despu = Despu;
            this.Antes = Antes;
            this.oper = oper.Trim();
        }
    }



    /// <summary>
    /// En este modulo estan todos los micro-interpretadores de SILF
    /// </summary>
    static partial class InterB
    {

        public static Operador SepararOperadores(string? Value)
        {

            if (Value == null)
                return new(new(), new(), "");


            {
                Value = Value.Trim();

                int e = 0;
                int c = 0;
                bool isString = false;
                bool isOper = false;
                StringBuilder Antes = new();
                string Oper = "";
                StringBuilder Despu = new();


                //Separador por bloques
                int i = -1;
                foreach (char E in Value)
                {
                    bool BD2 = false;
                    i += 1;

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


                    else if (isOper == true)
                    {
                        isOper = false;
                        continue;
                    }

                    //Nuevo bloque 
                    else if ((E == '=' | E == '<' | E == '>' | E == '!') & isString == false & c == 0)
                    {
                        e = 1;
                        if ((E == '<' | E == '>' | E == '!' | E == '=') & Value[i + 1] == '=')
                        {
                            Oper = E + "=";
                            isOper = true;
                            continue;
                        }

                        else
                            Oper = E.ToString();
                        continue;
                    }

                    //Insercion del caracter
                    if (BD2 == false)
                    {
                        if (e == 0)
                            Antes.Append(E);
                        if (e == 1)
                            Despu.Append(E);
                    }

                }

                return new(Despu, Antes, Oper);

            }


        }


        public static SubBloqueList Separar(string? Value, char cc = ',')
        {

            if (Value == null)
                return new();


            SubBloqueList Bloque = new();
            {
                Value = Value.Trim();
                SubBloque ss = new();

                int c = 0;
                bool isString = false;
                string? frame = null;

                //Separador por bloques
                foreach (char E in Value)
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
                    else if (E == cc & isString == false & c == 0)
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








        /// <summary>
        /// 
        /// </summary>
        /// <param name="Estancia"></param>
        /// <param name="Accion"></param>
        /// <param name="Modo"></param>
        /// <param name="Accesos"></param>
        /// <param name="Base"></param>
        /// <param name="Sett"></param>
        /// <param name="ListaA"></param>
        public static void MicroForArray(Instance Estancia, string Accion, int Modo, List<ID> Accesos, List<Objeto> Base, StatusApp Sett, List<string> ListaA)
        {

            // Separa
            SubBloqueList lista = Separar(Accion);

            // Conjunto de bloques compilados
            BloqueComList post = new();


            //// Procesamiento e interpretacion
            //{

            //    foreach (SubBloque block in lista.Bloques)
            //    {
            //        BloqueComp actual = new();

            //        foreach (string value in block.Valores)
            //        {
            //            if (value == null)
            //                continue;

            //            string? valor;
            //            string? tipo;

            //            var Res = InterpretadorS.Interprete(Estancia, value, 1, Accesos, Base, Sett, new(), "", Flex.Nulo, ByQ.Byvalue);

            //            if (Res == null)
            //                continue;


            //            if (Res.ContainsBlocks == true)
            //                actual.Items.AddRange(Res.Lista.Items);

            //            else
            //            {
            //                valor = Res.Dato ?? null;
            //                tipo = Res.Tipo ?? null;

            //                if (valor == null | tipo == null)
            //                    continue;

            //                var S = new BloqueBase(valor, tipo);
            //                actual.Items.Add(S);
            //            }

            //        }

            //        if (actual.Items.Count > 0)
            //            post.Bloques.Add(actual);

            //    }

            //}


            //Uniendo informacion
            {
                foreach (BloqueComp block in post.Bloques)
                {

                    StringBuilder actual = new();

                    foreach (BloqueBase item in block.Items)
                    {
                        actual.Append(item.Valor);
                    }


                    //Agrega el dato
                    ListaA.Add(actual.ToString());

                }

            }

        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static List<string> JustSeparar(string Value, char cc = ',')
        {

            List<string> valores = new();
            int c = 0;
            bool isString = false;
            string? frame = null;

            //Separador por bloques
            foreach (char E in Value)
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

                //Nuevo bloque 
                else if (E == cc & isString == false & c == 0)
                {

                    if (frame != null)
                        valores.Add(frame);

                    frame = null;
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
                    valores.Add(frame);
            }


            return valores;
        }








        // En desuso (/X/)
        public static void MicroForGenerics(Instance Estancia, string Accion, int Modo, List<ID> Accesos, List<Objeto> Base, StatusApp Sett, List<string> ListaA, string Tipo)
        {

            Tipo = Tipo.ToLower().Trim();

            Flex Flex = Flex.Seguro;
            ByQ By;

            if (IsValid.ForVal(Tipo))
                By = ByQ.Byvalue;
            else
                By = ByQ.ByRefer;


            if (Tipo == "obj")
            {
                MicroForArray(Estancia, Accion, 1, Accesos, Base, Sett, ListaA);
                return;
            }


            // Separa
            SubBloqueList lista = Separar(Accion);

            // Conjunto de bloques compilados
            BloqueComList post = new();


            // Procesamiento e interpretacion
            //{

            //    foreach (SubBloque block in lista.Bloques)
            //    {
            //        BloqueComp actual = new();

            //        foreach (string value in block.Valores)
            //        {
            //            if (value == null)
            //                continue;

            //            string? valor;
            //            string? tipo;

            //            var Res = InterpretadorS.Interprete(Estancia, value, 1, Accesos, Base, Sett,new(), Tipo, Flex, By);

            //            if (Res == null)
            //                continue;


            //            if (Res.ContainsBlocks == true)
            //                actual.Items.AddRange(Res.Lista.Items);

            //            else
            //            {
            //                valor = Res.Dato ?? null;
            //                tipo = Res.Tipo ?? null;

            //                if (valor == null | tipo == null)
            //                    continue;

            //                var S = new BloqueBase(valor, tipo);
            //                actual.Items.Add(S);
            //            }

            //        }

            //        if (actual.Items.Count > 0)
            //            post.Bloques.Add(actual);

            //    }

            //}



            //Uniendo informacion
            {
                foreach (BloqueComp block in post.Bloques)
                {

                    StringBuilder actual = new();

                    foreach (BloqueBase item in block.Items)
                    {

                        // Comprobacion de la flexibilidad


                        // Flex Nula
                        if (Flex == Flex.Nulo & item.Valor != null)
                            actual.Append(item.Valor);


                        // Flex Flexible
                        else if (Flex == Flex.FlexibleField & item.Valor != null & item.Tipo != null)
                        {
                            if (item.Tipo == Tipo)
                                actual.Append(item.Valor);

                            else
                            {
                                var Aux3 = IsValid.Converted(item.Valor ?? "", Tipo ?? "");
                                if (Aux3.CanBe == true)
                                    actual.Append(Aux3.Dato ?? "");

                                else
                                    Estancia.OnDebug($"El tipo '{item.Tipo ?? " "}' no pudo ser convertido en '{Tipo ?? " "}'");

                            }
                        }


                        // Flex seguro
                        else if (Flex == Flex.Seguro & item.Valor != null & item.Tipo != null)
                        {

                            // Si es el mismo tipo
                            if ((item.Tipo.ToLower().Trim() ?? "") == (Tipo ?? ""))
                                actual.Append(item.Valor);


                            // Si la nececidad es tipo OBJ
                            else if (Tipo == "obj")
                                actual.Append(item.Valor);



                            // Si el retorno obtenido es tipo OBJ
                            else if (item.Tipo == "obj")
                            {
                                var Aux3 = IsValid.Converted(item.Valor ?? "", Tipo ?? "");
                                if (Aux3.CanBe == true)
                                    actual.Append(Aux3.Dato);

                                else
                                    Estancia.OnDebug($"Esta base object no fue compatible con el tipo '{Tipo}'");

                            }
                            else
                                Estancia.OnDebug($"El tipo '{item.Tipo}' no es compatible con el tipo '{Tipo}'");

                        }


                    }


                    //Agrega el dato
                    ListaA.Add(actual.ToString());

                }

            }
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="Estancia"></param>
        /// <param name="Accion"></param>
        /// <param name="Modo"></param>
        /// <param name="Accesos"></param>
        /// <param name="Base"></param>
        /// <param name="Sett"></param>
        /// <param name="Type"></param>
        /// <param name="Flex"></param>
        /// <param name="By"></param>
        /// <returns></returns>
        public static BloqueComp MicroGetBlocksComp(Instance Estancia, string Accion, int Modo, List<ID> Accesos, List<Objeto> Base, StatusApp Sett, string Type, Flex Flex, ByQ By)
        {


            // Separa
            SubBloqueList lista = Separar(Accion);

            return new();
            // Procesamiento e interpretacion
            //{
            //    BloqueComp total = new();
            //    foreach (SubBloque block in lista.Bloques)
            //    {

            //        foreach (string value in block.Valores)
            //        {
            //            if (value == null)
            //                continue;

            //            string valor;
            //            string tipo;

            //            var Res = InterpretadorS.Interprete(Estancia, value, 1, Accesos, Base, Sett, new(), Type, Flex, By);

            //            if (Res == null)
            //                continue;


            //            if (Res.ContainsBlocks == true)
            //                total.Items.AddRange(Res.Lista.Items);

            //            else
            //            {
            //                valor = Res.Dato ?? null;
            //                tipo = Res.Tipo ?? null;

            //                if (valor == null | tipo == null)
            //                    continue;

            //                var S = new BloqueBase(valor, tipo);
            //                total.Items.Add(S);
            //            }

            //        }


            //    }

            //    return total;
            //}

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="Estancia"></param>
        /// <param name="Accion"></param>
        /// <param name="Modo"></param>
        /// <param name="Accesos"></param>
        /// <param name="Base"></param>
        /// <param name="Sett"></param>
        /// <param name="ListaA"></param>
        /// <param name="By"></param>
        public static void MicroForSeparar(Instance Estancia, string Accion, int Modo, List<ID> Accesos, List<Objeto> Base, StatusApp Sett, List<string> ListaA, ByQ By)
        {

            // Separa
            SubBloqueList lista = Separar(Accion);

            // Conjunto de bloques compilados
            BloqueComList post = new();


            // Procesamiento e interpretacion
            //{

            //    foreach (SubBloque block in lista.Bloques)
            //    {
            //        BloqueComp actual = new();

            //        foreach (string value in block.Valores)
            //        {
            //            if (value == null)
            //                continue;

            //            string? valor;
            //            string? tipo;

            //            var Res = InterpretadorS.Interprete(Estancia, value, 1, Accesos, Base, Sett, new(), "", Flex.Nulo, By);

            //            if (Res == null)
            //                continue;


            //            if (Res.ContainsBlocks == true)
            //                actual.Items.AddRange(Res.Lista.Items);

            //            else
            //            {
            //                valor = Res.Dato ?? null;
            //                tipo = Res.Tipo ?? null;

            //                if (valor == null | tipo == null)
            //                    continue;

            //                var S = new BloqueBase(valor, tipo);
            //                actual.Items.Add(S);
            //            }

            //        }

            //        if (actual.Items.Count > 0)
            //            post.Bloques.Add(actual);

            //    }

            //}


            //Uniendo informacion
            {
                foreach (BloqueComp block in post.Bloques)
                {

                    StringBuilder actual = new();

                    foreach (BloqueBase item in block.Items)
                        actual.Append(item.Valor);


                    //Agrega el dato
                    ListaA.Add(actual.ToString());

                }

            }

        }


    }


}
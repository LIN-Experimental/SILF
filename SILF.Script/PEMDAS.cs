namespace SILF.Script;


internal class PEMDAS
{











    private Instance estancia;
    private BloqueList lista1;


    public PEMDAS(BloqueList lista1, Instance estancia)
    {
        this.estancia = estancia;
        this.lista1 = lista1;
    }



    /// <summary>
    /// Resulve las operaciones y concatenaciones de S# en orden.
    /// </summary>
    public void Solve()
    {
        A();  //Operadores aritmeticos
        N();  //Resuelve Nulos
        E();  //Exponentes
        MD(); //Multiplicacion y Divicion
        P();  //%
        AS(); //Adiccion y sustraccion
        L_OY(); //Logico & y |
        T(); // Terniarios
    }




    //Resolver Multiplicaciones y diviciones
    private void L_OY()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "&", "|" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }

            //Hace la operacion


            switch (o.valor)
            {
                case "&":
                    {
                        if (a.tipo == "bool" & b.tipo == "bool" & (a.valor == IsValid.ValorTrue & b.valor == IsValid.ValorTrue | a.valor == "true" & b.valor == "true" | a.valor == "1" & b.valor == "1"))
                        {
                            valor = IsValid.ValorTrue;
                            tipo = "bool";
                        }
                        else if (a.tipo == "bool" & b.tipo == "bool")
                        {

                        }
                        else
                        {
                            estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                            valor = IsValid.ValorFalse;
                            tipo = "bool";
                        }
                        break;
                    }


                case "|":
                    {
                        if (a.tipo == "bool" & b.tipo == "bool" & (a.valor == IsValid.ValorTrue | b.valor == IsValid.ValorTrue | a.valor == "true" | b.valor == "true" | a.valor == "1" | b.valor == "1"))
                        {
                            valor = IsValid.ValorTrue;
                            tipo = "bool";
                        }
                        else if (a.tipo == "bool" & b.tipo == "bool")
                        {

                        }
                        else
                        {
                            estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                            valor = IsValid.ValorFalse;
                            tipo = "bool";
                        }
                        break;
                    }

            }

            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        L_OY();
    }

    //Resolver Nulos o vacios
    private void N()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "??" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }


            // segun el operador
            switch (o.valor)
            {
                case "??":
                    {
                        if (a.tipo == "null" | a.valor == "")
                        {
                            valor = b.valor;
                            tipo = b.tipo;
                        }
                        else
                        {
                            valor = a.valor;
                            tipo = a.tipo;
                        }
                        break;
                    }


            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        N();
    }

    //Resolver Exponenetes
    private void E()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "^" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }

            //Hace la operacion
            if (a.tipo.IsNumericType() & b.tipo.IsNumericType())
            {
                a.valor = a.valor.Replace(".", ",");
                b.valor = b.valor.Replace(".", ",");
                switch (o.valor)
                {
                    case "^":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & (double.TryParse(b.valor, out vl2) == true | b.valor == ""))
                            {
                                //Proceso
                                string total = Math.Pow(vl1, vl2).ToString();
                                valor = total;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = "";
                            }

                            if (a.tipo == b.tipo)
                                tipo = a.tipo;
                            else
                                tipo = "float";

                            break;
                        }
                }
            }

            //Si no fue compatible
            else
            {
                estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                valor = "";
                tipo = "obj";
            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        E();
    }

    //Resolver Multiplicaciones y diviciones
    private void MD()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "/", "*" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }

            //Hace la operacion
            if (a.tipo.IsNumericType() & b.tipo.IsNumericType())
            {
                a.valor = a.valor.Replace(".", ",");
                b.valor = b.valor.Replace(".", ",");
                switch (o.valor)
                {
                    case "/":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 / vl2).ToString();
                                valor = total;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = "";
                            }

                            if (a.tipo == b.tipo)
                                tipo = a.tipo;
                            else
                                tipo = "float";

                            break;
                        }


                    case "*":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 * vl2).ToString();
                                valor = total;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = "";
                            }

                            if (a.tipo == b.tipo)
                                tipo = a.tipo;
                            else
                                tipo = "float";

                            break;
                        }

                }
            }

            //Multiplicador de string
            else if (a.tipo == "string" & b.tipo == "int" & o.valor == "*")
            {
                int.TryParse(b.valor, out int vl1);
                for (var t = 0; t < vl1; t++)
                {
                    valor += a.valor ?? "";
                }
                tipo = "string";
            }

            //Si no fue compatible
            else
            {
                estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                valor = "";
                tipo = "obj";
            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        MD();
    }

    //Resolver %
    private void P()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "%" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }

            //Hace la operacion
            if (a.tipo.IsNumericType() & b.tipo.IsNumericType())
            {
                a.valor = a.valor.Replace(".", ",");
                b.valor = b.valor.Replace(".", ",");
                switch (o.valor)
                {
                    case "^":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 % vl2).ToString();
                                valor = total;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = "";
                            }

                            if (a.tipo == b.tipo)
                                tipo = a.tipo;
                            else
                                tipo = "float";

                            break;
                        }
                }
            }

            //Si no fue compatible
            else
            {
                estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                valor = "";
                tipo = "obj";
            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        P();
    }

    //Resolver Sumas y restas
    private void AS()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "+", "-" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }

            //Hace la operacion
            if (a.tipo.IsNumericType() & b.tipo.IsNumericType())
            {
                a.valor = a.valor.Replace(".", ",");
                b.valor = b.valor.Replace(".", ",");
                switch (o.valor)
                {
                    case "+":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 + vl2).ToString();
                                valor = total;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = "";
                            }

                            if (a.tipo == b.tipo)
                                tipo = a.tipo;
                            else
                                tipo = "float";

                            break;
                        }


                    case "-":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 - vl2).ToString();
                                valor = total;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = "";
                            }

                            if (a.tipo == b.tipo)
                                tipo = a.tipo;
                            else
                                tipo = "float";

                            break;
                        }


                }
            }


            //Concatenacion
            else if ((a.tipo == "string" | b.tipo == "string") & o.valor == "+")
            {
                ;
                valor = string.Concat(IsValid.Converted(a.valor, "string").Dato, IsValid.Converted(b.valor, "string").Dato);
                tipo = "string";
            }

            //Si no fue compatible
            else
            {
                estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                valor = "";
                tipo = "obj";
            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        AS();
    }

    //Resolver Aritmeticos
    private void A()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "<", ">", "<=", ">=" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque a = new("", "0", "null");
            Bloque o = new("", true);
            Bloque b = new("", "0", "null");


            //Obtiene los datos
            if (i + 1 <= lista1.items.Count)
            {
                a = lista1.items[i - 1];
                o = lista1.items[i];
                b = lista1.items[i + 1];
                needElim = true;
            }
            else
            {
                needElim = false;
            }

            //Hace la operacion
            if (a.tipo.IsNumericType() & b.tipo.IsNumericType())
            {
                a.valor = a.valor.Replace(".", ",");
                b.valor = b.valor.Replace(".", ",");
                switch (o.valor)
                {
                    case "<":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 < vl2).ToString();
                                tipo = "bool";
                                valor = IsValid.Converted(total, "bool").Dato;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = IsValid.ValorFalse;
                                tipo = "bool";
                            }

                            break;
                        }

                    case ">":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 > vl2).ToString();
                                tipo = "bool";
                                valor = IsValid.Converted(total, "bool").Dato;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = IsValid.ValorFalse;
                                tipo = "bool";
                            }

                            break;
                        }

                    case ">=":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 >= vl2).ToString();
                                tipo = "bool";
                                valor = IsValid.Converted(total, "bool").Dato;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = IsValid.ValorFalse;
                                tipo = "bool";
                            }

                            break;
                        }

                    case "<=":
                        {

                            double vl1 = 0;
                            double vl2 = 0;

                            //Trata de convertir el valor
                            if (double.TryParse(a.valor, out vl1) == true & double.TryParse(b.valor, out vl2) == true)
                            {
                                //Proceso
                                string total = (vl1 <= vl2).ToString();
                                tipo = "bool";
                                valor = IsValid.Converted(total, "bool").Dato;
                            }

                            // Si no se pudo convertir
                            else
                            {
                                valor = IsValid.ValorFalse;
                                tipo = "bool";
                            }

                            break;
                        }

                }
            }

            //Concatenacion
            else if (a.tipo == "string")
            {
                valor = a.valor + b.valor;
                tipo = "string";
            }

            //Si no fue compatible
            else
            {
                estancia.NewAdvertencia($"El operador {o.valor} no se puede aplica a valores <{a.tipo}> y <{b.tipo}>", "");
                valor = "";
                tipo = "obj";
            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        A();
    }

    //Resolver terniarios
    private void T()
    {

        // Comprueba si hay este operador
        string[] Operadores = { "?" };
        {
            bool bn = false;
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                {
                    bn = true;
                    break;
                }
            }

            if (bn == false)
                return;
        }


        //Obtencion la ubicacion
        int i = 0;
        {
            foreach (var item in lista1.items)
            {
                if (item.IsOper & Operadores.Contains(item.valor))
                    break;
                i += 1;
            }
        }


        //Ejecucion
        {
            bool needElim = false;
            string valor = "";
            string tipo = "";

            Bloque expresion = new("", "0", "null");
            Bloque o = new("", true);
            Bloque ytrue = new("", "0", "null");
            Bloque o2 = new("", true);
            Bloque yfalse = new("", "0", "null");


            //Obtiene los datos
            if (i + 3 <= lista1.items.Count)
            {
                expresion = lista1.items[i - 1];
                o = lista1.items[i];
                ytrue = lista1.items[i + 1];
                o2 = lista1.items[i + 2];
                yfalse = lista1.items[i + 3];
                needElim = true;
            }
            else
            {
                needElim = false;
            }


            // segun el operador
            switch (o.valor, o2.valor)
            {
                case ("?", ":"):
                    {
                        if (expresion.valor == IsValid.ValorTrue | expresion.valor == "1" | expresion.valor == "true")
                        {
                            valor = ytrue.valor;
                            tipo = ytrue.tipo;
                        }
                        else
                        {
                            valor = yfalse.valor;
                            tipo = yfalse.tipo;
                        }
                        break;
                    }


            }


            // Eliminacion de los datos
            if (needElim == true)
            {
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.RemoveAt(i - 1);
                lista1.items.Insert(i - 1, new(valor, "0", tipo));
            }


        }


        //Recursividas de la funcion
        T();
    }

}
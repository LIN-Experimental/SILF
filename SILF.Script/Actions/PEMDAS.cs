namespace SILF.Script.Actions;


internal class PEMDAS
{

    /// <summary>
    /// Instancia de la app.
    /// </summary>
    private Instance Instance;


    private List<Eval> Values = new();




    public PEMDAS(Instance instance, List<Eval> values)
    {
        this.Instance = instance;
        this.Values = values;
    }



    /// <summary>
    /// Resulve las operaciones y concatenaciones de S# en orden.
    /// </summary>
    public void Solve()
    {
    //    A();  //Operadores aritmeticos
    //    N();  //Resuelve Nulos
    //    E();  //Exponentes
        MD(); //Multiplicacion y Divicion
       // P();  //%
        AS(); //Adiccion y sustraccion
        //L_OY(); //Logico & y |
        //T(); // Terniarios
    }




    private void MD()
    {

        // Comprueba si hay este operador
        string[] operadores = { "*", "/" };
        var @continue = Contains(operadores);

        if (!@continue)
            return;

        // Obtiene la ubicación.
        int index = Values.FindIndex(T => T.Tipo.tipo == "operator" && operadores.Contains(T.Value.ToString()));


        // Evals
        Eval pre = Values[index - 1];
        Eval ope = Values[index];
        Eval pos = Values[index + 1];


        string valor = "";
        Tipo finalType = new();


        // Suma
        if (pre.Tipo.tipo == "number" && pos.Tipo.tipo == "number")
        {
            pre.Value = pre.Value.ToString().Replace(".", ",");
            pos.Value = pos.Value.ToString().Replace(".", ",");


            switch (ope.Value.ToString())
            {
                case "*":
                    {

                        double vl1 = 0;
                        double vl2 = 0;

                        //Trata de convertir el valor
                        if (double.TryParse(pre.Value.ToString(), out vl1) && double.TryParse(pos.Value.ToString(), out vl2))
                        {
                            //Proceso
                            string total = (vl1 * vl2).ToString();
                            valor = total;
                            finalType = pre.Tipo;
                        }

                        // Si no se pudo convertir
                        else
                        {
                            valor = "";
                        }

                        break;
                    }


                case "/":
                    {

                        double vl1 = 0;
                        double vl2 = 0;

                        //Trata de convertir el valor
                        if (double.TryParse(pre.Value.ToString(), out vl1) && double.TryParse(pos.Value.ToString(), out vl2))
                        {
                            //Proceso
                            string total = (vl1 / vl2).ToString();
                            valor = total;
                            finalType = pre.Tipo;
                        }

                        // Si no se pudo convertir
                        else
                        {
                            valor = "";
                        }

                        break;
                    }


            }

        }


        //Si no fue compatible
        else
        {
            Instance.WriteError("Error de operador");
        }

        {
            Values.RemoveRange(index, 3);
        }

        //Recursividas de la funcion
        MD();
    }

 

    private void AS()
    {

        // Comprueba si hay este operador
        string[] operadores = { "+", "-" };
        var @continue = Contains(operadores);

        if (!@continue)
            return;

        // Obtiene la ubicación.
        int index = Values.FindIndex(T => T.Tipo.tipo == "operator" && operadores.Contains(T.Value.ToString()));


        // Evals
        Eval pre = Values[index - 1];
        Eval ope = Values[index];
        Eval pos = Values[index + 1];


        string valor = "";
        Tipo finalType = new();


        // Suma
        if (pre.Tipo.tipo == "number" && pos.Tipo.tipo == "number")
        {
            pre.Value = pre.Value.ToString().Replace(".", ",");
            pos.Value = pos.Value.ToString().Replace(".", ",");


            switch (ope.Value.ToString())
            {
                case "+":
                    {

                        double vl1 = 0;
                        double vl2 = 0;

                        //Trata de convertir el valor
                        if (double.TryParse(pre.Value.ToString(), out vl1) && double.TryParse(pos.Value.ToString(), out vl2))
                        {
                            //Proceso
                            string total = (vl1 + vl2).ToString();
                            valor = total;
                            finalType = pre.Tipo;
                        }

                        // Si no se pudo convertir
                        else
                        {
                            valor = "";
                        }

                        break;
                    }


                case "-":
                    {

                        double vl1 = 0;
                        double vl2 = 0;

                        //Trata de convertir el valor
                        if (double.TryParse(pre.Value.ToString(), out vl1) && double.TryParse(pos.Value.ToString(), out vl2))
                        {
                            //Proceso
                            string total = (vl1 - vl2).ToString();
                            valor = total;
                            finalType = pre.Tipo;
                        }

                        // Si no se pudo convertir
                        else
                        {
                            valor = "";
                        }

                        break;
                    }


            }

        }


        // Concatenación
        else if ((pre.Tipo.tipo == "string" || pos.Tipo.tipo == "string") & ope.Value.ToString() == "+")
        {

            valor = pre.Value.ToString() + pos.Value.ToString();
            finalType = Instance.Tipos.Where(T => T.tipo == "string").FirstOrDefault();
        }

        //Si no fue compatible
        else
        {
            Instance.WriteError("Error de operador");
        }

        {
            Values.RemoveRange(index, 3);
        }

        //Recursividas de la funcion
        AS();
    }









    private bool Contains(string[] operators)
    {
        var have = Values.Where(T => T.Tipo.tipo == "operator" && operators.Contains(T.Value)).Any();
        return have;
    }








}
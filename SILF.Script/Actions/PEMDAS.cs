using SILF.Script.Objects;

namespace SILF.Script.Actions;


internal class PEMDAS
{


    /// <summary>
    /// Instancia actual de la app
    /// </summary>
    private Instance Instance { get; set; }


    /// <summary>
    /// Lista de valores
    /// </summary>
    private readonly List<Eval> Values = new();



    /// <summary>
    /// Nuevo solucionador PEMDAS
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="values">Valores a resolver</param>
    public PEMDAS(Instance instance, List<Eval> values)
    {
        this.Instance = instance;
        this.Values = values;
    }



    /// <summary>
    /// Resuelve las operaciones y concatenaciones de S# en orden.
    /// </summary>
    public void Solve()
    {

        SolveLL(); // Unarios
        //    A();  //Operadores aritmeticos
        //    N();  //Resuelve Nulos
        //    E();  //Exponentes
        SolveMD(); //Multiplicacion y Divicion
                   // P();  //%
        SolveSR(); //Adiccion y sustraccion
                   //L_OY(); //Logico & y |
                   //T(); // Terniarios

    }




    /// <summary>
    /// Soluciona multiplicaciones y divisiones.
    /// </summary>
    private void SolveMD()
    {
        // Operadores
        string[] operators = ["*", "/"];

        // Índice
        int index = Continue(operators, Values);

        // Si no hay indices
        if (index < 0)
            return;

        // Valores
        var (pre, ope, pos) = GetValues(index);


        if (pre == null || ope == null || pos == null)
        {
            Instance.WriteError("Error al realizar operaciones.");
            return;
        }

        // Valores finales
        object? value = null;
        Tipo type = new();

        // Operaciones
        if (pre.Object.Tipo.Description == "number" && pos.Object.Tipo.Description == "number")
        {
            // Si es preRun
            if (Instance.Environment == Environments.PreRun)
            {
                value = "0";
                type = pre.Object.Tipo;
            }

            // Valores
            else
            {
                // Valores numéricos
                bool canN1 = decimal.TryParse(pre.Object.GetValue().ToString()?.Replace(".", ","), out var number1);
                bool canN2 = decimal.TryParse(pos.Object.GetValue().ToString()?.Replace(".", ","), out var number2);

                // No se pudo convertir.
                if (!canN1 || !canN2)
                {
                    Instance.WriteError("Error de conversion numérica");
                    return;
                }

                // Segun el operador
                switch (ope.Object.GetValue().ToString())
                {

                    // Multiplicación
                    case "*":
                        {
                            // Total
                            decimal total = (number1 * number2);

                            // Proceso
                            value = total;
                            type = pre.Object.Tipo;

                            break;
                        }

                    // División
                    case "/":
                        {

                            // Total
                            decimal total = (number1 / number2);

                            // Proceso
                            value = total;
                            type = pre.Object.Tipo;

                            break;
                        }

                }
            }

        }


        // Operaciones
        else if (pre.Object.Tipo.Description == Library.LotNumber && pos.Object.Tipo.Description == Library.LotNumber)
        {

            // Si es preRun
            if (Instance.Environment == Environments.PreRun)
            {
                value = "0";
                type = pre.Object.Tipo;
            }

            // Valores
            else
            {


                // Segun el operador
                switch (ope.Object.GetValue().ToString())
                {

                    // Suma
                    case "*":
                        {

                            string final = ComplexMath.MultiplicarNumeros(pre.Object.GetValue().ToString(), pos.Object.GetValue().ToString());

                            value = final;
                            type = pre.Object.Tipo;

                            break;
                        }


                }
            }

        }



        // Si el operador no es compatible
        else
        {
            // Error
            Instance.WriteError($"El operador '{ope.Object.GetValue()}' no es compatible para tipos <{pre.Object.Tipo}> y <{pos.Object.Tipo}>");

        }


        // Eliminar los valores

        Values.RemoveRange(index - 1, 3);


        SILFObjectBase @base = Instance.Library.Get(type.Description);
        @base.SetValue(value);

        Values.Insert(index - 1, new()
        {
            IsVoid = false,
            Object = @base,
        });


        SolveMD();

    }



    /// <summary>
    /// Soluciona sumas y restas.
    /// </summary>
    private void SolveSR()
    {
        // Operadores
        string[] operators = ["+", "-"];

        // Índice
        int index = Continue(operators, Values);

        // Si no hay indices
        if (index < 0)
            return;

        // Valores
        var (pre, ope, pos) = GetValues(index);

        if (pre == null || ope == null || pos == null)
        {
            Instance.WriteError("Error al realizar operaciones.");
            return;
        }

        // Valores finales
        object? value = null;
        Tipo type = new();

        // Operaciones
        if (pre.Object.Tipo.Description == "number" && pos.Object.Tipo.Description == "number")
        {

            // Si es preRun
            if (Instance.Environment == Environments.PreRun)
            {
                value = "0";
                type = pre.Object.Tipo;
            }

            // Valores
            else
            {
                // Valores numéricos
                bool canN1 = decimal.TryParse(pre.Object.GetValue().ToString()?.Replace(".", ","), out decimal number1);
                bool canN2 = decimal.TryParse(pos.Object.GetValue().ToString()?.Replace(".", ","), out decimal number2);

                // No se pudo convertir.
                if (!canN1 || !canN2)
                {
                    Instance.WriteError("Error de conversion numérica");
                    return;
                }

                // Segun el operador
                switch (ope.Object.GetValue().ToString())
                {

                    // Suma
                    case "+":
                        {
                            // Total
                            decimal total = (number1 + number2);

                            // Proceso
                            value = total;
                            type = pre.Object.Tipo;

                            break;
                        }

                    // División
                    case "-":
                        {

                            // Total
                            decimal total = (number1 - number2);

                            // Proceso
                            value = total;
                            type = pre.Object.Tipo;

                            break;
                        }

                }
            }

        }


        // Operaciones
        else if (pre.Object.Tipo.Description == Library.LotNumber && pos.Object.Tipo.Description == Library.LotNumber)
        {

            // Si es preRun
            if (Instance.Environment == Environments.PreRun)
            {
                value = "0";
                type = pre.Object.Tipo;
            }

            // Valores
            else
            {

                
                // Segun el operador
                switch (ope.Object.GetValue().ToString())
                {

                    // Suma
                    case "+":
                        {

                            string final = ComplexMath.SumarNumeros(pre.Object.GetValue().ToString(), pos.Object.GetValue().ToString());

                            value = final;
                            type = pre.Object.Tipo;

                            break;
                        }

                    // División
                    case "-":
                        {

                            string final = ComplexMath.RestarNumeros(pre.Object.GetValue().ToString(), pos.Object.GetValue().ToString());

                            value = final;
                            type = pre.Object.Tipo;

                            break;
                        }

                }
            }

        }


        // Concatenar
        else if ((pre.Object.Tipo.Description == "string" || pos.Object.Tipo.Description == "string") & ope.Object.GetValue().ToString() == "+")
        {

            if (Instance.Environment == Environments.PreRun)
            {
                value = "";
                type = new("string");
            }
            else
            {
                value = pre.Object.GetValue()?.ToString() + pos.Object.GetValue()?.ToString();
                type = new("string");
            }

        }

        // Si el operador no es compatible
        else
        {
            // Error
            Instance.WriteError($"El operador '{ope.Object.GetValue()}' no es compatible para tipos <{pre.Object.Tipo}> y <{pos.Object.Tipo}>");

        }


        // Eliminar los valores
        try
        {
            Values.RemoveRange(index - 1, 3);


            SILFObjectBase @base = Instance.Library.Get(type.Description);
            @base.SetValue(value);

            Values.Insert(index - 1, new()
            {
                IsVoid = false,
                Object = @base,
            });

        }
        catch
        {

        }



        SolveSR();

    }



    /// <summary>
    /// Soluciona sumas y restas.
    /// </summary>
    private void SolveLL()
    {
        // Operadores
        string[] operators = ["!"];

        // Índice
        int index = Continue(operators, Values);

        // Si no hay indices
        if (index < 0)
            return;

        // Valores
        var (ope, pos) = GetTwoValues(index);

        if (pos == null || ope == null)
        {
            Instance.WriteError("Error al realizar operaciones.");
            return;
        }

        // Valores finales
        bool value = false;
        Tipo type = new();

        // Operaciones
        if (pos.Object is SILFBoolObject posObject)
        {


            // Segun el operador
            switch (ope.Object.GetValue().ToString())
            {

                // Suma
                case "!":
                    {

                        bool final = !posObject.GetValue();

                        value = final;

                        break;
                    }

            }


        }



        // Si el operador no es compatible
        else
        {
            // Error
            Instance.WriteError($"El operador '{ope.Object.GetValue()}' no es compatible para tipos <{pos.Object.Tipo}>");

        }


        // Eliminar los valores
        try
        {
            Values.RemoveRange(index, 2);

            Values.Insert(index, new()
            {
                IsVoid = false,
                Object = new SILFBoolObject()
                {
                    Value = value
                }
            });

        }
        catch
        {

        }



        SolveLL();

    }



    /// <summary>
    /// Comprueba si hay indices para ciertos operadores
    /// </summary>
    /// <param name="operators">Operadores a buscar</param>
    /// <param name="values">Valores</param>
    private static int Continue(string[] operators, List<Eval> values)
    {
        try
        {
            int index = values.FindIndex(T => T.Object.Tipo.Description == "operator" && operators.Contains(T.Object?.GetValue()?.ToString() ?? ""));
            return index;
        }
        catch
        {

        }
        return 0;

    }



    /// <summary>
    /// Obtiene los valores
    /// </summary>
    /// <param name="index">Índice</param>
    private (Eval? pre, Eval? ope, Eval? pos) GetValues(int index)
    {
        // Si no hay valores suficientes
        if (index <= 0 || index >= Values.Count)
        {
            Instance.WriteError("Error de operadores");
            return (null, null, null);
        }
        // Retorna elementos
        return (Values.ElementAtOrDefault(index - 1), Values.ElementAtOrDefault(index), Values.ElementAtOrDefault(index + 1));
    }



    /// <summary>
    /// Obtiene los valores
    /// </summary>
    /// <param name="index">Índice</param>
    private (Eval? pre, Eval? ope) GetTwoValues(int index)
    {
        // Si no hay valores suficientes
        if (index < 0 || index >= Values.Count)
        {
            Instance.WriteError("Error de operadores");
            return (null, null);
        }
        // Retorna elementos
        return (Values.ElementAtOrDefault(index), Values.ElementAtOrDefault(index + 1));
    }



}
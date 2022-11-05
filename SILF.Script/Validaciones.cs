namespace SILF.Script;


public static class Exist
{


    /// <summary>
    /// Devuelve si esxiste o no una funcion
    /// </summary>
    /// <param name="Estancia">Estancia de la app donde se va a buscar</param>
    /// <param name="Nombre">Nombre de la funcion que se va abuscar</param>
    /// <returns></returns>
    public static bool Funcion(Instance Estancia, string Nombre)
    {
        foreach (FuncionSILF W in Estancia.Funciones)
            if (W.Nombre == Nombre)
                return true;

        return false;
    }



    /// <summary>
    /// Comprueba si existe una variable
    /// </summary>
    /// <param name="Varname">Nombre de la variable</param>
    /// <param name="Acceso">Lista de accesos donde esta almacenada</param>
    /// <returns></returns>
    public static bool Variable(string Varname, List<ID> Acceso)
    {
        Varname = Varname.Trim();
        var var = Acceso.Where(i => i.Nombre == Varname);

        if (var.Count() >= 1)
            return true;

        return false;
    }



    /// <summary>
    /// Comprueba si existe una coleccion
    /// </summary>
    /// <param name="Varname">Nombre la coleccion</param>
    /// <param name="Acceso">Lista de accesos donde esta almacenada</param>
    /// <param name="Base">Base donde esta alamacenada</param>
    /// <returns></returns>
    public static object Coleccion(string Varname, List<ID> Acceso, List<Objeto> Base)
    {

        Varname = Varname.ToLower();

        foreach (ID obj in Acceso)
        {
            if (obj.Nombre == Varname)
            {
                if (Base[obj.i].Tipo.IsColeccionType())
                    return obj.i;
            }
        }
        return -1;
    }



}



public static class Create
{



    /// <summary>
    /// Crea un nuevo elemento Variable o Contante
    /// </summary>
    /// <param name="Base">Base jerarquica de los objetos</param>
    /// <param name="Accesos">Lista de accesos a la base</param>
    /// <param name="Nombre">Nombre del elemento</param>
    /// <param name="Tipo">Tipo del elemento</param>
    /// <param name="Seg">Seguridad del elemento</param>
    /// <param name="Value">Valor del elemento</param>
    /// <param name="Dynamic">Si es una variable dinamica o no</param>
    /// <returns></returns>
    public static double Elemento(List<Objeto> Base, List<ID> Accesos, string Nombre, string Tipo, Seguridad Seg, string Value, bool Dynamic = false)
    {
        //Genera el nuevo elemento
        int Location = Base.Count;
        var NewElement = new Objeto(Value, Tipo, Dynamic, Seg);
        Base.Add(NewElement);


        //Genera el acceso
        var ID = new ID(Nombre, Location);
        Accesos.Add(ID);

        return Location;
    }



    /// <summary>
    /// Crea una nuva coleccion tipo Array
    /// </summary>
    /// <param name="Base">Base jerarquica de los objetos</param>
    /// <param name="Accesos">Lista de accesos a la base</param>
    /// <param name="Nombre">Nombre del elemento</param>
    /// <param name="Value">Lista que se va a ingresar</param>
    /// <returns></returns>
    public static double Array(List<Objeto> Base, List<ID> Accesos, string Nombre, List<string> Value)
    {
        //Genera el nuevo elemento
        int Location = Base.Count;
        var NewElement = new Objeto(Value, "array");
        Base.Add(NewElement);

        //Genera el acceso
        var ID = new ID(Nombre, Location);
        Accesos.Add(ID);

        return Location;
    }



    /// <summary>
    /// Crea una nuva coleccion tipo Tuple
    /// </summary>
    /// <param name="Base">Base jerarquica de los objetos</param>
    /// <param name="Accesos">Lista de accesos a la base</param>
    /// <param name="Nombre">Nombre del elemento</param>
    /// <param name="Value">Lista que se va a ingresar</param>
    /// <returns></returns>
    public static double Tuple(List<Objeto> Base, List<ID> Accesos, string Nombre, List<string> Value)
    {
        //Genera el nuevo elemento
        int Location = Base.Count;
        var NewElement = new Objeto(Value, "tuple");
        Base.Add(NewElement);

        //Genera el acceso
        var ID = new ID(Nombre, Location);
        Accesos.Add(ID);

        return Location;
    }



    /// <summary>
    /// Crea una nuva coleccion tipo List con un tipo generico
    /// </summary>
    /// <param name="Base">Base jerarquica de los objetos</param>
    /// <param name="Accesos">Lista de accesos a la base</param>
    /// <param name="Nombre">Nombre del elemento</param>
    /// <param name="TipoGen">Tipo generico de la lista</param>
    /// <param name="Value">Lista nueva que se va a ingresar</param>
    /// <returns></returns>
    public static double CollGeneric(List<Objeto> Base, List<ID> Accesos, string Nombre, string TipoGen, List<string> Value)
    {
        //Genera el nuevo elemento
        int Location = Base.Count;
        var NewElement = new Objeto(Value, TipoGen, "list");
        Base.Add(NewElement);

        //Genera el acceso
        var ID = new ID(Nombre, Location);
        Accesos.Add(ID);

        return Location;
    }



    /// <summary>
    /// Crea una nuva coleccion tipo Stack con un tipo generico
    /// </summary>
    /// <param name="Base">Base jerarquica de los objetos</param>
    /// <param name="Accesos">Lista de accesos a la base</param>
    /// <param name="Nombre">Nombre del elemento</param>
    /// <param name="TipoGen">Tipo generico de la lista</param>
    /// <param name="Value">Lista nueva que se va a ingresar</param>
    /// <returns></returns>
    public static double Stack(List<Objeto> Base, List<ID> Accesos, string Nombre, string TipoGen, List<string> Value)
    {
        //Genera el nuevo elemento
        int Location = Base.Count;
        var NewElement = new Objeto(Value, TipoGen, "stack");
        Base.Add(NewElement);

        //Genera el acceso
        var ID = new ID(Nombre, Location);
        Accesos.Add(ID);

        return Location;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="Accesos"></param>
    /// <param name="Nombre"></param>
    /// <param name="indice"></param>
    /// <returns></returns>
    public static double Reference(List<ID> Accesos, string Nombre, int indice)
    {
        var ss = new ID(Nombre, indice);
        Accesos.Add(ss);
        return Accesos.Count - 1;
    }


}






/// <summary>
/// Comprueba sietos parametros
/// </summary>
public static class IsValid
{

    public const string ValorTrue = "​​‌‌‌‌";
    public const string ValorFalse = "‌‌‌​‍";



    public enum Er
    {
        IniciaConNumero,
        StarInvalidChar,
        InvalidChar,
        Si
    }


    /// <summary>
    /// Confirma si es un nombre valido o no
    /// </summary>
    /// <param name="Value">Nombre que se va a evaluar</param>
    /// <returns></returns>
    public static Er Name(string Value)
    {

        string[] sim = { " ", "|", "!", "#", "$", "%", "&", "/", "(", ")", "{", "}", "[", "]", "=", "?", "¡", "¿", "'", "@", "+", "-", "*", ".", "," };


        string first = Value.index(0);

        if (sim.Contains(first))
            return Er.StarInvalidChar;

        if (first.IsNumeric())
            return Er.IniciaConNumero;

        foreach (var t in Value)
        {

            if (sim.Contains(t.ToString()))
                return Er.InvalidChar;

        }




        return Er.Si;
    }







    /// <summary>
    /// Confirma si es un tipo de valor o no
    /// </summary>
    /// <param name="Value">Tipo a confirmar</param>
    /// <returns>Regresa si es un tipo de valor</returns>
    public static bool ForVal(string Value)
    {
        Value = Value.Trim().ToLower();

        string[] S = new[] { "obj", "int", "string", "float", "char", "date", "bool" };

        foreach (var u in S)
            if (u == Value)
                return true;

        return false;
    }


    public static bool IsPrimitive(string Value)
    {
        Value = Value.Trim().ToLower();

        string[] S = new[] { "int", "string", "float", "char", "date", "bool" };

        foreach (var u in S)
            if (u == Value)
                return true;

        return false;
    }



    /// <summary>
    /// Indica si el valor puede ser convertido a un tipo especifico
    /// </summary>
    /// <param name="Value">Valor a trasnformar</param>
    /// <param name="Tipo">Tipo al cual se quiere transformar</param>
    /// <returns></returns>
    public static Resultado Converted(string Value, string Tipo)
    {

        switch (Tipo.ToLower().Trim())
        {
            case "string":
                {
                    if (Value.Contains(ValorFalse))
                        Value = Value.Replace(ValorFalse, "false");
                    if (Value.Contains(ValorTrue))
                        Value = Value.Replace(ValorTrue, "true");
                    Resultado R = new Resultado(Value, true);
                    return R;
                }


            case "int":
                {
                    if (Value.Contains(ValorFalse))
                        Value = Value.Replace(ValorFalse, "0");
                    if (Value.Contains(ValorTrue))
                        Value = Value.Replace(ValorTrue, "1");

                    Value = Value.Replace(".", ",");
                    bool aux = double.TryParse(Value, out double v);

                    if (aux == false)
                        return new Resultado(Value, false);
                    else
                    {
                        var se = Math.Truncate(v);
                        return new Resultado(se.ToString(), true);
                    }

                }


            case "float":
                {
                    if (Value.Contains(ValorFalse))
                        Value = Value.Replace(ValorFalse, "0");
                    if (Value.Contains(ValorTrue))
                        Value = Value.Replace(ValorTrue, "1");

                    Value = Value.Replace(".", ",");
                    bool aux = double.TryParse(Value, out double v);

                    if (aux == false)
                        return new Resultado(Value, false);
                    else
                    {
                        return new Resultado(v.ToString(), true);
                    }

                }


            case "bool":
                {
                    if (Value == "1")
                        Value = ValorTrue;

                    if (Value == "0")
                        Value = ValorFalse;

                    if (Value == "true")
                        Value = ValorTrue;

                    if (Value == "false")
                        Value = ValorFalse;

                    if (Value == ValorFalse)
                    {
                        Resultado R = new Resultado(Value, true);
                        return R;
                    }

                    if (Value == ValorTrue)
                    {
                        Resultado R = new Resultado(Value, true);
                        return R;
                    }
                    return new Resultado(Value, false);
                }


            case "obj":
                {
                    Resultado R = new Resultado(Value, true);
                    return R;
                }


            case "char":
                {
                    if (Value.Contains(ValorFalse))
                        Value = Value.Replace(ValorFalse, "0");
                    if (Value.Contains(ValorTrue))
                        Value = Value.Replace(ValorTrue, "1");

                    if (Value.Length == 0 | Value.Length == 1)
                    {
                        Resultado R = new Resultado(Value, true);
                        return R;
                    }
                    else
                    {
                        Resultado R = new Resultado(Value, false);
                        return R;
                    }
                }


            case "date":
                {
                    break;
                }

        }
        Resultado J = new Resultado(Value, false);
        return J;
    }




    /// <summary>
    /// Comprueba si es un tipo de coleccion
    /// </summary>
    /// <param name="value">Tipo a comprobar</param>
    /// <returns></returns>
    public static bool IsColeccionType(string value)
    {
        var ColeccionTypes = new[] { "dictionary", "list", "array", "tuple", "stack" };
        value = value.Trim().ToLower();

        foreach (var W in ColeccionTypes)
        {
            if (W == value)
                return true;
        }
        return false;
    }



}



public class Resultado
{
    public string Dato { get; set; }
    public bool CanBe { get; set; }

    public Resultado(string Dto, bool Cb)
    {
        Dato = Dto;
        CanBe = Cb;
    }
}
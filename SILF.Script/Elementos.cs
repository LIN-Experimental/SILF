namespace SILF.Script;



/// <summary>
/// Parametros y datos compilados de una ejecucion
/// </summary>
public class Retorno
{

    public BloqueComp Lista;
    public BloqueList Lista1;

    private protected string _Dato;
    private protected string _Tipo;
    private protected bool _CointainBlocks;



    /// <summary>
    /// Genera un nuevo retorno
    /// </summary>
    /// <param name="Dato">Dato que se quiere entregar</param>
    /// <param name="Tipo">Tipo que se quiere entregar</param>
    public Retorno(string Dato, string Tipo)
    {
        Lista = new();
        this.Dato = Dato;
        this.Tipo = Tipo;
        ContainsBlocks = false;
    }



    /// <summary>
    /// Genera un nuevo retorno con una lista de bloques compilados dentro
    /// </summary>
    /// <param name="ListaContain"></param>
    public Retorno(BloqueComp ListaContain)
    {
        Lista = ListaContain;
        Dato = "";
        Tipo = "";
        ContainsBlocks = true;
    }

    public Retorno(BloqueList ListaContain)
    {
        Lista1 = ListaContain;
        Dato = "";
        Tipo = "";
        ContainsBlocks = true;
    }




    /// <summary>
    /// Obtiene o establece el Dato (Valor)
    /// </summary>
    public string Dato
    {
        get { return _Dato; }
        set { _Dato = value ?? ""; }
    }


    /// <summary>
    /// Obtiene o establece el Tipo
    /// </summary>
    public string Tipo
    {
        get { return _Tipo; }
        set { _Tipo = value.ToLower().Trim() ?? ""; }
    }


    /// <summary>
    /// Obtiene o establce si contine bloques
    /// </summary>
    public bool ContainsBlocks
    {
        get { return _CointainBlocks; }
        set { _CointainBlocks = value; }
    }


}







/// <summary>
/// Advertencia de SILF.Core
/// </summary>
public class Advertencia
{
    public string code;
    public string value;

    public Advertencia(string code, string value)
    {
        this.code = code;
        this.value = value;
    }
}



/// <summary>
/// Funcion de SILF.Core
/// </summary>
public class FuncionSILF
{

    private protected string _Nombre;
    private protected string _Tipo;
    private protected string _Ruta;
    public ArrayList Ejecucion = new ArrayList();
    public List<TypesAndValues> Argumetos = new List<TypesAndValues>();


    /// <summary>
    /// Genera una nueva funcion
    /// </summary>
    /// <param name="Nombre">Nombre de la funcion</param>
    /// <param name="Ruta">Ruta donde se leera la funcion</param>
    public FuncionSILF(string Nombre, string Ruta)
    {
        if (Nombre == null)
            Nombre = "";
        if (Ruta == null)
            Ruta = "";

        this.Nombre = Nombre;
        this.Ruta = Ruta;
    }



    public FuncionSILF()
    {

    }



    /// <summary>
    /// Obtiene o establece la ruta de la funcion
    /// </summary>
    /// <returns></returns>
    public string Ruta
    {
        get { return _Ruta; }
        set
        {
            _Ruta = value;

            var Count = 1;
            foreach (var LINEA1 in File.ReadLines(_Ruta))
            {
                var LINEA = LINEA1;

                if (Count == 1)
                {
                    var FuncionType = LINEA.ExtractFrom2("(", ")");
                    LINEA = LINEA.Remove(0, FuncionType.Count() + 2);


                    Tipo = FuncionType.ToLower().Trim();

                    if (LINEA.Trim().Contains(" "))
                    {
                        LINEA += ",";

                        var antes = "";
                        foreach (char d in LINEA)
                        {
                            if (d == ',')
                            {
                                antes = antes.Trim();

                                try
                                {
                                    string sT = antes.Split(' ')[0];
                                    string sN = antes.Split(' ')[1];

                                    var NewAdse = new TypesAndValues(sT, sN);
                                    Argumetos.Add(NewAdse);
                                }
                                catch (Exception ex)
                                {
                                    //  MessageBox.Show(ex.Message);
                                }

                                antes = "";
                            }
                            else
                                antes += d;
                        }
                    }
                    else
                    {
                    }

                    Count += 1;
                }
                else
                {
                    LINEA = LINEA.Trim();
                    Ejecucion.Add(LINEA);
                }
            }
        }
    }


    /// <summary>
    /// Obtiene o establece el nombre de la funcion
    /// </summary>
    /// <returns></returns>
    public string Nombre
    {
        get { return _Nombre; }
        set { _Nombre = value.ToLower().Trim(); }
    }


    /// <summary>
    /// Obtiene o establece el tipo de la funcion
    /// </summary>
    /// <returns></returns>
    public string Tipo
    {
        get { return _Tipo; }
        set { _Tipo = value.ToLower().Trim(); }
    }



    public class TypesAndValues
    {
        public string Tipo { get; set; } = "";
        public string Nombre { get; set; } = "";


        public TypesAndValues(string Tipo, string Nombre)
        {
            this.Tipo = Tipo;
            this.Nombre = Nombre;
        }
    }
}



/// <summary>
/// Genera un estatus de la app
/// </summary>
public class StatusApp
{
    public List<string> Recojedor = new List<string> { };

    private protected bool _OnFuncion;
    private protected string _FuncionTipo;
    private protected bool _NeedReturns;



    /// <summary>
    /// Genera un nuevo estado de aplicacion
    /// </summary>
    /// <param name="OnFuncion"></param>
    /// <param name="Tipo"></param>
    /// <param name="NR"></param>
    public StatusApp(bool OnFuncion, string Tipo, bool NR)
    {
        this.OnFuncion = OnFuncion;
        FuntionType = Tipo;
        _NeedReturns = NR;
    }



    /// <summary>
    /// Genera un nuevo estado de aplicacion
    /// </summary>
    /// <param name="OnFuncion"></param>
    /// <param name="Tipo"></param>
    public StatusApp(bool OnFuncion, string Tipo)
    {
        this.OnFuncion = OnFuncion;
        FuntionType = Tipo;
        _NeedReturns = false;
    }


    /// <summary>
    /// Obtiene o establece si la aplicacion se encuentra ejecutando una funcion
    /// </summary>
    public bool OnFuncion
    {
        get { return _OnFuncion; }
        set { _OnFuncion = value; }
    }


    /// <summary>
    /// Obtiene o establece el tipo de la funcion actual que se esta ejecutando
    /// </summary>
    public string FuntionType
    {
        get { return _FuncionTipo; }
        set { _FuncionTipo = value.ToLower().Trim(); }
    }


    /// <summary>
    /// Obtiene o establece que la aplicacion necesita de la intruccion Returns
    /// </summary>
    public bool NeedReturns
    {
        get { return _NeedReturns; }
        set { _NeedReturns = value; }
    }


}




public struct Tipo
{


    private string _Tipo;


    // Genera un tipo primitivo
    public Tipo(string tipo)
    {
        _Tipo = tipo.Trim().ToLower();
        _IsPrimitivo = true;
        _NeedGenerico = false;
    }


    // Genera un tipo con una base
    public Tipo(string tipo, bool NeedGeric)
    {
        _Tipo = tipo.Trim().ToLower();
        _IsPrimitivo = false;
        _NeedGenerico = NeedGeric;
    }


    public string tipo => _Tipo;

    public bool IsPrimitivo => _IsPrimitivo;

    public bool needGenerico => _NeedGenerico;


}


#region "Contenedores y Bloques"




public class SubBloque
{
    public List<string> Valores = new();
}

public class SubBloqueList
{
    public List<SubBloque> Bloques = new List<SubBloque>();
}





public class BloqueBase
{
    public string Valor = "";
    public string Tipo = "";

    public BloqueBase(string Valor, string Tipo)
    {
        this.Valor = Valor;
        this.Tipo = Tipo;
    }
}


public class BloqueComp
{
    public List<BloqueBase> Items = new List<BloqueBase>();
}

public class BloqueComList
{
    public List<BloqueComp> Bloques = new List<BloqueComp>();
}

public class BloqueDiccionario
{
    public string Value;

    /// <summary>
    ///             ''' Valor
    ///             ''' </summary>
    ///             ''' <param name="Value"></param>
    public BloqueDiccionario(string Value)
    {
        this.Value = Value;
    }
}


#endregion

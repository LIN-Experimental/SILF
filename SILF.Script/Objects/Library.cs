namespace SILF.Script.Objects;


public class Library
{


    public const string String = "string";
    public const string Number = "number";
    public const string LotNumber = "big";
    public const string Bool = "bool";
    public const string List = "array";
    public const string Null = "null";



    private readonly List<string> OtherTypes = [];



    /// <summary>
    /// Funciones y tipos.
    /// </summary>
    public Dictionary<Tipo, (List<IFunction>, List<IProperty>)> Objects { get; set; } = [];



    /// <summary>
    /// Cargar funciones a un tipo.
    /// </summary>
    /// <param name="type">Tipo.</param>
    /// <param name="functions">Lista de funciones.</param>
    public void Load(string type, List<IFunction> functions)
    {

        // Si ya existe.
        if (Objects.TryGetValue(new(type), out var values))
        {
            values.Item1.AddRange(functions);
            return;
        }

        // Nuevo elemento.
        Objects.Add(new(type), (functions, []));

    }



    /// <summary>
    /// Cargar funciones a un tipo.
    /// </summary>
    /// <param name="type">Tipo.</param>
    /// <param name="properties">Lista de propiedades.</param>
    public void Load(string type, List<IProperty> properties)
    {

        // Si ya existe.
        if (Objects.TryGetValue(new(type), out var values))
        {
            values.Item2.AddRange(properties);
            return;
        }

        // Nuevo elemento.
        Objects.Add(new(type), ([], properties));

    }



    /// <summary>
    /// Obtener un nuevo objeto.
    /// </summary>
    /// <param name="type">Tipo</param>
    public SILFObjectBase Get(string type = Null)
    {

        // Normalizar.
        type = type.Trim().ToLower();

        SILFObjectBase obj = new SILFNullObject();

        // Tipo string.
        if (type == String)
            obj = new SILFStringObject();

        // Tipo numérico.
        else if (type == Number)
            obj = new SILFNumberObject();

        // Tipo booleano.
        else if (type == Bool)
            obj = new SILFBoolObject();

        // Tipo numero grande.
        else if (type == LotNumber)
            obj = new SILFNumberLotObject();

        // Tipo lista.
        else if (type == List)
            obj = new SILFArrayObject();

        // Null.
        else if (type == Null)
            obj = new SILFBoolObject();

        else
        {
            // Encontrar el tipo.
            var find = OtherTypes.Where(t => t == type).FirstOrDefault();

            if (find != null)
            {
                // --
            }

        }

        // Data.
        var data = Objects.Where(t => t.Key.Description == type).Select(t => t.Value);

        // Establecer las funciones.
        obj.Functions = data.SelectMany(t => t.Item1).ToList();
        obj.Properties = data.SelectMany(t => t.Item2).ToList();

        // Null.
        return obj;

    }



    /// <summary>
    /// Obtener un nuevo objeto.
    /// </summary>
    /// <param name="type">Tipo</param>
    public Tipo? Exist(string type)
    {

        // Normalizar.
        type = type.Trim().ToLower();

        // Tipos
        string[] localTypes = ["string", "number", "array", "big", "bool"];
        string[] types = [.. localTypes, .. OtherTypes];

        // Null.
        return types.Where(t => t == type).Select(t => new Tipo(t)).FirstOrDefault();

    }



}
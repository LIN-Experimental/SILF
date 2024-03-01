namespace SILF.Script.Objects;


public class Library
{


    /// <summary>
    /// String.
    /// </summary>
    public const string String = "string";
    public const string Number = "number";
    public const string Bool = "bool";



    /// <summary>
    /// Funciones y tipos.
    /// </summary>
    public Dictionary<Tipo, List<IFunction>> Objects { get; set; } = [];



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
            values.AddRange(functions);
            return;
        }

        // Nuevo elemento.
        Objects.Add(new(type), functions);

    }



    /// <summary>
    /// Obtener un nuevo objeto.
    /// </summary>
    /// <param name="type">Tipo</param>
    public SILFObjectBase Get(string type = "null")
    {

        // Normalizar.
        type = type.Trim().ToLower();

        SILFObjectBase obj = new SILFNullObject();

        // Tipo string.
        if (type == "string")
            obj = new SILFStringObject();

        // Tipo numérico.
        else if (type == "number")
            obj = new SILFNumberObject();

        // Tipo booleano.
        else if (type == "bool")
            obj = new SILFBoolObject();

        // Null.
        else if (type == "null")
            obj = new SILFBoolObject();


        // Establecer las funciones.
        obj.Functions = Objects.Where(t => t.Key.Description == type)?.SelectMany(t => t.Value)?.ToList() ?? [];


        // Null.
        return obj;

    }



}
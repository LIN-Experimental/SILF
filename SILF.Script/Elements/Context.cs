namespace SILF.Script.Elements;


public class Context
{


    /// <summary>
    /// Contexto Base
    /// </summary>
    public Context? BaseContext { get; set; }



    /// <summary>
    /// Lista de campos
    /// </summary>
    private readonly Dictionary<string, Field> Fields = [];



    public bool IsReturn { get; set; } = false;



    /// <summary>
    /// Obtiene un campo
    /// </summary>
    /// <param name="name">Nombre</param>
    private Field? GetField(string name)
    {

        Fields.TryGetValue(name, out var field);

        // Si hay un contexto base
        field ??= BaseContext?.GetField(name);

        return field;
    }



    /// <summary>
    /// Nuevo campo
    /// </summary>
    internal bool SetField(Field field)
    {

        var exitField = GetField(field.Name);

        if (exitField == null)
        {
            Fields.Add(field.Name, field);
            return true;
        }

        return false;
    }



    internal Field? this[string name]
    {
        get => GetField(name);
    }


}
namespace SILF.Script.Objects;

public class SILFObjectBase
{

    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public string Key = Guid.NewGuid().ToString();


    /// <summary>
    /// Lista de propiedades.
    /// </summary>
    public List<IProperty> Properties { get; set; } = [];


    /// <summary>
    /// Lista de funciones.
    /// </summary>
    public List<IFunction> Functions { get; set; } = [];


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public object Value = null!;


    /// <summary>
    /// Tipo del objeto.
    /// </summary>
    public Tipo Tipo { get; set; }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public object GetValue()
    {
        return Value;
    }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public void SetValue(object? @object)
    {
        Value = @object;
    }

}
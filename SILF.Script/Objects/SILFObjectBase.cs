namespace SILF.Script.Objects;


public class SILFObjectBase
{

    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public object Value = null!;



    /// <summary>
    /// Tipo del objeto.
    /// </summary>
    public Tipo Tipo { get; set; }



    /// <summary>
    /// Métodos.
    /// </summary>
    public List<IFunction> Functions { get; set; } = [];



    /// <summary>
    /// Propiedades.
    /// </summary>
    public List<IProperty> Properties { get; set; } = [];




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
    public void SetValue(object @object)
    {
        Value = @object;
    }



}
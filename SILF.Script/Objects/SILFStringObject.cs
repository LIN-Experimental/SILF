namespace SILF.Script.Objects;


public class SILFStringObject : SILFObjectBase
{

    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFStringObject()
    {
        base.Tipo = new("string");
    }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new string GetValue()
    {
        if (Value is string value)
            return value;

        return "";
    }



    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object? value)
    {
        Value = value?.ToString() ?? "";
    }


}
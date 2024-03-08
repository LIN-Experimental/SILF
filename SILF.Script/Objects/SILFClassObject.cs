namespace SILF.Script.Objects;


public class SILFClassObject : SILFObjectBase
{



    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFClassObject(string type)
    {
        base.Tipo = new(type);
    }


    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFClassObject()
    {
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


    public static SILFClassObject Create(string type) => new(type);

}
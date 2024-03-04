namespace SILF.Script.Objects;


public class SILFNumberObject : SILFObjectBase
{



    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFNumberObject()
    {
        base.Tipo = new("number");
    }



    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new object GetValue()
    {
        if (Value is decimal value)
            return value;

        return 0;
    }


    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object value)
    {
        if (value is decimal vl)
            Value = vl;
    }


    public static SILFNumberObject Create() => new();


}
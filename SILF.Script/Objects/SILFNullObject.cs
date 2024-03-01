namespace SILF.Script.Objects;


public class SILFNullObject : SILFObjectBase
{



    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFNullObject()
    {
        base.Tipo = new("");
    }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new object? GetValue()
    {
        return null;
    }


    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object? value = null)
    {
        Value = null;
    }



    public static SILFNullObject Create()
    {
        return new();
    }

}
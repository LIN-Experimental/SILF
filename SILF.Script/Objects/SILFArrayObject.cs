using System.Collections;

namespace SILF.Script.Objects;


public class SILFArrayObject : SILFObjectBase
{


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new SILFArray Value = null!;



    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFArrayObject()
    {
        base.Tipo = new("array");
    }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new SILFArray GetValue()
    {
        if (Value is SILFArray value)
            return value;

        Value = [];
        return Value ?? [];
    }



    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object? value)
    {

        if (value is SILFArray lista)
            Value = lista;

        Value = [];

    }


    public static SILFArrayObject Create() => new();


}


public class SILFArray : List<SILFObjectBase>
{

    public override string ToString() => Library.List;


}
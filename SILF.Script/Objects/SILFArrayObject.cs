using System.Collections;

namespace SILF.Script.Objects;


public class SILFArrayObject : SILFObjectBase
{

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
    public new IEnumerable<SILFObjectBase> GetValue()
    {
        if (Value is IEnumerable<SILFObjectBase> value)
            return value;

        Value = new List<SILFObjectBase>();
        return Value as IEnumerable<SILFObjectBase> ?? [];
    }



    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object? value)
    {

        if (value is IEnumerable<SILFObjectBase> lista)
            Value = lista;

        Value = new List<SILFObjectBase>();

    }


}
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
    public new IEnumerable GetValue()
    {
        if (Value is IEnumerable value)
            return value;

        Value = new ArrayList();
        return Value as ArrayList;
    }



    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object? value)
    {

        if (value is IEnumerable lista)
            Value = lista;

        Value = new ArrayList();

    }


}
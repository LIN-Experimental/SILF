namespace SILF.Script.Objects;

public class SILFArrayObject : SILFObjectBase
{


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new SILFArray Value { get => base.Value as SILFArray ?? []; set { Value = value; } }



    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFArrayObject()
    {
        base.Tipo = new(Library.List);
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



    /// <summary>
    /// Crear nuevo objeto.
    /// </summary>
    public static SILFArrayObject Create() => new();


}


public class SILFArray : List<SILFObjectBase>
{

    public override string ToString() => Library.List;


}
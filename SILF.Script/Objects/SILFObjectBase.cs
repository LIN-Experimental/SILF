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





    public static SILFObjectBase GetByName(string type)
    {

        type = type.Trim().ToLower();

        if (type == "string")
            return new SILFStringObject();

        if (type == "number")
            return new SILFNumberObject();

        if (type == "bool")
            return new SILFBoolObject();


        return new SILFNullObject();

    }

}
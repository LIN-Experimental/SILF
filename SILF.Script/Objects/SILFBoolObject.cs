//namespace SILF.Script.Objects;


//public class SILFBoolObject : SILFObjectBase
//{

//    /// <summary>
//    /// Nuevo objeto.
//    /// </summary>
//    public SILFBoolObject()
//    {
//        base.Tipo = new(Library.Bool);
//    }


//    /// <summary>
//    /// Obtener el valor.
//    /// </summary>
//    public new bool GetValue()
//    {
//        if (Value is bool value)
//            return value;

//        return false;
//    }


//    /// <summary>
//    /// Establecer el valor.
//    /// </summary>
//    public new void SetValue(object? value)
//    {

//        if (value is bool res)
//        {
//            Value = res;
//            return;
//        }
//        Value = false;
//    }


//    public static SILFBoolObject Create() => new();


//}
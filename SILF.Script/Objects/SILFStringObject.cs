﻿//namespace SILF.Script.Objects;


//public class SILFStringObject : SILFObjectBase
//{


//    /// <summary>
//    /// Nuevo objeto.
//    /// </summary>
//    public SILFStringObject()
//    {
//        base.Tipo = new(Library.String);
//    }



//    /// <summary>
//    /// Obtener el valor.
//    /// </summary>
//    public new string GetValue()
//    {
//        if (Value is string value)
//            return value;

//        return "";
//    }



//    /// <summary>
//    /// Establecer el valor.
//    /// </summary>
//    public new void SetValue(object? value)
//    {
//        Value = value?.ToString() ?? "";
//    }


//    public static SILFStringObject Create() => new();

//}
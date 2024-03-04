﻿namespace SILF.Script.Elements.Functions;


public class FuncContext
{

    public string Name { get; set; }

    public Objects.SILFObjectBase Value { get; set; }

    public Tipo? WaitType { get; set; }

    public bool IsReturning { get; set; }

    public bool IsVoid => WaitType == null || WaitType.Value.Description.Trim() == "";



    /// <summary>
    /// Genera un nuevo contexto
    /// </summary>
    /// <param name="function">Función</param>
    public static FuncContext GenerateContext(IFunction function)
    {
        return new FuncContext()
        {
            IsReturning = false,
            WaitType = function.Type,
            Value =  Objects.SILFNullObject.Create(),
            Name = function.Name
        };
    }

}
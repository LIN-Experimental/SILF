namespace SILF.Script.Elements.Functions;


public class FuncContext
{



    public ObjectContext? ObjectContext { get; set; }



    public string Name { get; set; } = string.Empty;

    public Objects.SILFObjectBase Value { get; set; } = null!;

    public Tipo? WaitType { get; set; }

    public bool IsReturning { get; set; }

    public bool IsVoid => WaitType == null || WaitType.Value.Description?.Trim() == "";



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
            Value = Objects.SILFNullObject.Create(),
            Name = function.Name,
            ObjectContext = null
        };
    }




}
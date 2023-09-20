namespace SILF.Script.Elements.Functions;


public class FuncContext
{

    public string Name { get; set; }

    public Value Value { get; set; }

    public Tipo? WaitType { get; set; }

    public bool IsReturning { get; set; }

    public bool IsVoid => WaitType == null;



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
            Value = new(""),
            Name = function.Name
        };
    }

}
namespace SILF.Script.Elements.Functions;


internal class FuncContext
{

    public string Name { get; set; }

    public Value Value { get; set; }

    public Tipo WaitType { get; set; }

    public bool IsReturning { get; set; }

    public bool IsVoid { get; set; }



    /// <summary>
    /// Genera un nuevo contexto
    /// </summary>
    /// <param name="function">Función</param>
    public static FuncContext GenerateContext(Function function)
    {
        return new FuncContext()
        {
            IsReturning = false,
            IsVoid = (function.Type.Description == "" || function.Type.Description == null),
            WaitType = function.Type,
            Value = new(""),
            Name = function.Name
        };
    }

}
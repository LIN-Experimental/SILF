namespace SILF.Script.Elements.Functions;


internal class BridgeFunction : IFunction
{
    public Tipo? Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Parameter> Parameters { get; set; } = new();

    Context IFunction.Context { get; set; }



    Func<List<SILF.Script.Elements.ParameterValue>, FuncContext> Action;

    public BridgeFunction(Func<List<SILF.Script.Elements.ParameterValue>, FuncContext> action)
    {
        this.Action = action;
    }

    public FuncContext Run(Instance instance, List<SILF.Script.Elements.ParameterValue> values, ObjectContext @object)
    {
        return Action.Invoke(values);
    }

}

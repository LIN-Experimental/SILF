namespace SILF.Script.Elements.Functions;


internal class Function : IFunction
{

    public string Name { get; set; }

    public List<string> CodeLines { get; set; }

    public List<Parameter> Parameters { get; set; }

    public Tipo? Type { get; set; }

    public Function(string name, Tipo? tipo)
    {
        this.Name = name;
        this.Type = tipo;
        this.CodeLines = new();
        this.Parameters = new();
    }



    public FuncContext Run(Instance instance, List<ParameterValue> @params)
    {

        var context = new Context();


        foreach (var param in @params)
        {
            context.SetField(new(param.Name, new(param.Value, param.Tipo), param.Tipo, Isolation.Read) { IsAssigned = true});
        }

        var func = FuncContext.GenerateContext(this);

        // Interprete de líneas.
        foreach (var line in CodeLines)
            Runners.ScriptInterpreter.Interprete(instance, context, func, line, 0);

        return func;

    }

}

namespace SILF.Script.Elements.Functions;


internal class Function : IFunction
{

    public string Name { get; set; }

    public List<string> CodeLines { get; set; }

    public List<Parameter> Parameters { get; set; }

    public Tipo? Type { get; set; }


    public Context Context { get; set; }


    public Function(string name, Tipo? tipo, Tipo? parentType)
    {
        this.Name = name;
        this.Type = tipo;
        this.CodeLines = new();
        this.Parameters = new();

    }



    public FuncContext Run(Instance instance, List<ParameterValue> @params, ObjectContext @object)
    {

        var context = new Context();


        foreach (var param in @params)
        {

            var value = instance.Library.Get(param.Objeto.Tipo.Description);

            value.SetValue(param.Objeto.Value);

            var field = new Field()
            {
                Instance = instance,
                IsAssigned = true,
                Isolation = Isolation.Read,
                Name = param.Name,
                Tipo = param.Objeto.Tipo,
                Value = value
            };

            context.SetField(field);
        }


        var func = FuncContext.GenerateContext(this);

        // Interprete de líneas.
        foreach (var line in CodeLines)
            Runners.ScriptInterpreter.Interprete(instance, context, @object, func, line, 0);

        return func;

    }


}

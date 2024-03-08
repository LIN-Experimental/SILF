namespace SILF.Script.Elements.Functions;


internal class BridgeProperty : IProperty
{

    public string Name { get; set; }

    public Tipo? Tipo { get; set; }

    public SILFObjectBase Value { get; set; } = null!;

    public SILFObjectBase Parent { get; set; } = null!;

    public IFunction Get { get; set; }
    public IFunction Set { get; set; }
    public BridgeProperty(string name, Tipo? tipo)
    {
        this.Name = name;
        this.Tipo = tipo;

        Get = new BridgeFunction((e) =>
        {
            return new FuncContext()
            {
                IsReturning = true,
                Value = Value,
                WaitType = Tipo,
            };
        })
        {
            Name = "get",
            Parameters =
            [
               new Parameter("value", Tipo.Value)
            ]
        };


        Set = new BridgeFunction((e) =>
        {

            var value = e.LastOrDefault(t => t.Name == "value");


            if (value.Objeto is SILFObjectBase obj)
                Value = obj;

            return new FuncContext()
            {
                IsReturning = true,
                WaitType = new(),
            };
        })
        {
            Name = "set",
            Parameters =
            [
               new Parameter("value", Tipo.Value)
            ]
        };







    }





    public SILFObjectBase GetValue(Instance instance)
    {


        var result = Get.Run(instance,
            [
            new ParameterValue("", Value),
            new ParameterValue("",  Parent)
            ], ObjectContext.GenerateContext(Parent));


        return result.Value;
    }


    public void SetValue(Instance instance, SILFObjectBase @base)
    {
        Set.Run(instance, [
            new ParameterValue("", @base),
            new ParameterValue("", Parent)
            ], ObjectContext.GenerateContext(Parent));
    }

    public IProperty Clone()
    {
        return new BridgeProperty(Name, Tipo);
    }

    public void Establish(SILFObjectBase obj)
    {
       // throw new NotImplementedException();
    }
}

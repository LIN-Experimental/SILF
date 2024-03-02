using SILF.Script.Objects;

namespace SILF.Script.Elements.Functions;


internal class Property : IProperty
{

    public string Name { get; set; }

    public Tipo? Type { get; set; }

    public SILFObjectBase Value { get; set; }

    public IFunction Get { get; set; }
    public IFunction Set { get; set; }
    public SILFObjectBase Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Property(string name, Tipo? tipo)
    {
        this.Name = name;
        this.Type = tipo;

        Get = new BridgeFunction((e) =>
        {
            return new FuncContext()
            {
                IsReturning = true,
                Value = Value,
                WaitType = Type,
            };
        })
        {
            Name = "get",
            Parameters =
            [
               new Parameter("value", Type.Value)
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
               new Parameter("value", Type.Value)
            ]
        };







    }



    public SILFObjectBase GetValue(Instance instance)
    {
        var result = Get.Run(instance, [new ParameterValue("", Value)]);
        return result.Value;
    }

    public void SetValue(Instance instance, SILFObjectBase @base)
    {
        Set.Run(instance, [new ParameterValue("", @base)]);
    }
}

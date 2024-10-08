﻿namespace SILF.Script.Elements.Functions;


internal class Property : IProperty
{

    public string Name { get; set; }

    public Tipo? Tipo { get; set; }

    public SILFObjectBase Value { get; set; }

    public IFunction Get { get; set; }
    public IFunction Set { get; set; }
    public SILFObjectBase Parent { get; set; }


    public Instance Instance { get; set; }


    public Property(string name, Tipo? tipo)
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

            var value = e.FirstOrDefault();


            if (value?.Objeto is SILFObjectBase obj)
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
        var result = Get.Run(instance, [new ParameterValue("", Value)], ObjectContext.GenerateContext(Parent));
        return result.Value;
    }

    public void SetValue(Instance instance, SILFObjectBase @base)
    {
        Set.Run(instance, [new ParameterValue("", @base)], ObjectContext.GenerateContext(Parent));
    }


    public IProperty Clone()
    {
        return new Property(Name, Tipo)
        {

        };
    }

    public void Establish(SILFObjectBase objectBase)
    {
        SetValue(Instance, objectBase);
    }
}

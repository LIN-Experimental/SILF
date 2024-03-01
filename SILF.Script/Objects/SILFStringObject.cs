namespace SILF.Script.Objects;


public class SILFStringObject : SILFObjectBase
{

    /// <summary>
    /// Nuevo objeto.
    /// </summary>
    public SILFStringObject()
    {


        base.Tipo = new("string");


        BridgeFunction trim = new((values) =>
        {
            var cadena = values.LastOrDefault(t => t.Name == "value")!.Value ?? "";
            cadena = cadena?.ToString()?.Trim() ?? "";
            return new()
            {
                IsReturning = true,
                Value = new SILFStringObject()
                {
                    Tipo = new Tipo("string"),
                    Value = cadena,
                }
            };
        })
        {
            Name = "trim",
            Type = new("string"),
            Parameters =
            [
                new("value", new("string"))
            ]
        };


        BridgeFunction upper = new((values) =>
        {
            var cadena = values.LastOrDefault(t => t.Name == "value")!.Value ?? "";
            cadena = cadena?.ToString()?.ToUpper() ?? "";
            return new()
            {
                IsReturning = true,
                Value = new SILFStringObject()
                {
                    Tipo = new Tipo("string"),
                    Value = cadena,
                }
            };
        })
        {
            Name = "upper",
            Type = new("string"),
            Parameters =
           [
               new("value", new("string"))
           ]
        };

        BridgeFunction lower = new((values) =>
        {
            var cadena = values.LastOrDefault(t => t.Name == "value")!.Value ?? "";
            cadena = cadena?.ToString()?.ToLower() ?? "";
            return new()
            {
                IsReturning = true,
                Value = new SILFStringObject()
                {
                    Tipo = new Tipo("string"),
                    Value = cadena,
                }
            };
        })
        {
            Name = "lower",
            Type = new("string"),
            Parameters =
           [
               new("value", new("string"))
           ]
        };


        BridgeFunction toNumber = new((values) =>
        {
            var cadena = values.LastOrDefault(t => t.Name == "value")!.Value ?? "";

            decimal.TryParse(cadena.ToString(), out decimal result);

            return new()
            {
                IsReturning = true,
                Value = new SILFNumberObject()
                {
                    Tipo = new Tipo("number"),
                    Value = result,
                }
            };
        })
        {
            Name = "toNumber",
            Type = new("number"),
            Parameters =
           [
               new("value", new("string"))
           ]
        };

        base.Functions = [.. Functions, trim, lower, upper, toNumber];



    }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    public new string GetValue()
    {
        if (Value is string value)
            return value;

        return "";
    }


    /// <summary>
    /// Establecer el valor.
    /// </summary>
    public new void SetValue(object? value)
    {
        Value = value?.ToString() ?? "";
    }


}
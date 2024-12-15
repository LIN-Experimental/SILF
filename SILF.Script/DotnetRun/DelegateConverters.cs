using SILF.Script.DotnetRun.Interop;
using System.Reflection;

namespace SILF.Script.DotnetRun;

internal class DelegateConverters
{

    /// <summary>
    /// Obtener la información de un método de C# a SILF.
    /// </summary>
    /// <param name="method">Método.</param>
    public static (IFunction? element, string type) GetInformation(Delegate method)
    {

        // Validar información.
        if (method is null)
            return (null, "");

        // Información
        MethodInfo information = method.Method;

        // Información desde atributo.
        var atributo = information.GetCustomAttribute<SILFFunctionNameAttribute>();
        string methodName = atributo != null ? atributo.Name : information.Name;
        string? collectionName = atributo != null ? atributo.Type : "";

        // Obtener el tipo de retorno del método
        Type tipoRetorno = information.ReturnType;

        string tipo = GetSilfType(tipoRetorno.FullName);

        var function = new DotnetBridgeFunction
        {
            Name = methodName,
            Type = new(tipo),
            Action = method,
        };

        // Obtener los parámetros del método
        List<ParameterInfo> parametros = information.GetParameters().ToList();

        ParameterInfo DI = parametros.FirstOrDefault();

        if (DI != null && DI.ParameterType.FullName.ToLower() == "system.iserviceprovider")
        {
            function.IsDependency = true;
            parametros.Remove(DI);
        }

        foreach (var parametro in parametros)
        {
            function.Parameters.Add(new(parametro.Name, new(GetSilfType(parametro.ParameterType.FullName))));
        }

        return (function, collectionName ?? "");

    }


    /// <summary>
    /// Obtener el tipo SILF a partir del tipo de C#.
    /// </summary>
    public static string GetSilfType(string type)
    {
        switch (type.ToLower())
        {
            case "system.string":
                return "string";
            case "system.int16" or "system.int32" or "system.int64" or "system.decimal":
                return "number";
            case "system.boolean":
                return "bool";
            case "system.object":
                return "mutable";
        }
        return "mutable";
    }

}
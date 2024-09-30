namespace SILF.Script.Builders;

internal class MethodBuilder
{


    /// <summary>
    /// Obtiene la lista de funciones
    /// </summary>
    /// <param name="instance">Instancia de la app.</param>
    /// <param name="code">Líneas de código.</param>
    public static List<Function> Build(SILFClass @class, Instance instance, IEnumerable<string> code)
    {

        // Funciones obtenidas.
        List<Function> functions = [];

        // Función actual.
        Function? function = null;

        // Recorrer el código
        foreach (var line in code)
        {

            // Obtiene el resultado
            var isMatch = Actions.Functions.Match(line, out string tipo, out string name, out var parameters);

            // Si no es una función
            if (!isMatch)
            {
                // Agrega la línea a la función de contexto
                function?.CodeLines.Add(line);
                continue;
            }


            var normalType = instance.Library.Exist(tipo);

            if (normalType == null && tipo != "void")
            {
                instance.WriteError("SC012", $"el tipo '{tipo}' de la función '{name}' no existe.");
                continue;
            }


            function = new(name, normalType, new(@class.Name))
            {
                Parameters = [new Parameter("value", new(@class.Name), true)]
            };

            functions.Add(function);

            foreach (var param in parameters)
            {

                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var paramType = instance.Library.Exist(param.Trim().Split(" ")[0]);
                var paramName = param.Trim().Split(" ").ElementAtOrDefault(1);

                if (paramName == null)
                {
                    instance.WriteError("SC015", $"Parámetro sin nombre en la función '{name}'.");
                    continue;
                }

                if (paramType == null)
                {
                    instance.WriteError("SC012", $"El tipo '{paramType}' del parámetro '{paramName}' de la función '{name}' no existe.");
                    continue;
                }

                Parameter parameter = new(paramName, paramType.Value);
                function.Parameters.Add(parameter);
            }



        }

        return functions;

    }


}
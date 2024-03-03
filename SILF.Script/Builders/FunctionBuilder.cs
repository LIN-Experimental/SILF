namespace SILF.Script.Builders;


internal class FunctionBuilder
{


    /// <summary>
    /// Obtiene la lista de funciones
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="code">Líneas de código</param>
    public static List<Function> GetFunctions(Instance instance, IEnumerable<string> code)
    {

        // Lista de instrucciones de nivel superior (Main)
        List<string> superior = new();

        // Funciones obtenidas
        List<Function> functions = new();

        // Función actual
        Function? function = null;

        // Si se esta recogiendo en el main superior
        bool isSuperior = true;

        // Recorrer el código
        foreach (var line in code)
        {

            // Obtiene el resultado
            var isMatch = Actions.Functions.Match(line, out string tipo, out string name, out var parameters);

            // Si no es una función
            if (!isMatch)
            {
                // Si la instrucción es de nivel superior
                if (isSuperior)
                {
                    superior.Add(line);
                    continue;
                }

                // Agrega la línea a la función de contexto
                function?.CodeLines.Add(line);
                continue;
            }

            // Cambia el estado de recolección superior
            isSuperior = false;


           

            var normalType =  instance.Library.Exist(tipo);

            if (normalType == null && tipo != "void")
            {
                instance.WriteError("SC012", $"el tipo '{tipo}' de la función '{name}' no existe.");
                continue;
            }


            function = new(name, normalType);

            functions.Add(function);

            foreach (var param in parameters)
            {

                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var paramType = instance.Library.Exist(param.Split(" ")[0]);
                var paramName = param.Split(" ").ElementAtOrDefault(1);

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



        var main = functions.Where(T => T.Name == "main").Any();

        if (!main && superior.Count > 0)
        {
            var mainFunc = new Function("main", new())
            {
                CodeLines = superior
            };
            functions.Add(mainFunc);
        }
        else if (!main)
        {
            instance.WriteError("SC016", "No hay función main al compilar");
        }

        return functions;



    }


}
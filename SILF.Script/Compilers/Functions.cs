namespace SILF.Script.Compilers;


internal class Functions
{



    private static bool Match(string input, out string tipo, out string nombre, out List<string> parameters)
    {

        tipo = "";
        nombre = "";
        parameters = new();
        string pattern = @"function\s+(?<tipoRetorno>\w+)\s+(?<nombre>\w+)\s*\((?<parametros>.+?)?\)";

        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            tipo = match.Groups["tipoRetorno"].Value;
            nombre = match.Groups["nombre"].Value;
            string parametersCrude = match.Groups["parametros"].Value;

            // Dividir los parámetros en una lista si es necesario
            parameters = parametersCrude.Split(new char[] { ',' }, StringSplitOptions.TrimEntries).ToList();
            return true;
        }
        return false;

    }


    public static List<Function> GetFunctions(Instance instance, IEnumerable<string> code)
    {

        List<string> superior = new();
        List<Function> functions = new();
        Function? function = null;
        bool isSuperior = true;


        foreach (var line in code)
        {

            var isMatch = Match(line, out string tipo, out string name, out var parameters);

            if (!isMatch)
            {

                if (isSuperior)
                {
                    superior.Add(line);
                    continue;
                }

                function?.CodeLines.Add(line);
                continue;
            }

            isSuperior = false;
            var normalType = instance.Tipos.Where(T => T.Description == tipo).FirstOrDefault();
            function = new(name, normalType);
            functions.Add(function);

            foreach(var param in parameters)
            {

                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var paramType = instance.Tipos.Where(T => T.Description == param.Split(" ")[0]).FirstOrDefault();
                var paramName = param.Split(" ").ElementAtOrDefault(1);

                if (paramName == null)
                {
                    instance.WriteError($"Parámetro sin nombre en la función '{name}'.");
                    continue;
                }

                Parameter parameter = new(paramName, paramType);
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
            instance.WriteError("No hay función main al compilar");
        }

        return functions;



    }


}

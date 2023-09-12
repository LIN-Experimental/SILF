namespace SILF.Script.Compilers;


internal class Functions
{



    private static bool Match(string input, out string tipo, out string nombre, out List<string> parameters)
    {

        tipo = "";
        nombre = "";
        parameters = new();
        string pattern = @"function\s+(?<tipoRetorno>\w+)\s+(?<nombre>\w+)\((?<parametros>.+?)\)";

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

        List<Function> functions = new();
        Function? function = null;

        foreach (var line in code)
        {

            var isMatch = Match(line, out string tipo, out string name, out var parameters);

            if (!isMatch)
            {
                function?.CodeLines.Add(line);
                continue;
            }

            var normalType = instance.Tipos.Where(T => T.Description == tipo).FirstOrDefault();
            function = new(name, normalType);
            functions.Add(function);

            foreach(var param in parameters)
            {
                var paramType = instance.Tipos.Where(T => T.Description == param.Split(" ")[0]).FirstOrDefault();
                var paramName = param.Split(" ")[1];

                Parameter parameter = new(paramName, paramType);
                function.Parameters.Add(parameter);
            }
           


        }


        return functions;



    }


}

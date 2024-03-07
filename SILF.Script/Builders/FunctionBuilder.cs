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




            var normalType = instance.Library.Exist(tipo);

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



    public static ControlStructure ParseCode(IEnumerable<string> code, Instance instance)
    {
        ControlStructure root = new ControlStructure("Root");

        Stack<ControlStructure> stack = new Stack<ControlStructure>();
        stack.Push(root);

        int id = 0;

        foreach (string line in code)
        {

            // IF
            if (Regex.IsMatch(line.Trim(), @"^if\s*\(.+\)\s*$"))
            {
                id++;
                ControlStructure newStructure = new IfStructure()
                {
                    Id = id,
                    Expression = line.Trim().Remove(0, 2)
                };

                var peek = stack.Peek();

                peek.InnerStructures.Add(newStructure);
                peek.Lines.Add($"?i{id}");

                instance.Structures.Add(newStructure);

                stack.Push(newStructure);
                continue;
            }


       


            // Foreach
            var z = Regex.Match(line.Trim(), @"^for\s*\(\s*(?<varName>\w+)\s+in\s+(?<collection>.+?)\s*\)\s*$");
            if (z.Success)
            {
                id++;
                ControlStructure newStructure = new ForStructure()
                {
                    Id = id,
                    Name = z.Groups["varName"].Value,
                    Expression = z.Groups["collection"].Value,
                };

                var peek = stack.Peek();

                peek.InnerStructures.Add(newStructure);

                peek.Lines.Add($"?f{id}");

                instance.Structures.Add(newStructure);

                stack.Push(newStructure);
            }
            else if (Regex.IsMatch(line.Trim(), @"^}$"))
            {
                stack.Pop();
            }
            else
            {
                stack.Peek().Lines.Add(line.Trim());
            }
        }

        return root;
    }

  
   




    public class ControlStructure
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<string> Lines { get; set; }
        public List<ControlStructure> InnerStructures { get; set; }

        public ControlStructure(string type)
        {
            Type = type;
            Lines = new List<string>();
            InnerStructures = new List<ControlStructure>();
        }
    }


    public class ForStructure : ControlStructure
    {

        public string Name {  get; set; }

        public string Expression { get; set; }

        public ForStructure() : base("for")
        {
        }

    }


    public class IfStructure : ControlStructure
    {

        public string Expression { get; set; }

        public IfStructure() : base("if")
        {
        }

    }


}
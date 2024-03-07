namespace SILF.Script.Builders;

internal class ClassBuilder
{

    private const string Lines = """
        function void main()
        """;


    /// <summary>
    /// Construir las clases.
    /// </summary>
    /// <param name="codeLines">Líneas de código.</param>
    /// <param name="instance">Instancia.</param>
    public static IEnumerable<SILFClass> Build(IEnumerable<string> codeLines, Instance instance)
    {

        // Patron.
        const string pattern = @"\bclass\s+(\w+)";

        // Objeto de clase.
        SILFClass @class = new()
        {
            Name = "Startup",
            Lineas = [.. Lines.Split('\n')]
        };

        // Lista de clases.
        List<SILFClass> classes = [@class];

        // Recorrer las líneas.
        foreach (string line in codeLines)
        {

            // Patron.
            Match match = Regex.Match(line, pattern);

            // No hubo match.
            if (!match.Success)
            {
                @class.Lineas.Add(line);
                continue;
            }

            // Nueva clase.
            @class = new()
            {
                Name = match.Groups[1].Value,
            };
            classes.Add(@class);

        }

        // Retornar.
        return classes;

    }



}
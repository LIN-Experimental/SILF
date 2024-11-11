using SILF.Script.Builders;

namespace SILF.Script;

/// <summary>
/// Nueva app SILF
/// </summary>
/// <param name="code">Código a ejecutar.</param>
public class App(string code, IConsole? console = null, Environments environment = Environments.Release)
{

    /// <summary>
    /// Instancia de SILF
    /// </summary>
    private Instance? Instance { get; set; }


    /// <summary>
    /// Librería externa.
    /// </summary>
    public Library Library { get; set; } = new();


    /// <summary>
    /// Consola.
    /// </summary>
    private IConsole? Console { get; set; } = console;


    /// <summary>
    /// Ambiente de la app
    /// </summary>
    public Environments Environment { get; private set; } = environment;



    /// <summary>
    /// Funciones default de C#
    /// </summary>
    public List<IFunction> Functions { get; set; } = [];



    /// <summary>
    /// Código a ejecutar
    /// </summary>
    private readonly string Code = code ?? "";



    /// <summary>
    /// Agrega funciones default de C# a SILF.Core
    /// </summary>
    /// <param name="functions">Funciones base</param>
    public void AddDefaultFunctions(IEnumerable<IFunction> functions)
    {
        Functions.AddRange(functions);
    }


    /// <summary>
    /// Ejecuta la aplicación
    /// </summary>
    public void Run()
    {

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Comprobar el entorno.
        if (Environment == Environments.PreRun)
        {
            RunTest();
            return;
        }

        // Generar la estancia.
        Instance = new(Console, Environment);

        // Compilar clases.
        var classes = Builders.ClassBuilder.Build(Code.Split('\n'), Instance);

        // Compilar métodos.
        foreach (var @class in classes)
        {

            // Construir los métodos.
            var methods = Builders.MethodBuilder.Build(@class, Instance, @class.Lineas);

            // Construir las propiedades.
            var properties = Builders.PropertyBuilder.Build(Instance, @class.Lineas);

            foreach (var method in methods)
                method.CodeLines = FunctionBuilder.ParseCode(method.CodeLines, Instance).Lines;

            Instance.Library.Load(@class.Name, [.. methods]);
            Instance.Library.Load(@class.Name, [.. properties]);
            @class.Functions.AddRange(methods);
        }

        // Cargar objetos de los frameworks.
        LoadObjects();

        // Obtener el método main
        IFunction? main = classes.Where(t => t.Name == "Startup").FirstOrDefault().Functions.Where(t => t.Name == "main").FirstOrDefault();

        // Validar el método main.
        if (main == null)
        {
            Console?.InsertLine("SC016", "No se encontró la función 'main'", LogLevel.Error);
            return;
        }

        // Cargar funciones.
        Instance.Functions =
        [
            .. Functions,
        ];

        var @object = Instance.Library.Get("Startup");

        // Ejecutar.
        main.Run(Instance, [], ObjectContext.GenerateContext(@object));

        Instance.Write($"Execution time {stopwatch.ElapsedMilliseconds}ms");

    }



    /// <summary>
    /// Ejecuta la aplicación
    /// </summary>
    public object? RunProfit()
    {

        // Generar la estancia.
        Instance = new(Console, Environment);


        // Cargar objetos de los frameworks.
        LoadObjects();


        // Cargar funciones.
        Instance.Functions =
        [
            .. Functions,
        ];

        var result = Runners.ScriptInterpreter.Interprete(Instance, new(), new(), new(), Code, 1);

        return result.ElementAtOrDefault(0)?.Object?.GetValue() ?? "";
    }



    /// <summary>
    /// Ejecutar en modo Test.
    /// </summary>
    private void RunTest()
    {
        // El modo de TESTS en SILF Script 2024.3 esta deshabilitado.
    }



    /// <summary>
    /// Cargar los objetos.
    /// </summary>
    private void LoadObjects()
    {

        // Validar.
        if (Instance == null)
            return;

        // Cargar objetos de los frameworks.
        foreach (var objects in Library.Objects)
        {
            // Funciones.
            Instance.Library.Load(objects.Key.Description, objects.Value.Item1);

            // Propiedades.
            Instance.Library.Load(objects.Key.Description, objects.Value.Item2);
        }

    }


}
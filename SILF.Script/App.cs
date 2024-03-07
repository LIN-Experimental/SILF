
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
            var methods = Builders.MethodBuilder.Build(@class, Instance, @class.Lineas);
            Instance.Library.Load(@class.Name, [.. methods]);
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
            // Funciones del compilador
          //  .. build.Functions,
            // Funciones externas.
            .. Functions,
        ];

        // Contexto.
        Context context = new();

        // Contexto de la función.
        FuncContext funContext = FuncContext.GenerateContext(main);

        // Ejecutar.
        main.Run(Instance, []);
    }



    /// <summary>
    /// Ejecutar en modo Test.
    /// </summary>
    private void RunTest()
    {

        // Nueva estancia
        Instance = new(Console, Environment);

        // Cargar objetos.
        LoadObjects();


        // Compilar el código.
        CompileResult build = new Compilers.ScriptCompiler(Code).Compile(Instance);

        // Obtener el método main
        Function? main = build.GetMain();

        // Validar el método main.
        if (main == null)
        {
            Console?.InsertLine("SC016", "No se encontró la función 'main'", LogLevel.Error);
            return;
        }

        // Funciones generales.
        Instance.Functions =
        [
            // Funciones del compilador
            .. build.Functions,

            // Funciones externas
            .. Functions,
        ];


        // Funciones.
        foreach (Function function in Instance.Functions.Where(t => t is Function))
        {

            // Generar el contexto.
            Context context = new();

            // Contexto de la función.
            FuncContext funcContext = FuncContext.GenerateContext(function);

            // Parámetros.
            foreach (var parameter in function.Parameters)
            {

                // Campo.
                Field field = new()
                {
                    Name = parameter.Name,
                    IsAssigned = true,
                    Instance = Instance,
                    Isolation = Isolation.Read,
                    Tipo = parameter.Tipo
                };

                // Establecer el parámetro.
                context.SetField(field);

            }


            // Ejecutar la función.
            foreach (var line in function.CodeLines)
                Runners.ScriptInterpreter.Interprete(Instance, context, funcContext, line, 0);

        }

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
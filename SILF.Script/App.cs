using SILF.Script.Objects;

namespace SILF.Script;


public class App
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
    private IConsole? Console { get; set; }



    /// <summary>
    /// Ambiente de la app
    /// </summary>
    public Environments Environment { get; private set; }



    /// <summary>
    /// Funciones default de C#
    /// </summary>
    public List<IFunction> Functions { get; set; } = [];



    /// <summary>
    /// Código a ejecutar
    /// </summary>
    private readonly string Code;


    private bool UseCache { get; set; } = false;



    /// <summary>
    /// Nueva app SILF
    /// </summary>
    /// <param name="code">Código a ejecutar.</param>
    public App(string code, IConsole? console = null, Environments environment = Environments.Release, bool useCache = false)
    {
        this.Code = code ?? "";
        this.Console = console;
        this.Environment = environment;
        this.UseCache = useCache;
    }



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

        if (Environment == Environments.PreRun)
        {
            RunTest();
            return;
        }

        // Nueva estancia
        Instance = new(Console, Environment, UseCache);

        foreach (var x in Library.Objects)
        {
            Instance.Library.Load(x.Key.Description, x.Value);
        }

        var build = new Compilers.ScriptCompiler(this.Code).Compile(Instance);

        var main = build.GetMain();

        if (main == null)
        {
            Console?.InsertLine("No se encontró la función 'main'", LogLevel.Error);
            return;
        }

        Instance.Functions =
        [
            // Funciones del compilador
            .. build.Functions,
            // Funciones externas
            .. Functions,
        ];

        Context context = new();
        FuncContext funContext = FuncContext.GenerateContext(main);

        

        foreach (var line in main.CodeLines)
            ScriptInterpreter.Interprete(Instance, context, funContext, line, 0);

    }


    private void RunTest()
    {
        // Nueva estancia
        Instance = new(Console, Environment, UseCache);


        foreach(var x in Library.Objects)
        {
            Instance.Library.Load(x.Key.Description, x.Value);
        }


        var build = new Compilers.ScriptCompiler(this.Code).Compile(Instance);

        var main = build.GetMain();

        if (main == null)
        {
            Console?.InsertLine("No se encontró la función 'main'", LogLevel.Error);
            return;
        }

        Instance.Functions =
        [
            // Funciones del compilador
            .. build.Functions,

            // Funciones externas
            .. Functions,
        ];


        foreach (var function in Instance.Functions)
        {

            if (function is Function baseFunction)
            {
                Context context = new();
                FuncContext funContext = FuncContext.GenerateContext(baseFunction);

                foreach (var var in function.Parameters)
                {
                    context.SetField(new(var.Name, null, var.Tipo, Instance, Isolation.ReadAndWrite)
                    {
                        IsAssigned = true
                    });
                }


                foreach (var line in baseFunction.CodeLines)
                    Runners.ScriptInterpreter.Interprete(Instance, context, funContext, line, 0);
            }


        }


    }


}
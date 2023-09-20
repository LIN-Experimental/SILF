namespace SILF.Script;


public class App
{

    /// <summary>
    /// Instancia de SILF
    /// </summary>
    private Instance? Instance { get; set; }



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
    public List<IFunction> Functions { get; set; } = new();



    /// <summary>
    /// Código a ejecutar
    /// </summary>
    private readonly string Code;



    /// <summary>
    /// Nueva app SILF
    /// </summary>
    /// <param name="code">Código a ejecutar.</param>
    public App(string code, IConsole? console = null, Environments environment = Environments.Release)
    {
        this.Code = code ?? "";
        this.Console = console;
        this.Environment = environment;
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
        Instance = new(Console, Environment);

        var build = new Compilers.ScriptCompiler(this.Code).Compile(Instance);

        var main = build.GetMain();

        if (main == null)
        {
            Console?.InsertLine("No se encontró la función 'main'", LogLevel.Error);
            return;
        }
        Instance.Functions = new();

        // Funciones del compilador
        Instance.Functions.AddRange(build.Functions);

        // Funciones externas
        Instance.Functions.AddRange(Functions);

        Context context = new();
        FuncContext funContext = FuncContext.GenerateContext(main);

        foreach (var line in main.CodeLines)
            ScriptInterpreter.Interprete(Instance, context, funContext, line, 0);

    }


    private void RunTest()
    {
        // Nueva estancia
        Instance = new(Console, Environment);

        var build = new Compilers.ScriptCompiler(this.Code).Compile(Instance);

        var main = build.GetMain();

        if (main == null)
        {
            Console?.InsertLine("No se encontró la función 'main'", LogLevel.Error);
            return;
        }

        Instance.Functions = new();
        Instance.Functions.AddRange(build.Functions);

        foreach (var function in Instance.Functions)
        {

            if (function is Function baseFunction)
            {
                Context context = new();
                FuncContext funContext = FuncContext.GenerateContext(baseFunction);

                foreach (var line in baseFunction.CodeLines)
                    Runners.ScriptInterpreter.Interprete(Instance, context, funContext, line, 0);
            }


        }


    }


}
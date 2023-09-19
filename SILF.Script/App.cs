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
        Instance.Functions = build.Functions;

        Context context = new();
        FuncContext funContext = FuncContext.GenerateContext(main);

        foreach (var line in main.CodeLines)
            Runners.ScriptInterpreter.Interprete(Instance, context, funContext, line, 0);

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

        Instance.Functions = build.Functions;

        foreach (var function in Instance.Functions)
        {
            Context context = new();
            FuncContext funContext = FuncContext.GenerateContext(function);

            foreach (var line in function.CodeLines)
                Runners.ScriptInterpreter.Interprete(Instance, context, funContext, line, 0);

        }


    }


}
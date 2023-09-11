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


    public void Run()
    {
        Instance = new(Console, Environment);
        var x = Code.Split('\n');

        Context context = new();
        foreach (var code in x)
        {
            Runners.ScriptInterpreter.Interprete(Instance, context, code, 0);
        }
    }

}
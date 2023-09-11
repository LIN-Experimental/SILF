namespace SILF.Script;


public class App
{

    /// <summary>
    /// Instancia de SILF
    /// </summary>
    private Instance Instance { get; set; }



    /// <summary>
    /// Consola.
    /// </summary>
    private IConsole? Console { get; set; }



    /// <summary>
    /// Código a ejecutar
    /// </summary>
    private readonly string Code;



    /// <summary>
    /// Nueva app SILF
    /// </summary>
    /// <param name="code">Código a ejecutar.</param>
    public App(string code, IConsole? console = null)
    {
        this.Code = code ?? "";
        this.Console = console;
    }


    public void Run()
    {
        Instance = new(Console);
        var x = Code.Split('\n');

        Context context = new();
        foreach(var code in x)
        {
            Runners.ScriptInterpreter.Interprete(Instance, context, code, 0);
        }
    }

}
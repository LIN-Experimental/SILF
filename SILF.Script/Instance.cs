using SILF.Script.Objects;

namespace SILF.Script;


public class Instance
{

    /// <summary>
    /// Consola
    /// </summary>
    private IConsole? Console { get; set; }



    /// <summary>
    /// Lista de funciones
    /// </summary>
    public List<IFunction> Functions { get; set; }



    /// <summary>
    /// Lista de funciones
    /// </summary>
    internal Library Library { get; set; } = new();



    /// <summary>
    /// Si la app esta ejecutando
    /// </summary>
    public bool IsRunning { get; set; } = true;



    /// <summary>
    /// Ambiente de la app
    /// </summary>
    public Environments Environment { get; private set; }



    /// <summary>
    /// Nueva estancia de la app.
    /// </summary>
    /// <param name="console">IConsole</param>
    public Instance(IConsole? console, Environments environment)
    {
        this.Console = console;
        this.Environment = environment;
        this.Functions = new();
    }




    /// <summary>
    /// Escribe sobre la consola.
    /// </summary>
    /// <param name="result">Resultado.</param>
    public void Write(string result)
    {
        if (Environment != Environments.PreRun)
            Console?.InsertLine(result, LogLevel.None);
    }



    /// <summary>
    /// Escribe sobre la consola.
    /// </summary>
    /// <param name="result">Resultado.</param>
    public void WriteError(string result)
    {
        IsRunning = false;
        Console?.InsertLine(result, LogLevel.Error);
    }




    /// <summary>
    /// Escribe sobre la consola.
    /// </summary>
    /// <param name="result">Resultado.</param>
    public void WriteWarning(string result)
    {
        if (Environment != Environments.PreRun)
            Console?.InsertLine(result, LogLevel.Warning);
    }


}
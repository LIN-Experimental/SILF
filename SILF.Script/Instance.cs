namespace SILF.Script;


internal class Instance
{

    /// <summary>
    /// Consola
    /// </summary>
    private IConsole? Console { get; set; }


    public List<Function> Functions { get; set; }



    public bool IsRunning { get; set; } = true;



    /// <summary>
    /// Ambiente de la app
    /// </summary>
    public Environments Environment { get; private set; }



    /// <summary>
    /// Lista de tipos
    /// </summary>
    public List<Tipo> Tipos = new()
    {
        new("string"),
        new("number"),
        new("bool"),
        new("char"),
        new("operator"),
        new("mutable")
    };



    /// <summary>
    /// Nueva estancia de la app.
    /// </summary>
    /// <param name="console">IConsole</param>
    public Instance(IConsole? console, Environments environment)
    {
        this.Console = console;
        this.Environment = environment;
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
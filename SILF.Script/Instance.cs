using static SILF.Script.Builders.FunctionBuilder;

namespace SILF.Script;


/// <summary>
/// Nueva estancia de la app.
/// </summary>
/// <param name="console">IConsole</param>
/// <param name="environment">Entorno</param>
public class Instance(IConsole? console, Environments environment)
{


    internal List<ControlStructure> Structures = [];



    /// <summary>
    /// Consola
    /// </summary>
    private IConsole? Console { get; set; } = console;



    /// <summary>
    /// Lista de funciones
    /// </summary>
    public List<IFunction> Functions { get; set; } = [];



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
    public Environments Environment { get; private set; } = environment;



    /// <summary>
    /// Escribe sobre la consola.
    /// </summary>
    /// <param name="result">Resultado.</param>
    public void Write(string result)
    {
        if (Environment != Environments.PreRun)
            Console?.InsertLine(result, "", LogLevel.None);
    }



    /// <summary>
    /// Escribe sobre la consola.
    /// </summary>
    /// <param name="result">Resultado.</param>
    public void WriteError(string errorCode, string result)
    {
        IsRunning = false;
        Console?.InsertLine(result, errorCode, LogLevel.Error);
    }



    /// <summary>
    /// Escribe sobre la consola.
    /// </summary>
    /// <param name="result">Resultado.</param>
    public void WriteWarning(string result)
    {
        if (Environment != Environments.PreRun)
            Console?.InsertLine(result, "", LogLevel.Warning);
    }





}
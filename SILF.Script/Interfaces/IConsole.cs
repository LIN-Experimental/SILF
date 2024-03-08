namespace SILF.Script.Interfaces;


public interface IConsole
{


    /// <summary>
    /// Método que se ejecuta al recibir un resultado.
    /// </summary>
    public void InsertLine(string result, string code, LogLevel logLevel);


}
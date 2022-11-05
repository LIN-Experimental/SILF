namespace SILF.Script.Interfaces;


public interface IFunction
{

    /// <summary>
    /// Método que se ejecuta al recibir un resultado.
    /// </summary>
    public object InsertLine(Enums.LogLevel logLevel);

}
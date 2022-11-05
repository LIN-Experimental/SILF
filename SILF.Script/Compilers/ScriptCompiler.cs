namespace SILF.Script.Compilers;


internal class ScriptCompiler
{

    /// <summary>
    /// Código
    /// </summary>
    private readonly string Code;



    /// <summary>
    /// Nuevo compilador de scripts
    /// </summary>
    /// <param name="code">Código.</param>
    public ScriptCompiler(string code)
    {
        this.Code = code;
    }


}
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



    /// <summary>
    /// Aplicar compilación de código.
    /// </summary>
    public CompileResult Compile(Instance instance)
    {

        CompileResult result = new();

        string[] code = Code.Split('\n');

        var functions = Builders.FunctionBuilder.GetFunctions(instance, code);

        result.Functions = functions;


        return result;

    }


}
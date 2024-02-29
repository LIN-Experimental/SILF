namespace SILF.Script.Compilers;


/// <summary>
/// Nuevo compilador de scripts
/// </summary>
/// <param name="code">Código.</param>
internal class ScriptCompiler(string code)
{


    /// <summary>
    /// Código
    /// </summary>
    private readonly string Code = code;



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
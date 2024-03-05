namespace SILF.Script.Compilers;


/// <summary>
/// Nuevo compilador de scripts
/// </summary>
/// <param name="code">Código.</param>
internal class ScriptCompiler(string code)
{


    /// <summary>
    /// Código.
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


        foreach (var f in functions)
        {
            try
            {
                var structure = Builders.FunctionBuilder.ParseCode(f.CodeLines, instance);
                f.CodeLines = structure.Lines;
            }
            catch { }

        }

        result.Functions = functions;


        return result;

    }


}
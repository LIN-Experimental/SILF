namespace SILF.Script.Elements;


internal class CompileResult
{

    /// <summary>
    /// Lista de funciones
    /// </summary>
    public List<Function> Functions { get; set; } = new();



    /// <summary>
    /// Obtiene la función de arranque
    /// </summary>
    public Function? GetMain()
    {
        string mainName = "main";
        var mainFunc = Functions.Where(T => T.Name == mainName).FirstOrDefault();
        return mainFunc;
    }


}

namespace SILF.Script.Elements;

internal class SILFClass
{
    public string Name { get; set; } = string.Empty;

    public List<string> Lineas { get; set; } = [];

    public List<IFunction> Functions { get; set; } = [];
}

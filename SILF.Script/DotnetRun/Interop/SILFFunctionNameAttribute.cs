namespace SILF.Script.DotnetRun.Interop;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class SILFFunctionNameAttribute(string name, string? tipo = null) : Attribute
{
    public string Name { get; } = name;
    public string? Type { get; } = tipo;
}
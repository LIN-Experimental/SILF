namespace SILF.Script.Elements.Expressions;

internal class CodeBlock
{

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">value</param>
    public CodeBlock(string value = "")
    {
        this.Value = value;
    }

}

namespace SILF.Script.Expressions.Objects;


internal class FieldResult
{

    public string Name { get; set; }
    public string Type { get; set; }
    public string Expression { get; set; }
    public bool Success { get; set; }


    public FieldResult(string name, string type, string expression, bool success = true)
    {
        this.Name = name;
        this.Type = type;
        this.Expression = expression;
        this.Success = success;
    }


    public FieldResult(bool success = false) : this(string.Empty, string.Empty, string.Empty, success)
    {

    }


}
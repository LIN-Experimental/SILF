namespace SILF.Script.Elements.Expressions;


internal class Value
{

    public string Expression { get; set; }
    public bool IsOperator { get; set; }


    public Value(string expression, bool isOperator = false)
    {
        this.Expression = expression;
        this.IsOperator = isOperator;
    }

}

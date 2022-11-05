namespace SILF.Script.Actions;


internal class Options
{

    public static bool IsNumber(string expression)
    {
        bool isNumber = decimal.TryParse(expression, out _);

        return isNumber;

    }


    public static bool IsBool(string expression)
    {

        string[] values = { "false", "true" };

        return values.Contains(expression);

    }

}

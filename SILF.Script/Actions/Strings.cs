namespace SILF.Script.Actions;


public class Strings
{


    public static IEnumerable<string> SepararPorLlaves(string cadena)
    {

        List<string> result = new List<string>();
        int counter = 0;
        string command = string.Empty;
        string values = string.Empty;
        bool isCommand = false;


        foreach (var @char in cadena)
        {

            if (@char == '{')
            {
                counter++;

                if (!isCommand)
                {
                    isCommand = true;
                    result.Add(values);
                    values = "";
                }
            }

            if (@char == '}')
            {
                counter--;
                if (isCommand && counter == 0)
                {
                    isCommand = false;
                    result.Add(command + "}");
                    command = "";
                    continue;
                }
            }


            if (isCommand)
            {
                command += @char;
            }
            else
            {
                values += @char;
            }
        

        }

        if (isCommand)
            result.Add(command);
        else
            result.Add(values);


        return result;
    }


}

namespace SILF.Script.Builders;


internal class FunctionBuilder
{


    public static ControlStructure ParseCode(IEnumerable<string> code, Instance instance)
    {

        ControlStructure root = new("Root");

        Stack<ControlStructure> stack = new();
        stack.Push(root);

        int id = 0;

        foreach (string line in code)
        {

            if (line.Trim() == "")
                continue;

            // IF
            if (Regex.IsMatch(line.Trim(), @"^if\s*\(.+\)\s*$"))
            {
                id++;
                ControlStructure newStructure = new IfStructure()
                {
                    Id = id,
                    Expression = line.Trim().Remove(0, 2)
                };

                var peek = stack.Peek();

                peek.InnerStructures.Add(newStructure);
                peek.Lines.Add($"?i{id}");

                instance.Structures.Add(newStructure);

                stack.Push(newStructure);
                continue;
            }

            // Foreach
            var z = Regex.Match(line.Trim(), @"^for\s*\(\s*(?<varName>\w+)\s+in\s+(?<collection>.+?)\s*\)\s*$");
            if (z.Success)
            {

                id++;
                ControlStructure newStructure = new ForStructure()
                {
                    Id = id,
                    Name = z.Groups["varName"].Value,
                    Expression = z.Groups["collection"].Value,
                };

                var peek = stack.Peek();

                peek.InnerStructures.Add(newStructure);

                peek.Lines.Add($"?f{id}");

                instance.Structures.Add(newStructure);

                stack.Push(newStructure);
            }


            else if (Regex.IsMatch(line.Trim(), @"^}$"))
            {
                stack.Pop();
            }





            else
            {
                stack.Peek().AddLine(line.Trim());
            }
        }

        return root;
    }







    public class ControlStructure
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<string> Lines { get; set; }
        public List<ControlStructure> InnerStructures { get; set; }

        public ControlStructure(string type)
        {
            Type = type;
            Lines = new List<string>();
            InnerStructures = new List<ControlStructure>();
        }


        public virtual void AddLine(string line)
        {
            Lines.Add(line);
        }


    }


    public class ForStructure : ControlStructure
    {

        public string Name { get; set; } = string.Empty;

        public string Expression { get; set; } = string.Empty;

        public ForStructure() : base("for")
        {
        }

    }


    public class IfStructure : ControlStructure
    {

        public bool IsElse = false;

        public string Expression { get; set; } = string.Empty;

        public List<string> ElseLines { get; set; } = [];

        public IfStructure() : base("if")
        {
        }


        public override void AddLine(string line)
        {

            if (IsElse)
            {
                ElseLines.Add(line);
                return;
            }

            base.AddLine(line);
        }

    }


}
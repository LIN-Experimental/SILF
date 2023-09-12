namespace SILF.Script.Elements.Functions;


internal class Function
{

    public string Name { get; set; }

    public List<string> CodeLines { get; set; }

    public List<Parameter> Parameters { get; set; }

    public Tipo Type { get; set; }


    public Function(string name, Tipo tipo)
    {
        this.Name = name;
        this.Type = tipo;
        this.CodeLines = new();
        this.Parameters = new();
    }


}

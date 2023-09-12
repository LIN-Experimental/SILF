namespace SILF.Script.Elements.Functions;


internal class Parameter
{

    public string Name { get; set; }
    public Tipo Tipo { get; set; }


    public Parameter(string name, Tipo tipo)
    {
        this.Name = name;
        this.Tipo = tipo;
    }

}

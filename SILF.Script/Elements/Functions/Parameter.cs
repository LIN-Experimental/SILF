namespace SILF.Script.Elements.Functions;


public class Parameter
{

    public string Name { get; set; }
    public Tipo Tipo { get; set; }
    public bool Hidden { get; set; }    



    public Parameter(string name, Tipo tipo, bool hidden = false)
    {
        this.Name = name;
        this.Tipo = tipo;
        Hidden = hidden;
    }

}

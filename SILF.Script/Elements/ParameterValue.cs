namespace SILF.Script.Elements;


public class ParameterValue
{

    public string Name { get; set; }
    public Tipo Tipo { get; set; }

    public object Value { get; set; }

    public ParameterValue(string name, Tipo tipo, object value)
    {
        this.Name = name;
        this.Tipo = tipo;
        this.Value = value;

    }





}

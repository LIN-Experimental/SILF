namespace SILF.Script.Elements;


public class Value
{


    /// <summary>
    /// Tipo del valor
    /// </summary>
    public Tipo Tipo { get; set; }



    /// <summary>
    /// Valor de la variable
    /// </summary>
    public object Element { get; set; }



    public Value(object element, Tipo tipo = new())
    {
        this.Element = element;
        this.Tipo = tipo;
    }


}

namespace SILF.Script.Elements;


internal class Eval
{

    /// <summary>
    /// Expresión
    /// </summary>
    public object Value { get; set; } = string.Empty;



    /// <summary>
    /// Tipo resultante
    /// </summary>
    public Tipo? Tipo { get; set; }


    /// <summary>
    /// Es Void
    /// </summary>
    public bool IsVoid { get; set; }



    /// <summary>
    /// Nueva evaluación.
    /// </summary>
    /// <param name="value">valor</param>
    public Eval(object value, Tipo? tipo, bool isVoid = false)
    {
        this.Value = value;
        this.Tipo = tipo;
        this.IsVoid = isVoid;
    }


    public Eval(bool isVoid = false)
    {
        this.Value = "";
        this.Tipo = new();
        this.IsVoid = isVoid;
    }

}
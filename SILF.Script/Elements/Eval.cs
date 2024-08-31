namespace SILF.Script.Elements;


internal class Eval
{


    public Objects.SILFObjectBase Object { get; set; }


    /// <summary>
    /// Es Void
    /// </summary>
    public bool IsVoid { get; set; }



    /// <summary>
    /// Nueva evaluación.
    /// </summary>
    /// <param name="value">valor</param>
    public Eval(Objects.SILFObjectBase Object, bool isVoid = false)
    {
        this.Object = Object;
        this.IsVoid = isVoid;
    }



    public Eval(bool isVoid = false)
    {
        this.Object = Objects.SILFNullObject.Create();
        this.IsVoid = isVoid;
    }

}
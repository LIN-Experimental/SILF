namespace SILF.Script.Runtime;

public class ObjectContext
{

    public SILFObjectBase SILFObjectBase { get; set; }



    public static ObjectContext GenerateContext(SILFObjectBase @base)
    {
        return new()
        {
            SILFObjectBase = @base
        };
    }


}
using SILF.Script.Objects;

namespace SILF.Script.Elements;


public class ParameterValue
{

    public string Name { get; set; }

    public SILFObjectBase Objeto { get; set; }



    public ParameterValue(string name, SILFObjectBase objectBase)
    {
        this.Name = name;
        this.Objeto = objectBase;

    }





}

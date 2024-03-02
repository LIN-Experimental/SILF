using SILF.Script.Objects;

namespace SILF.Script.Interfaces;


public interface IProperty
{

    public Tipo? Type { get; set; }

    public string Name { get; set; }

    public Objects.SILFObjectBase Value { get; set; }

    public SILFObjectBase Parent { get; set; }

    public IFunction Get {  get; set; }
    public IFunction Set {  get; set; }


    public SILFObjectBase GetValue(Instance instance);

    public void SetValue(Instance instance, SILFObjectBase @base);


}
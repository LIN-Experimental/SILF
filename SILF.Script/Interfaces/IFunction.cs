namespace SILF.Script.Interfaces;


public interface IFunction
{

    public Tipo Type { get; set; }

    public string Name { get; set; }

    public List<Parameter> Parameters { get; set; }

    public FuncContext Run(Instance instance, List<ParameterValue> values);

}
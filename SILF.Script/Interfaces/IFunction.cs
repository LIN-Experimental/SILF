namespace SILF.Script.Interfaces;

public interface IFunction
{

    /// <summary>
    /// Nombre de la función.
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Contexto.
    /// </summary>
    public Context Context { get; set; }


    /// <summary>
    /// Tipo de la función.
    /// </summary>
    public Tipo? Type { get; set; }


    /// <summary>
    /// Lista de parámetros.
    /// </summary>
    public List<Parameter> Parameters { get; set; }


    /// <summary>
    /// Ejecutar la función.
    /// </summary>
    /// <param name="instance">Instancia.</param>
    /// <param name="values">Valores de los parámetros.</param>
    public FuncContext Run(Instance instance, List<ParameterValue> values, ObjectContext @object);

}
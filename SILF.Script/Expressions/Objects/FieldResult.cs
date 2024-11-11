namespace SILF.Script.Expressions.Objects;

internal class FieldResult(string name, string type, string expression, bool success = true)
{

    /// <summary>
    /// Nombre.
    /// </summary>
    public string Name { get; set; } = name;


    /// <summary>
    /// Tipo.
    /// </summary>
    public string Type { get; set; } = type;


    /// <summary>
    /// Expresión.
    /// </summary>
    public string Expression { get; set; } = expression;


    /// <summary>
    /// Correcto.
    /// </summary>
    public bool Success { get; set; } = success;


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="success"></param>
    public FieldResult(bool success = false) : this(string.Empty, string.Empty, string.Empty, success)
    {
    }

}
namespace SILF.Script.Elements;


internal class Function
{

    /// <summary>
    /// Guarda el nombre de la función.
    /// </summary>
    private string name = string.Empty;


    /// <summary>
    /// Obtiene el nombre de la función.
    /// </summary>
    public string Name => name;


    /// <summary>
    /// Tipo de la función.
    /// </summary>
    public Tipo Type { get; set; }



    /// <summary>
    /// Nueva función.
    /// </summary>
    /// <param name="name">Nombre</param>
    /// <param name="type">Tipo</param>
    public Function(string name, Tipo type)
    {
        this.name = name.ToLower();
        Type = type;
    }



}
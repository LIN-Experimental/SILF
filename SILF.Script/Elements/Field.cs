namespace SILF.Script.Elements;


internal class Field
{

    /// <summary>
    /// Nombre de la variable
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Si el elemento fue asignado
    /// </summary>
    public bool IsAssigned { get; set; }


    /// <summary>
    /// Valor de la variable
    /// </summary>
    public Value Value { get; set; }


    /// <summary>
    /// Tipo de la variable
    /// </summary>
    public Tipo Tipo { get; set; }


    /// <summary>
    /// Nivel de asolación
    /// </summary>
    public Isolation Isolation { get; set; }



    /// <summary>
    /// Constructor
    /// </summary>
    public Field(string name, Value value, Tipo tipo, Isolation isolation = Isolation.ReadAndWrite)
    {
        this.Name = name;
        this.Value = value;
        this.Tipo = tipo;
        this.Isolation = isolation;
    }


}
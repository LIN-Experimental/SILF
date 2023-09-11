namespace SILF.Script;


public readonly record struct Tipo
{


    /// <summary>
    /// Guardar valor
    /// </summary>
    private readonly string _description;



    /// <summary>
    /// Generar nuevo tipo
    /// </summary>
    /// <param name="tipo">nombre del tipo</param>
    public Tipo(string tipo)
    {
        _description = tipo.Trim().ToLower();
    }



    /// <summary>
    /// Obtiene la descripción del tipo
    /// </summary>
    public readonly string Description => _description;


}
namespace SILF.Script;


public struct Tipo
{


    private string _Tipo;


    // Genera un tipo primitivo
    public Tipo(string tipo)
    {
        _Tipo = tipo.Trim().ToLower();
    }




    public string tipo => _Tipo;



}

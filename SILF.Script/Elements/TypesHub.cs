namespace SILF.Script.Elements;


public class TypesHub : List<Tipo>
{

    public Tipo? this[string name]
    {
        get
        {
            // Tipo
            var tipo = Find(T => T.Description == name);

            // Si el tipo es null
            if (tipo.Description == null || tipo.Description == "")
                return null;

            // Retorna el tipo encontrado.
            return tipo;

        }
    }

}
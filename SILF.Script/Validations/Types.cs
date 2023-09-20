namespace SILF.Script.Validations;


internal class Types
{


    /// <summary>
    /// Validación de tipos
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="tipoA">Tipo A</param>
    /// <param name="tipoB">Tipo B</param>
    public static bool IsCompatible(Instance instance, Tipo? tipoA, Tipo? tipoB)
    {

        if (!tipoA.HasValue && !tipoB.HasValue)
            return false;

        if (!tipoA.HasValue || !tipoB.HasValue)
            return false;

        // Si el tipo es mutable
        if (tipoA.Value.Description == "mutable")
            return true;
        
        // Si el tipo no es igual
        return tipoA.Value.Description == tipoB.Value.Description;

    }


}
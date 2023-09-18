namespace SILF.Script.Validations;


internal class Types
{


    public static bool IsCompatible(Instance instance, Tipo tipoA, Tipo tipoB)
    {

        if (tipoA.Description == "mutable")
        {
            return true;
        }


        return tipoA.Description == tipoB.Description;



    }

}
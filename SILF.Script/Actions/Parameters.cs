namespace SILF.Script.Actions;


internal class Parameters
{


    /// <summary>
    /// Construye los parámetros
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="function">Función</param>
    /// <param name="params">Parámetros.</param>
    public static bool BuildParams(Instance instance, IFunction function, List<ParameterValue> @params)
    {

        // Si no hay la misma cantidad de parámetros (bloques).
        if (function.Parameters.Count != @params.Count)
        {
            instance.WriteError("SC001", $"La función '{function.Name}' necesita {function.Parameters.Count} parámetros.");
            return false;
        }

        // Armar los valores (Parámetros).
        for (int index = 0; index < function.Parameters.Count; index++)
        {
            // Parámetro.
            Parameter parameter = function.Parameters[index];

            // Valor.
            ParameterValue parameterValue = @params[index];

            // Son compatibles los tipos.
            bool isCompatible = Validations.Types.IsCompatible(instance, parameter.Tipo, parameterValue.Objeto.Tipo);

            // Si no son compatibles.
            if (!isCompatible)
            {
                instance.WriteError("SC002",$"El parámetro '{parameter.Name}' de La función '{function.Name}' no puede tomar valores del tipo <{parameterValue.Objeto.Tipo.Description}>.");
                return false;
            }

            // Remplaza el valor del nombre.
            parameterValue.Name = parameter.Name;

        }

        // Success
        return true;

    }


}

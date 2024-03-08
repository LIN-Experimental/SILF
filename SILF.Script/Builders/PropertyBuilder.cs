namespace SILF.Script.Builders;


internal class PropertyBuilder
{

    /// <summary>
    /// Obtiene la lista de funciones
    /// </summary>
    /// <param name="instance">Instancia de la app.</param>
    /// <param name="code">Líneas de código.</param>
    public static List<Property> Build(Instance instance, IEnumerable<string> code)
    {

        // Funciones obtenidas.
        List<Property> properties = [];

        // Recorrer el código
        foreach (var line in code)
        {

            // Obtiene el resultado
            var isMatch = Actions.Functions.MatchProperty(line, out string tipo, out string name);

            // Si no es una función
            if (!isMatch)
            {
                continue;
            }


            var normalType = instance.Library.Exist(tipo);

            if (normalType == null && tipo != "void")
            {
                instance.WriteError("SC012", $"el tipo '{tipo}' de la función '{name}' no existe.");
                continue;
            }



            // Función actual.
            Property? property = new(name, normalType);

            properties.Add(property);

        }

        return properties;

    }

}
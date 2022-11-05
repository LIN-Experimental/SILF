namespace SILF.Script.Elements;


internal class Context
{

    /// <summary>
    /// Lista de campos
    /// </summary>
    private readonly List<Field> Fields = new();



    /// <summary>
    /// Obtiene un campo
    /// </summary>
    /// <param name="name">Nombre</param>
    public Field? GetField(string name)
    {
        var field = (from F in Fields
                     where F.Name.ToLower() == name.ToLower()
                     select F).FirstOrDefault();

        return field;
    }



    /// <summary>
    /// Nuevo campo
    /// </summary>
    public bool SetField(Field field)
    {

        var exitField = GetField(field.Name);

        if (exitField == null)
        { 
            Fields.Add(field);
            return true;
        }

        return false;
    }


}
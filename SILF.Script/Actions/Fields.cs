namespace SILF.Script.Actions;


internal class Fields
{


    /// <summary>
    /// Crear nueva variable.
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="context">Contexto</param>
    /// <param name="expression">Expression</param>
    public static bool CreateVar(Instance instance, Context context, FuncContext funcContext, string name, string type, string? expression)
    {

        // Validar Nombre
         bool isValidName = Validations.Options.IsValidName(name);

        if (!isValidName)
        {
            instance.WriteError("SC006", $"El nombre '{name}' es invalido.");
            return false;
        }

        // Validar Tipo
        Tipo? tipo = null;

        // Tipo explicito
        if (type != "let")
        {

            // 
            tipo = instance.Library.Exist(type);

            if (tipo == null)
            {
                instance.WriteError("SC007", $"El tipo '{type}' es invalido.");
                return false;
            }

        }

        if (type == "let" && expression == null)
        {
            instance.WriteError("SC008", $"Las variables con tipo implícito se tienen que asignar");
            return false;
        }

        // Obtiene el valor
        Eval? value = null;
        bool assigned = true;


        if (expression != null)
        {

            var values = MicroRunner.Runner(instance, context, funcContext, expression, 1);


            if (values.Count <= 0 ||values.Count > 1) 
            {
                instance.WriteError("SC009", $"Los valores deben de ser validos");
                return false;
            }

            value = values[0];


            if (value.IsVoid)
            {
                instance.WriteError("SC010", $"El valor de la variable '{name}' no puede ser void");
                return false;
            }


            tipo ??= value.Object.Tipo;

            if (!Validations.Types.IsCompatible(instance, tipo.Value, value.Object.Tipo))
            {
                instance.WriteError("SC011",$"El tipo <{value.Object.Tipo}> no puede ser convertido en <{tipo.Value}>.");
                return false;
            }

        }
        else
        {
            assigned = false;
            var desType = new Tipo(type);

            if (string.IsNullOrWhiteSpace(desType.Description))
            {
                instance.WriteError("SC012", $"El tipo <{type}> no se encontró o aun no esta definido.");
                return false;
            }

            value = new(new Objects.SILFNullObject());
        }


        Field field = new()
        {
            Name = name,
            Instance = instance,
            IsAssigned = assigned,
            Isolation = Isolation.ReadAndWrite,
            Tipo = value.Object.Tipo
        };

        return CreateField(field, instance, context, value.Object.Tipo.Description, value.Object);

    }



    /// <summary>
    /// Crear nueva constante.
    /// </summary>
    /// <param name="instance">Instancia de la app</param>
    /// <param name="context">Contexto</param>
    /// <param name="expression">Expression</param>
    public static bool CreateConst(Instance instance, Context context, FuncContext funcContext, string name, string expression)
    {

        // Validar Nombre
        bool isValidName = Validations.Options.IsValidName(name);

        if (!isValidName)
        {
            instance.WriteError("SC006", $"El nombre '{name}' es invalido.");
            return false;
        }


        // Obtiene el valor
        var values = MicroRunner.Runner(instance, context, funcContext, expression, 1);


        if (values.Count <= 0 || values.Count > 1)
        {
            instance.WriteError("SC009", $"Los valores deben de ser validos");
            return false;
        }


        var value = values[0];

        if (value.IsVoid)
        {
            instance.WriteError("SC010", $"El valor de la constante '{name}' no puede ser void");
            return false;
        }

        if (value.Object.Tipo.Description == "mutable")
        {
            instance.WriteError("SC014", $"El valor de la constante '{name}' no puede ser mutable");
            return false;
        }




        Field field = new()
        {
            Name = name,
            Instance = instance,
            IsAssigned = true,
            Isolation = Isolation.Read,
            Tipo = value.Object.Tipo
        };


        return CreateField(field, instance, context, value.Object.Tipo.Description, value.Object);

    }



    /// <summary>
    /// Crear campo.
    /// </summary>
    /// <param name="field">Campo.</param>
    /// <param name="instance">Estancia.</param>
    /// <param name="context">Contexto.</param>
    /// <param name="type">Tipo</param>
    /// <param name="value">Valor.</param>
    private static bool CreateField(Field field, Instance instance, Context context, string type, SILFObjectBase value)
    {

        // Valor.
        field.Value = (instance.Environment == Environments.PreRun)
                      ? instance.Library.Get(type) 
                      : value;

        // Establecer.
        var can = context.SetField(field);

        // Si hubo un error.
        if (!can)
        {
            instance.WriteError("SC013", "campo duplicada.");
            return false;
        }

        // Correcto.
        return true;

    }



}

using SILF.Script.Objects;

namespace SILF.Script.Actions;


internal class Fields
{








    public static Tipo? GetTipo(Instance instance, string tipo)
    {
        var type = instance.Tipos.Where(T => T.Description == tipo);

        if (type.Any())
            return type.ElementAt(0);

        return null;
    }




    /// <summary>
    /// Crear nueva variable
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
            instance.WriteError($"El nombre '{name}' es invalido.");
            return false;
        }

        // Validar Tipo
        Tipo? tipo = null;

        // Tipo explicito
        if (type != "let")
        {

            // 
            tipo = GetTipo(instance, type);

            if (tipo == null)
            {
                instance.WriteError($"El tipo '{type}' es invalido.");
                return false;
            }

        }

        if (type == "let" && expression == null)
        {
            instance.WriteError($"Las variables con tipo implícito se tienen que asignar");
            return false;
        }

        // Obtiene el valor
        Eval? value = null;
        bool assigned = true;


        if (expression != null)
        {
            var values = MicroRunner.Runner(instance, context, funcContext, expression, 1);


            if (values.Count != 1)
            {
                instance.WriteError($"No MMM");
                return false;
            }
            value = values[0];


            if (value.IsVoid)
            {
                instance.WriteError($"El valor de la variable '{name}' no puede ser void");
                return false;
            }

            tipo ??= value.Object.Tipo;

            if (!Validations.Types.IsCompatible(instance, tipo.Value, value.Object.Tipo))
            {
                instance.WriteError($"El tipo <{tipo.Value}> no puede ser convertido en <{value.Object.Tipo}>.");
                return false;
            }

        }
        else
        {
            assigned = false;
            var desType = instance.Tipos.Where(T => T.Description == type).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(desType.Description))
            {
                instance.WriteError($"El tipo <{type}> no se encontró o aun no esta definido.");
                return false;
            }

            value = new( new Objects.SILFNullObject());
        }


        Field field = new()
        {
            Name = name,
            Instance = instance,
            IsAssigned = assigned,
            Isolation = Isolation.ReadAndWrite,
            Tipo = value.Object.Tipo
        };

        if (instance.Environment == Environments.PreRun)
        {
            field.Value = SILFObjectBase.GetByName(value.Object.Tipo.Description);
        }
        else
        {
            field.Value = value.Object;
        }


        var can = context.SetField(field);

        if (!can)
        {
            instance.WriteError("campo duplicada.");
            return false;
        }


        return true;

    }



    /// <summary>
    /// Crear nueva constante
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
            instance.WriteError($"El nombre '{name}' es invalido.");
            return false;
        }


        // Obtiene el valor
        var values = MicroRunner.Runner(instance, context, funcContext, expression, 1);

        if (values.Count != 1)
        {
            instance.WriteError($"No MMM");
            return false;
        }
        var value = values[0];

        if (value.IsVoid)
        {
            instance.WriteError($"El valor de la constante '{name}' no puede ser void");
            return false;
        }

        if (value.Object.Tipo.Description == "mutable")
        {
            instance.WriteError($"El valor de la constante '{name}' no puede ser mutable");
            return false;
        }




        Field field = new()
        {
            Name = name,
            Instance = instance,
            IsAssigned = true,
            Isolation = Isolation.ReadAndWrite,
            Tipo = value.Object.Tipo
        };

        if (instance.Environment == Environments.PreRun)
        {
            field.Value = SILFObjectBase.GetByName(value.Object.Tipo.Description);
        }
        else
        {
            field.Value = value.Object;
        }


        var can = context.SetField(field);

        if (!can)
        {
            instance.WriteError("campo duplicada.");
            return false;
        }


        return true;


    }



}

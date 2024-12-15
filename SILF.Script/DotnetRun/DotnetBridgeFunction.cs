namespace SILF.Script.DotnetRun;

public class DotnetBridgeFunction : IFunction
{
    public Tipo? Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Parameter> Parameters { get; set; } = [];
    public Context Context { get; set; } = null!;
    public Delegate Action { get; set; }
    public bool IsDependency { get; set; }


    public FuncContext Run(Instance instance, List<ParameterValue> values, ObjectContext @object)
    {


        if (IsDependency)
        {
            values.Insert(0, new("DI", new SILFObjectBase()
            {
                Value = instance.ServiceProvider
            } ));
        }


        // Convertir a tipos compatibles.
        var x = Action?.DynamicInvoke(values.Select(t => t.Objeto.GetValue()).ToArray());

        if (EsTask(x))
        {

            if (EsTaskGenerico(x))
            {
                var espera = EsperarTaskGenerico(x);
                espera.Wait();

                x = espera.Result;
            }
            else
            {
                var ss = (x as Task);
                ss.Wait();
                x = "";
            }

        }

        return new()
        {
            IsReturning = true,
            ObjectContext = new() { SILFObjectBase = new() { Value = x, Tipo = this.Type ?? new() } },
            Value = new() { Value = x },
            WaitType = Type,
        };
    }



    // Método para verificar si un objeto es un Task
    public static bool EsTask(object obj)
    {
        if (obj == null) return false;
        return obj is Task;
    }

    // Método para verificar si un objeto es un Task genérico
    public static bool EsTaskGenerico(object obj)
    {
        if (obj == null) return false;

        Type tipoTarea = obj.GetType();
        return tipoTarea.IsGenericType && tipoTarea.GetGenericTypeDefinition() == typeof(Task<>);
    }

    // Método para esperar un Task genérico usando dynamic
    public static async Task<object> EsperarTaskGenerico(object obj)
    {
        if (!EsTaskGenerico(obj))
            throw new ArgumentException("El objeto no es un Task genérico", nameof(obj));

        dynamic taskDinamico = obj;
        return await taskDinamico;
    }


}

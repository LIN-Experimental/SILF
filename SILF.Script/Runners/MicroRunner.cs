using SILF.Script.Elements.Functions;

namespace SILF.Script.Runners;


internal class MicroRunner
{


    /// <summary>
    /// Runner.
    /// </summary>
    /// <param name="instance">Instancia principal de la app.</param>
    /// <param name="context">Contexto.</param>
    /// <param name="expression">Expresión a evaluar.</param>
    /// <param name="level">Nivel de insolación.</param>
    public static Eval Runner(Instance instance, Context context, FuncContext funcContext, string expression, short level)
    {
        // Si la app esta detenida
        if (!instance.IsRunning)
            return new("", new());

        // Obtiene la expresión separada
        var bloques = Actions.Blocks.GetOperators(expression, instance);

        List<Eval> evals = new();
        foreach (var bloque in bloques)
        {

            if (bloque.Tipo.Description == "operator")
            {
                evals.Add(bloque);
                continue;
            }

            var result = ScriptInterpreter.Interprete(instance, context, funcContext, bloque.Value.ToString() ?? "", level);
            evals.Add(result);

        }


        Actions.PEMDAS calc = new (instance, evals);

        calc.Solve();

        if (evals == null || evals.Count != 1)
            return new("", new(), true);

        return evals[0];


    }



}
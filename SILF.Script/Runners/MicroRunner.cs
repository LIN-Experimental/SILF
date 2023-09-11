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
    public static Eval Runner(Instance instance, Context context, string expression, short level)
    {

        var bloques = Actions.Blocks.GetOperators(expression, instance);

        List<Eval> evals = new();
        foreach (var bloque in bloques)
        {

            if (bloque.Tipo.Description == "operator")
            {
                evals.Add(bloque);
                continue;
            }

            var result = ScriptInterpreter.Interprete(instance, context, bloque.Value.ToString() ?? "", level);
            evals.Add(result);
        }


        Actions.PEMDAS calc = new Actions.PEMDAS(instance, evals);


        calc.Solve();

        return evals[0];


    }



}
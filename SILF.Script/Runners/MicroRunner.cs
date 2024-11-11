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
    public static List<Eval> Runner(Instance instance, Context context, FuncContext funcContext, ObjectContext classContext, string expression, short level)
    {

        try
        {
            // Si la app esta detenida
            if (!instance.IsRunning)
                return [];

            // Obtiene la expresión separada
            var bloques = Actions.Blocks.Separar(expression);

            // Si no hay bloques
            if (bloques.Count <= 0)
                return [];

            // Resultados
            List<Eval> final = [];

            foreach (var bloque in bloques)
            {

                List<Eval> evals = [];

                // Obtiene la expresión separada
                var expressions = Actions.Blocks.GetOperators(bloque.Value, instance);

                // Recorre
                foreach (var ex in expressions)
                {

                    if (ex.IsOperator)
                    {
                        evals.Add(new()
                        {
                            IsVoid = true,
                            Object = new SILFObjectBase
                            {
                                Tipo = new("operator"),
                                Value = ex.Value,
                            }
                        });
                        continue;
                    }

                    var result = ScriptInterpreter.Interprete(instance, context, classContext, funcContext, ex.Value.ToString() ?? "", level);

                    evals.AddRange(result);

                }

                // Solucionador PEMDAS
                Actions.PEMDAS calcs = new(instance, evals);

                calcs.Solve();

                evals ??= [];


                final.AddRange(evals);

            }

            return final;
        }
        catch (Exception)
        {
        }

        return [];

    }


}
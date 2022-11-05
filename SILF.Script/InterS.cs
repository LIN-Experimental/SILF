

//#pragma warning disable

using SILF.Script;

namespace SILF.Script
{


    /// <summary>
    /// Nececidades especialmenete para genericos
    /// </summary>
    public sealed class Nececidades
    {


        public string clase;
        public string tipo;


        public Nececidades(string clase, string tipo)
        {
            this.clase = clase;
            this.tipo = tipo;
        }

        public Nececidades()
        {
            clase = "array";
            tipo = "obj";
        }


    }


    class InterpretadorS
    {



        public static void Compilar(RichTextBox codigo, string ubicacion, Instance estancia)
        {

            StringBuilder timeComp = new();

            //Hora de inicio
            timeComp.AppendLine("Inicio: " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString());


            // Elimina la carpeta del programa general
            if (Directory.Exists(ubicacion + "compiled/"))
                Directory.Delete(ubicacion + "compiled/", true);


            // Crea las subcarpetas
            string folderPath = ubicacion + "compiled/";
            Compilares.Folders(folderPath);


            // Obtiene el Main
            List<string> Main = Compilares.GetMain(codigo);


            List<string> Main2 = new();
            List<string> Main3 = new();
            List<string> Main4 = new();
            List<string> Importaciones = new();

            // Funciones
            Dictionary<string, string> WERTY = new();


            //Obtiene las funciones
            {
                // nombre y tipo de la funcion
                string name = "";
                string type = "";

                // Argumentos de la funcion
                var Args = "";

                var Bander = false;
                RichTextBox lx = new RichTextBox();


                foreach (string S in codigo.Lines)
                {
                    if (S.Replace(" ", "").ToLower().StartsWith("funt"))
                    {
                        Bander = true;

                        name = S.Split(' ')[2];
                        type = S.Split(' ')[1];


                        {
                            var E1 = "";
                            E1 = S.ExtractFrom("(", ")");
                            Args = E1.Trim();
                        }
                    }
                    else if (S.Replace(" ", "").ToLower() == "endfunt")
                    {
                        Bander = false;
                        File.WriteAllText(folderPath + @"functions\" + name + ".sfc", $"({type.ToLower().Trim()})" + Args + Environment.NewLine + lx.Text);
                        WERTY.Add(name, folderPath + @"functions\" + name + ".sfc");
                        lx.ResetText();
                        name = "";
                        type = "";
                        Args = "";
                    }
                    else if (Bander == true)
                    {
                        string S2 = S.Trim();
                        lx.Text = lx.Text + S2 + Environment.NewLine;
                    }
                }

            }


            // Obtiene IF, FOR , WHILE
            {
                // IF
                try
                {
                    Main2 = Compilacion.GetIf(Main, folderPath);
                    foreach (var s in WERTY.Values)
                        Compilacion.GetIf2(s, folderPath + @"ifs\");
                }
                catch { }

                // FOR
                try
                {
                    Main3 = Compilacion.GetFOR(Main2, folderPath);

                    foreach (var s in WERTY.Values)
                        Compilacion.GetFOR2(s, folderPath + @"do\");
                }
                catch { }

                // WHILE
                try
                {
                    Main4 = Compilacion.GetWHILE(Main3, folderPath);

                    foreach (var s in WERTY.Values)
                        Compilacion.GetWHILE2(s, folderPath + @"while\");
                }
                catch
                {
                }
            }


            //Proceso de ejecucion
            {


                // Jerarquia y tipos de datos
                List<ID> accesos = new List<ID>();
                List<Objeto> Base = new List<Objeto>();
                StatusApp sett = new StatusApp(false, "", false);


                // Agrega las funciones
                foreach (var pair in WERTY)
                {
                    FuncionSILF IOQ = new FuncionSILF(pair.Key, pair.Value);
                    estancia.Funciones.Add(IOQ);
                }


                // Crea las variables primordiales
                Create.Elemento(Base, accesos, "", "null", Seguridad.constant, "", false);
                Create.Elemento(Base, accesos, "newline", "string", Seguridad.constant, Environment.NewLine, false);
                Create.Elemento(Base, accesos, "pi", "float", Seguridad.constant, "3.1415926535897931", false);
                Create.Elemento(Base, accesos, "tau", "float", Seguridad.constant, "6.2831853071795862", false);
                Create.Elemento(Base, accesos, "e", "float", Seguridad.constant, "2.7182818284590451", false);



                List<string> Ejecutor = new();
                StringBuilder SaveDoc = new();
                foreach (string P in Main4)
                {
                    if (P.Trim() == "" | P.StartsWith("//")) { }
                    else
                    {
                        Ejecutor.Add(P);
                        SaveDoc.AppendLine(P);
                    }
                }



                //Guarda el documento
                File.WriteAllText(folderPath + "main.sfc", SaveDoc.ToString());


                //Hora de inicio de interpretacion
                timeComp.AppendLine("Inicio la ejecucion: " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString());


                // Inicia la interpretacion
                foreach (string Line in Ejecutor)
                    Interprete(estancia, Line, 0, accesos, Base, sett, new());


                //finalizacion de la interpretacion
                timeComp.AppendLine("Finalizo: " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString());

                estancia.OnDebug(timeComp.ToString());
                estancia.OnDebug("--Ejecucion Finaliada--");


                // Elimina las variables y listas creadas
                Base.Clear();
                accesos.Clear();


            }

        }



        public static Devolucion Interprete(Instance estancia, string data, short modo, List<ID> accesos, List<Objeto> Base, StatusApp sett, Nececidades nececidad, string tipoActual = "obj", Flex Flex = Flex.Nulo, ByQ By = ByQ.Byvalue)
        {

            if (data == null | data == "") return null;

            long ElRef = Mvars.GetVarItem(data, accesos);





            // Sentencia Alert
            if (data.ToLower().StartsWith("alert(") & data.EndsWith(")") & modo == 0)
            {
                string value = data.ExtractFrom("(", ")");
                Devolucion rest = Interpretes.Micro(estancia, value, accesos, Base, sett, nececidad, "", Flex.Nulo, By);

                if (rest != null)
                    MessageBox.Show(rest.Dato);
                else
                    MessageBox.Show("");

                return null;
            }


            // Sentencia Print
            else if (data.ToLower().StartsWith("print(") & data.EndsWith(")") & modo == 0)
            {
                string value = data.ExtractFrom("(", ")");
                Devolucion rest = Interpretes.Micro(estancia, value, accesos, Base, sett, nececidad, "", Flex.Nulo, By);

                if (rest != null)
                    estancia.WriteLine(rest.Dato);
                else
                    estancia.WriteLine("");

                return null;
            }


            // Obtiene el dato de una nueva variable
            else if (ElRef > -1 & modo == 1)
            {
                string value = Base[(int)ElRef].Valor;
                string tipo = Base[(int)ElRef].Tipo;
                string loc = ElRef.ToString();

                return new(value, loc, tipo);

            }


            // Variable
            else if (data.ToLower().StartsWith("let ") & modo == 0)
            {
                data = data.Remove(0, 3);
                InterVars.Variables(estancia, data.Trim(), accesos, Base);
                return null;
            }


            // Contantes
            else if (data.ToLower().StartsWith("const ") & modo == 0)
            {
                data = data.Remove(0, 5);
                InterVars.Constantes(estancia, data.Trim(), accesos, Base);
                return null;
            }


            // Ciclo For
            else if (data.StartsWith("execute|do|"))
            {
                // Variables necesarias
                string file = "";
                string tipo = "";
                string variable = "";

                // Lista de ejecucion
                List<string> Ejecutor = new();

                // Obiene los datos a ejecutar
                string ant = data.Split('|')[0] + data.Split('|')[1] + data.Split('|')[2] + data.Split('|')[3] + data.Split('|')[4] + "|||||";
                ant = data.Remove(0, ant.Length);

                //Rellena las variables
                {
                    // Tipo del for (Repetidor o Iterador)
                    tipo = data.Split('|')[3];
                    // Obtiene la ubicacion del For
                    file = data.Split('|')[2];
                    // Contante del ciclo for
                    variable = data.Split('|')[4].ToLower();
                }


                // Comprueba el tipo de for
                var Bander = 0;

                // Iniciador si es iter o rep
                if (tipo.ToLower() == "iter")
                    Bander = 1;
                else if (tipo.ToLower() == "rep")
                    Bander = 2;


                //Hace segun el caso
                switch (Bander)
                {
                    //Iterador
                    case 1:
                        {
                            // Variables que se van a leer
                            List<string> listaR = new();
                            Dictionary dictR = new();


                            string tipoColeccion = "";
                            string Datos = ant.ExtractFrom("[", "]").Trim();


                            // Obtiene el nombre del elemento
                            var ListName = "";
                            if (Datos.StartsWith("$"))
                                ListName = Datos.Remove(0, 1);
                            else
                                ListName = Datos;


                            // Obtencion de datos
                            int ia = -1;
                            {
                                List<string> Valores = new();
                                var ty = Interpretes.MicroForGetItems(estancia, ListName, accesos, Base, sett, "", Flex.Nulo, ByQ.ByRefer);

                                Valores.Add(ty.items[0].valor);

                                if (Valores.Count == 0)
                                {
                                    estancia.OnDebug($"Debe estar especificado el acceso a una coleccion");
                                    return null;
                                }
                                else if (Valores.Count == 1)
                                {
                                    string s = Base[int.Parse(Valores[0])].Tipo;

                                    if (IsValid.IsColeccionType(s))
                                    {
                                        tipoColeccion = Base[int.Parse(Valores[0])].Tipo;
                                        ia = int.Parse(Valores[0]);
                                    }
                                    else
                                    {
                                        estancia.OnDebug($"El tipo '{Base[int.Parse(Valores[0])].Tipo}' no puede ser covertido en una coleccion");
                                        return null;
                                    }
                                }
                                else
                                {
                                    estancia.OnDebug($"Debe estar especificado solamente 1");
                                    return null;
                                }


                            }


                            // Obtiene la lista y el tipo de variable a crear
                            string tipoVar = "";
                            {
                                // Hace segun el tipo de coleccion
                                if (tipoColeccion == "array")
                                {
                                    tipoVar = "obj";
                                    listaR = Base[ia].Array;
                                }
                                else if (tipoColeccion == "tuple")
                                {
                                    tipoVar = "obj";
                                    listaR = Base[ia].Array;
                                }
                                else if (tipoColeccion == "list")
                                {
                                    tipoVar = Base[ia].TipoGenerico;
                                    listaR = Base[ia].Array;
                                }
                                else if (tipoColeccion == "stack")
                                {
                                    tipoVar = Base[ia].TipoGenerico;
                                    listaR = Base[ia].Array;
                                }
                                else if (tipoColeccion == "dictionary")
                                {
                                    tipoVar = "pair";
                                    //
                                }

                            }


                            //Datos a ejecutar
                            {
                                List<string> ejecucion = new List<string>();
                                ejecucion = File.ReadAllLines(estancia.RutaDO + file + ".sfc").ToList();

                                int i;

                                if (Exist.Variable(variable, accesos))
                                {
                                    estancia.NewAdvertencia("Ya existe un elemento con el nombre", "00");
                                    return null;
                                }


                                i = (int)Create.Elemento(Base, accesos, variable, tipoVar, Seguridad.var, "", false);
                                var Totales = accesos.Count;


                                //Proceso de pre-ejecucion + ejecucion
                                if (ejecucion.Count > 0)
                                {
                                    ByQ ByW;
                                    if (IsValid.ForVal(tipoVar))
                                        ByW = ByQ.Byvalue;
                                    else
                                        ByW = ByQ.ByRefer;


                                    //Duplica la lista para iterar sobre ella
                                    List<string> g = new();
                                    g.AddRange(listaR);
                                    foreach (string Linea in g)
                                    {

                                        // Remueve las variables creadas del bloque
                                        try
                                        {
                                            List<ID> f = new List<ID>();
                                            f.AddRange(accesos.GetRange(0, Totales));
                                            accesos.Clear();
                                            accesos.AddRange(f);
                                        }
                                        catch { }

                                        if (ByW == ByQ.Byvalue)
                                            Base[i].Valor = Linea;
                                        else
                                        {
                                            int io = 0;
                                            int.TryParse(Linea, out io);
                                            Base[i] = Base[io];

                                        }


                                        List<string> sett2 = new List<string>();

                                        foreach (var TY in ejecucion)
                                        {
                                            Interprete(estancia, TY, 0, accesos, Base, sett, new());

                                            var ro = 0;
                                            foreach (var H in sett2)
                                            {
                                                if (H == "exit-for")
                                                {
                                                    sett.Recojedor.RemoveAt(ro);
                                                    estancia.OnDebug("saliendo del for");
                                                    return null;
                                                }
                                                ro += 1;
                                            }
                                        }
                                    }
                                }




                                // Remueve las variables creadas del bloque
                                try
                                {
                                    List<ID> f = new List<ID>();
                                    f.AddRange(accesos.GetRange(0, Totales - 1));
                                    accesos.Clear();
                                    accesos.AddRange(f);
                                }
                                catch { }

                            }


                            break;


                        }


                    //Repetidor
                    case 2:
                        {
                            string E1 = ant.ExtractFrom("[", "]") ?? "";

                            double DT1 = 0;
                            double DT2 = 0;
                            double DT3 = 1;

                            //Lleva los datos a las variables DT
                            {
                                List<string> K = new();
                                Interpretes.MicroForSeparar(estancia, E1, 1, accesos, Base, sett, K, By);

                                if (K.Count == 3)
                                {
                                    DT1 = K[0].ToDouble();
                                    DT2 = K[1].ToDouble();
                                    DT3 = K[2].ToDouble();
                                }
                                else if (K.Count == 2)
                                {
                                    DT1 = K[0].ToDouble();
                                    DT2 = K[1].ToDouble();
                                    DT3 = 1;
                                }
                                else
                                {
                                    estancia.NewAdvertencia("Error x", "");
                                    return null;
                                }
                            }

                            //Procesos de ejecucion
                            {
                                foreach (var LINEA in File.ReadLines(estancia.RutaDO + file + ".sfc"))
                                    Ejecutor.Add(LINEA.Trim());

                                int ___ki;
                                string ty = "float";
                                if (int.TryParse(DT3.ToString(), out ___ki) == true)
                                {
                                    ty = "int";
                                }

                                int i;
                                if (Exist.Variable(variable, accesos) == false)
                                    i = (int)Create.Elemento(Base, accesos, variable, ty, Seguridad.constant, "", false);
                                else
                                {
                                    estancia.NewAdvertencia("ya existe un elemnto", "");
                                    return null;
                                }


                                int Totales = accesos.Count;


                                //Ejecucion
                                {
                                    for (double G = DT1; G <= DT2; G += DT3)
                                    {
                                        //Elimina variables del bloque
                                        try
                                        {
                                            List<ID> f = new List<ID>();
                                            f.AddRange(accesos.GetRange(0, Totales));
                                            accesos.Clear();
                                            accesos.AddRange(f);
                                        }
                                        catch { }

                                        //Lleva el valor a la variable
                                        Base[i].Valor = G.ToString();

                                        //Genera un Status
                                        StatusApp sett2 = new(false, "");

                                        foreach (string linea in Ejecutor)
                                        {
                                            Interprete(estancia, linea, 0, accesos, Base, sett2, new());

                                            int r = 0;
                                            foreach (var H in sett2.Recojedor)
                                            {
                                                if (H == "exit-for")
                                                {
                                                    sett.Recojedor.RemoveAt(r);
                                                    estancia.OnDebug("saliendo del for");
                                                    return null;
                                                }
                                                r += 1;
                                            }
                                        }
                                    }


                                    //Elimina los residuos del bloque
                                    try
                                    {
                                        List<ID> f = new List<ID>();
                                        f.AddRange(accesos.GetRange(0, Totales - 1));
                                        accesos.Clear();
                                        accesos.AddRange(f);
                                    }
                                    catch { }


                                }



                            }


                            break;

                        }

                }

                estancia.OnDebug($"Se ejecuto un ciclo for");
                return null;
            }


            // Ejecuta un IF
            else if (data.StartsWith("execute|if|"))
            {


                string file = data.Split('|')[2];
                string Datos = "";

                List<string> IFNormal = new();
                List<string> IFelse = new();
                List<string> IFNormal2 = new();
                List<string> IFelse2 = new();

                Datos = data.Split('|')[0] + data.Split('|')[1] + data.Split('|')[2] + "|||";
                Datos = data.Remove(0, Datos.Length);

                string Dato1 = "";

                //Compila los datos
                {
                    string value = Datos.ExtractFrom("[", "]");

                    Dato1 = Interpretes.Micro(estancia, value, accesos, Base, sett, new(), "bool", Flex.FlexibleField, ByQ.Byvalue).Dato;

                }


                //Organiza los datos
                {
                    string BD = "no";
                    foreach (var LINEA in File.ReadLines(estancia.RutaIFs + file + ".sfc"))
                    {
                        if (LINEA == ":nn:")
                            BD = "ren";
                        else if (LINEA == ":else:")
                            BD = "else";
                        else if (BD == "ren")
                            IFNormal.Add(LINEA);
                        else if (BD == "else")
                            IFelse.Add(LINEA);
                    }

                    IFNormal2 = IFNormal.Where(T => T.Trim() != "").ToList();
                    IFelse2 = IFelse.Where(T => T.Trim() != "").ToList();
                }


                //Ejecucion
                {
                    int Totales = accesos.Count;


                    if (Dato1 == IsValid.ValorTrue | Dato1 == "1")
                    {
                        foreach (string xo in IFNormal2)
                            Interprete(estancia, xo, modo, accesos, Base, sett, new(), "", Flex.Nulo, ByQ.Byvalue);
                    }
                    else
                    {
                        foreach (string xo in IFelse2)
                            Interprete(estancia, xo, modo, accesos, Base, sett, new(), "", Flex.Nulo, ByQ.Byvalue);
                    }



                    // ELIMINAR LA VARIABLE
                    try
                    {
                        List<ID> f = new List<ID>();
                        f.AddRange(accesos.GetRange(0, Totales));
                        accesos.Clear();
                        accesos.AddRange(f);
                    }
                    catch { }
                    return null;
                }

            }


            // Llamada a un input
            else if (data.ToLower().StartsWith("input") & modo == 0)
            {
                string[] Tipos = new[] { "text", "num", "date" };

                //Variables
                string nombre = "";
                string tipo;
                string titulo = "";

                //Hace segun el modo
                switch (modo)
                {
                    case 0:
                        {
                            nombre = data.Split(' ')[2].ToLower();
                            tipo = data.Split(' ')[1].ToLower();
                            break;
                        }
                    case 1:
                        {
                            tipo = data.Split(' ')[1].ToLower();
                            break;
                        }
                    default:
                        {
                            tipo = data.Split(' ')[1].ToLower();
                            break;
                        }
                }


                //Comprueba el tipo
                if (Tipos.Contains(tipo) == false)
                {
                    estancia.NewAdvertencia($"No se encontro el tipo '{tipo ?? " "}' de un input", "093");
                }


                //Comprueba si es un nombre valido
                {
                    IsValid.Er aux = IsValid.Name(nombre ?? "");

                    //Inicia con numero
                    if (aux == IsValid.Er.IniciaConNumero)
                    {
                        estancia.OnDebug($"El nombre '{nombre}' es invalido porque inicia con un numero");
                        estancia.NewAdvertencia("El nombre de una variable no puede iniciar con numeros", "001");
                        return null;
                    }

                    //Inicia con simbolo invalido
                    else if (aux == IsValid.Er.StarInvalidChar)
                    {
                        estancia.OnDebug($"El nombre '{nombre}' es invalido porque inicia con un simbolo prohibido");
                        return null;
                    }

                }

                //Comprueba si ya existe la variable nueva
                if (Exist.Variable(nombre, accesos) & modo == 0)
                {
                    estancia.OnDebug($"Ya existe un elemento con el nombre '{nombre}'");
                    return null;
                }


                {

                    //Obtiene el titulo
                    if (data.Contains("(") & data.Contains(")"))
                    {
                        var t = data.ExtractFrom("(", ")");
                        titulo = Interpretes.Micro(estancia, t, accesos, Base, sett, new(), "string", Flex.FlexibleField, By).Dato;
                    }
                    else { titulo = "input"; }


                    //
                    switch (tipo.ToLower())
                    {
                        case "text":
                            {
                                break;
                            }

                        case "num":
                            {
                                break;
                            }

                        case "date":
                            {
                                break;
                            }

                    }


                }

            }



            #region "Clases de S#"


            // Clase Date
            else if (data.Split('.')[0].ToLower() == "date" & modo == 1)
            {

                string value = data.Split('.')[1].ToLower() ?? "";

                switch (value)
                {

                    case "hour":
                        {
                            string resultado = DateTime.Now.Hour.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "minute":
                        {
                            string resultado = DateTime.Now.Minute.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "seconds":
                        {
                            string resultado = DateTime.Now.Second.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "month":
                        {
                            string resultado = DateTime.Now.Month.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "year":
                        {
                            string resultado = DateTime.Now.Year.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "dayweek":
                        {
                            string resultado = DateTime.Now.DayOfWeek.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "dayyear":
                        {
                            string resultado = DateTime.Now.DayOfYear.ToString();
                            return new Devolucion(resultado, "int");
                        }

                    case "day":
                        {
                            string resultado = DateTime.Now.Day.ToString();
                            return new Devolucion(resultado, "int");
                        }

                }



            }


            // Si se va a igualar una variable
            else if (Exist.Variable(data.Split('=')[0].Trim(), accesos) & data.EndsWith(")"))
            {

                string name;
                name = data.Split('=')[0].Trim() ?? "";

                int i = (int)Mvars.GetVarItem(name, accesos);

                if (i == -1)
                {
                    estancia.NewAdvertencia($"No se encontro el elemento {name}", "00");
                    return null;
                }

                //Comprueba la seguridad
                if (Base[i].Seguridad == Seguridad.constant | Base[i].Seguridad == Seguridad.interna)
                {
                    estancia.NewAdvertencia($"No se pueden cambiar los valores de '{name}' porque es una {Base[i].Seguridad}", "00");
                    return null;
                }


                {
                    data = data.ExtractFrom("(", ")");

                    // Si es dinamica
                    if (Base[i].IsDynamic == true)
                    {
                        var tipo = "obj"; //Adivinador de tipos
                        var res = Interpretes.Micro(estancia, data, accesos, Base, sett, new(), tipo, Flex.FlexibleField, By);
                        Base[i].Valor = res.Dato;
                        Base[i].Tipo = res.Tipo;

                        estancia.OnDebug($"Se cambio el valor de '{name}'");
                        return null;
                    }

                    else
                    {
                        var res = Interpretes.Micro(estancia, data, accesos, Base, sett, new(), Base[i].Tipo, Flex.FlexibleField, By); ;


                        if (IsValid.ForVal(Base[i].Tipo))
                        {
                            Base[i].Valor = res.Dato;
                        }
                        else
                        {
                            Base[i] = Base[res.referencia.ToInt32()];
                        }


                        estancia.OnDebug($"Se cambio el valor de '{name}'");
                        return null;
                    }
                }




            }


            // Obtiene el ID de un elemento
            else if (data.Split('(')[0].ToLower() == "getid(")
            {
                string datos = data.ExtractFrom("(", ")");
                datos = datos.Trim();

                var res = Interpretes.Micro(estancia, datos, accesos, Base, sett, new(), "", Flex.Nulo, ByQ.ByRefer);

                return new Devolucion(res.Dato, "int");
            }


            // Si hay un punto haciendo referencia
            else if (Exist.Variable(data.Split('.')[0].Trim(), accesos))
            {
                int i = (int)Mvars.GetVarItem(data.Split('.')[0].Trim(), accesos);

                string tipo = Base[i].Tipo;

                //Si es una coleccion
                if (IsValid.IsColeccionType(tipo))
                    return Interprete(estancia, "$" + data, modo, accesos, Base, sett, new(), tipoActual, Flex, By);

                //Si es otro
                else
                    return Interprete(estancia, "!" + data, modo, accesos, Base, sett, new(), tipoActual, Flex, By);

            }


            // Ejecuta una funcion
            else if (data[0] == '¡')
            {
                // Funcion
                FuncionSILF funcion = new();

                // Lista de argumentos
                List<string> VArgumentos = new();

                // Confirmaciones
                {
                    //Nombre
                    string nombre = data.ExtractFrom2("¡", "(").ToLower() ?? "";

                    //Si no existe la funcion
                    if (Exist.Funcion(estancia, nombre) == false)
                    {
                        estancia.NewAdvertencia($"No se encontro la funcion '{nombre}'", "00");
                        return null;
                    }

                    //Busca la funcion en la estancia
                    foreach (var Funt in estancia.Funciones)
                    {
                        if (Funt == null) { continue; }
                        else if (Funt.Nombre == nombre)
                        {
                            funcion = Funt;
                            break;
                        }
                    }


                    //Compilacion e interpretacion
                    {
                        List<string> Separado;
                        string E1 = data.ExtractFrom("(", ")") ?? "";
                        Separado = new();
                        Separado = InterB.JustSeparar(E1);


                        List<ID> Acceso2 = new List<ID>();
                        int i = 0;

                        //Interprete
                        foreach (var w in funcion.Argumetos)
                        {
                            Flex fx = Flex.FlexibleField;
                            ByQ bq = ByQ.Byvalue;
                            string fName = w.Nombre ?? "";
                            string fTipo = w.Tipo ?? "obj";
                            //Confirma el By y el Flex
                            {
                                if (IsValid.ForVal(fTipo))
                                {
                                    fx = Flex.FlexibleField;
                                    bq = ByQ.Byvalue;
                                }
                                else
                                {
                                    fx = Flex.Seguro;
                                    bq = ByQ.ByRefer;
                                }
                            }

                            var M = "";
                            M = Interpretes.Micro(estancia, Separado[i], accesos, Base, sett, new(), fTipo, fx, bq).Dato ?? "";
                            Create.Elemento(Base, accesos, fName, fTipo, Seguridad.var, M, false);
                            i += 1;
                        }

                        //Crea la nueva "estancia"
                        {
                            string TipoFuncion = funcion.Tipo ?? "";
                            StatusApp Recolector = new(true, TipoFuncion);

                            if (TipoFuncion == "void" | TipoFuncion == null)
                                Recolector.NeedReturns = false;
                            else
                                Recolector.NeedReturns = true;

                            //Ejecuta
                            foreach (string value in funcion.Ejecucion)
                            {
                                Interprete(estancia, value.Trim(), 0, Acceso2, Base, Recolector, new(), "", Flex.Nulo, ByQ.Byvalue);

                                if (TipoFuncion != "void")
                                {
                                    foreach (string o in Recolector.Recojedor)
                                    {
                                        if (o.StartsWith("retn|"))
                                            return new Devolucion(o.Remove(0, 5), TipoFuncion);
                                    }

                                }
                            }

                            if (TipoFuncion == "void")
                                return null;
                            else
                            {
                                estancia.NewAdvertencia($"La funcion '{nombre}' no tuvo ningun Devolucion", "00");
                                return null;
                            }


                        }
                    }



                }


            }


            // Metodos de las colecciones
            else if (data[0] == '$')
            {
                //Globales del metodo
                string name; //Nombre de la coleccion
                string tipo; //Tipo de la coleccion
                string restant; //Lo que resta despues 
                int i; //Indice de la coleccion en la base (Heap)
                {
                    name = data.Split('.')[0].ToLower().Remove(0, 1);
                    i = (int)Mvars.GetVarItem(name, accesos);

                    if (i > -1)
                    {
                        tipo = Base[i].Tipo;
                        if (IsValid.IsColeccionType(tipo) == false)
                        {
                            estancia.NewAdvertencia($"el elemento '{name}' no es una coleccion", "0");
                            return null;
                        }
                    }
                    else
                    {
                        estancia.NewAdvertencia($"No existe el elemento '{name}' en el contexto actual", "0");
                        return null;
                    }
                }


                switch (tipo)
                {
                    case "array":
                        {

                            // Obtiene un item de un array
                            if (data.Split('.')[1].ToLower().StartsWith("get"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "get.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;


                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), "int", Flex.Seguro, ByQ.Byvalue).Dato ?? "";


                                if (compilacion.IsNumeric())
                                    return new Devolucion(ReadList.ValidAndGet(int.Parse(compilacion)), "obj");
                                else
                                {
                                    estancia.NewAdvertencia($"'{compilacion}' no es index valido para la lista '{name}'", "00");
                                    return null;
                                }
                            }


                            // Obtiene un item de un array
                            else if (data.Split('.')[1].ToLower().StartsWith("add"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "add.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;


                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), "", Flex.Nulo, By).Dato ?? "";

                                ReadList.Add(compilacion);
                                estancia.OnDebug($"Se agrego un item a '{name}'");

                                return null;

                            }


                            // Obtiene la cantidad de items de una lista
                            else if (data.Split('.')[1].ToLower() == "len")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                return new Devolucion(ReadList.Count.ToString() ?? "0", "int");
                            }

                            // Obtiene la cantidad de items de una lista
                            else if (data.Split('.')[1].ToLower() == "type")
                            {
                                return new Devolucion("silf.array", "string");
                            }


                            // Elimina un item especifico de la lista
                            else if (data.Split('.')[1].ToLower().StartsWith("pop"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "pop.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;


                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), "int", Flex.Seguro, By).Dato ?? "";


                                if (compilacion.IsNumeric())
                                {
                                    ReadList.RemoveAt(int.Parse(compilacion));
                                    estancia.OnDebug($"Se elimino el item ubicado el el index '{compilacion}' de la lista '{name}'");
                                    return null;
                                }

                                else
                                {
                                    estancia.NewAdvertencia($"'{compilacion}' no es index valido para la lista '{name}'", "00");
                                    return null;
                                }
                            }


                            // Elimina los items de la lista
                            else if (data.Split('.')[1].ToLower() == "clear")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                estancia.OnDebug($"Se eliminaron todos los items de '{name}'");
                                ReadList.Clear();
                                return null;
                            }


                            // Reversa los datos de la lista
                            else if (data.Split('.')[1].ToLower() == "reverse")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                estancia.OnDebug($"Se reversaron todos los items de '{name}'");
                                ReadList.Reverse();
                                return null;
                            }


                            // Organiza los items de la lista 
                            else if (data.Split('.')[1].ToLower() == "sort")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                //Obtiene los numeros y las cadenas
                                List<string> strings = new();
                                List<double> doubles = new();


                                foreach (string value in ReadList)
                                {
                                    double xx;
                                    bool au = double.TryParse(value, out xx);

                                    if (au == true)
                                        doubles.Add(xx);
                                    else
                                        strings.Add(value);
                                }

                                //Sortea los datos
                                strings.Sort();
                                doubles.Sort();

                                //Elimina los datos antiguos
                                ReadList.Clear();

                                //Agrega los nuevos datos
                                ReadList.AddRange(doubles.Cast<string>());
                                ReadList.AddRange(strings);

                                estancia.OnDebug($"Se organizaron todos los items de '{name}'");
                                return null;
                            }


                            //Devuelve una copia de la lista lista actual
                            else if (data.Split('.')[1].ToLower() == "clone")
                            {

                                //Obtiene la lista
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                //Auxiliar de ID
                                List<ID> aux = new List<ID>();

                                //nueva lista
                                List<string> nuevaLista = new();
                                nuevaLista.AddRange(ReadList);

                                //Obtiene la ubicacion
                                double loc = Create.Array(Base, aux, "disp", nuevaLista);

                                return new Devolucion(loc.ToString(), "array");
                            }


                            // Elimina un item especifico de la lista
                            else if (data.Split('.')[1].ToLower().StartsWith("remove"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "remove.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;


                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), "", Flex.Nulo, By).Dato ?? "";


                                ReadList.Remove(compilacion);
                                estancia.OnDebug($"Se removieron valores de la lista '{name}'");
                                return null;

                            }


                            // Si no se encontro el metodo
                            else
                            {
                                estancia.NewAdvertencia($"El metodo '{data.Split('.')[1]}' no pertenece a Array", "");
                                return null;
                            }


                        }


                    case "list":
                        {

                            string GenericTipo = Base[i].TipoGenerico.ToLower().Trim();

                            // Obtiene un item de un array
                            if (data.Split('.')[1].ToLower().StartsWith("get"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "get.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;


                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), "int", Flex.Seguro, By).Dato ?? "";


                                if (compilacion.IsNumeric())
                                {


                                    if (IsValid.ForVal(GenericTipo))
                                    {

                                    }
                                    else
                                    {

                                    }

                                    if (By == ByQ.Byvalue)
                                    {
                                        var x = ReadList.ValidAndGet(int.Parse(compilacion));
                                        return new Devolucion("collection.item", GenericTipo);
                                    }
                                    else
                                    {
                                        var x = ReadList.ValidAndGet(int.Parse(compilacion));
                                        return new Devolucion(x, GenericTipo);
                                    }



                                    if (By == ByQ.ByRefer)
                                    {


                                    }
                                    else
                                    {
                                        var x = ReadList.ValidAndGet(int.Parse(compilacion));
                                        return new Devolucion(Base[x.ToInt32()].Valor, x, GenericTipo);
                                    }

                                }
                                else
                                {
                                    estancia.NewAdvertencia($"'{compilacion}' no es index valido para la lista '{name}'", "00");
                                    return null;
                                }
                            }


                            // Obtiene un item de un array
                            else if (data.Split('.')[1].ToLower().StartsWith("add"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "add.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                ByQ B;
                                if (IsValid.ForVal(GenericTipo))
                                    B = ByQ.Byvalue;
                                else
                                    B = ByQ.ByRefer;

                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), GenericTipo, Flex.Seguro, B).Dato ?? "";

                                ReadList.Add(compilacion);
                                estancia.OnDebug($"Se agrego un item a '{name}'");

                                return null;

                            }


                            // Obtiene la cantidad de items de una lista
                            else if (data.Split('.')[1].ToLower() == "len")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                return new Devolucion(ReadList.Count.ToString() ?? "0", "int");
                            }


                            // Elimina un item especifico de la lista
                            else if (data.Split('.')[1].ToLower().StartsWith("pop"))
                            {
                                // Obtiene los datos necesarios
                                restant = data.Split('.')[0] + "pop.";
                                restant = data.Remove(0, restant.Length);
                                List<string> ReadList;
                                ReadList = Base[i].Array;


                                //Interpretacion
                                string E1 = restant.ExtractFrom("(", ")") ?? "";
                                string compilacion = Interpretes.Micro(estancia, E1, accesos, Base, sett, new(), "int", Flex.Seguro, By).Dato ?? "";


                                if (compilacion.IsNumeric())
                                {
                                    ReadList.RemoveAt(int.Parse(compilacion));
                                    estancia.OnDebug($"Se elimino el item ubicado el el index '{compilacion}' de la lista '{name}'");
                                    return null;
                                }

                                else
                                {
                                    estancia.NewAdvertencia($"'{compilacion}' no es index valido para la lista '{name}'", "00");
                                    return null;
                                }
                            }

                            // Obtiene la cantidad de items de una lista
                            else if (data.Split('.')[1].ToLower() == "type")
                            {
                                return new Devolucion($"silf.collection.generic<{GenericTipo}>", "string");
                            }

                            // Elimina los items de la lista
                            else if (data.Split('.')[1].ToLower() == "clear")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                estancia.OnDebug($"Se eliminaron todos los items de '{name}'");
                                ReadList.Clear();
                                return null;
                            }


                            // Reversa los datos de la lista
                            else if (data.Split('.')[1].ToLower() == "reverse")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                estancia.OnDebug($"Se reversaron todos los items de '{name}'");
                                ReadList.Reverse();
                                return null;
                            }


                            // Organiza los items de la lista 
                            else if (data.Split('.')[1].ToLower() == "sort")
                            {
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                //Obtiene los numeros y las cadenas
                                List<string> strings = new();
                                List<double> doubles = new();
                                foreach (string value in ReadList)
                                {
                                    double xx;
                                    bool au = double.TryParse(value, out xx);

                                    if (au == true)
                                        doubles.Add(xx);
                                    else
                                        strings.Add(value);
                                }

                                //Sortea los datos
                                strings.Sort();
                                doubles.Sort();

                                //Elimina los datos antiguos
                                ReadList.Clear();

                                //Agrega los nuevos datos
                                ReadList.AddRange(doubles.Cast<string>());
                                ReadList.AddRange(strings);

                                estancia.OnDebug($"Se organizaron todos los items de '{name}'");
                                return null;
                            }


                            //Devuelve una copia de la lista lista actual
                            else if (data.Split('.')[1].ToLower() == "clone")
                            {

                                //Obtiene la lista
                                List<string> ReadList;
                                ReadList = Base[i].Array;

                                //Auxiliar de ID
                                List<ID> aux = new List<ID>();

                                //nueva lista
                                List<string> nuevaLista = new();
                                nuevaLista.AddRange(ReadList);

                                //Obtiene la ubicacion
                                double loc = Create.Array(Base, aux, "disp", nuevaLista);

                                return new Devolucion(loc.ToString(), "list");
                            }


                            // Si no se encontro el metodo
                            else
                            {
                                estancia.NewAdvertencia($"El metodo '{data.Split('.')[1]}' no pertenece a List", "");
                                return null;
                            }

                        }


                }

            }


            // Metodos de las variables y constantes
            else if (data[0] == '!')
            {
                //Globales del metodo  -----------
                string name; //Nombre del elemento
                string tipo; //Tipo del elemento
                string restant; //Lo que resta despues 
                int i; //Indice del elemento en la base (Heap)
                {
                    name = data.Split('.')[0].ToLower().Remove(0, 1);
                    i = (int)Mvars.GetVarItem(name, accesos);

                    if (i > -1)
                    {
                        tipo = Base[i].Tipo;
                        if (IsValid.ForVal(tipo) == false)
                        {
                            estancia.NewAdvertencia($"No es un tipo valido", "0");
                            return null;
                        }
                    }
                    else
                    {
                        estancia.NewAdvertencia($"No existe el elemento '{name}' en el contexto actual", "0");
                        return null;
                    }
                }


                switch (tipo)
                {
                    case "string":
                        {


                            // Trim
                            if (data.Split('.')[1].ToLower() == "trim")
                            {
                                return new(Base[i].Valor.Trim(), "string");
                            }

                            // Trim END
                            else if (data.Split('.')[1].ToLower() == "trimend")
                            {
                                return new(Base[i].Valor.TrimEnd(), "string");
                            }

                            // Trim Star
                            else if (data.Split('.')[1].ToLower() == "trimstart")
                            {
                                return new(Base[i].Valor.TrimStart(), "string");
                            }

                            // Lower
                            else if (data.Split('.')[1].ToLower() == "lower")
                            {
                                return new(Base[i].Valor.ToLower(), "string");
                            }

                            // Upper
                            else if (data.Split('.')[1].ToLower() == "upper")
                            {
                                return new(Base[i].Valor.ToUpper(), "string");
                            }

                            // Len
                            else if (data.Split('.')[1].ToLower() == "len")
                            {
                                return new(Base[i].Valor.Length.ToString(), "int");
                            }

                            // Value (Rebundancia al elemento en si)
                            else if (data.Split('.')[1].ToLower() == "value")
                            {
                                return new(Base[i].Valor, tipo);
                            }

                            // String
                            else if (data.Split('.')[1].ToLower() == "string")
                            {

                                var t = IsValid.Converted(Base[i].Valor, "string").Dato;

                                return new(t, tipo);
                            }


                            // Si no se encontro el metodo
                            else
                            {
                                estancia.NewAdvertencia($"El metodo '{data.Split('.')[1]}' no pertenece a {tipo}", "");
                                return null;
                            }


                        }


                    case "int":
                        {
                            // Trim
                            if (data.Split('.')[1].ToLower() == "string")
                            {
                                return new(Base[i].Valor.Trim(), "string");
                            }

                            // Value (Rebundancia al elemento en si)
                            else if (data.Split('.')[1].ToLower() == "value")
                            {
                                return new(Base[i].Valor, tipo);
                            }


                            // Si no se encontro el metodo
                            else
                            {
                                estancia.NewAdvertencia($"El metodo '{data.Split('.')[1]}' no pertenece a {tipo}", "");
                                return null;
                            }


                        }


                    case "float":
                        {
                            // Trim
                            if (data.Split('.')[1].ToLower() == "string")
                            {
                                return new(Base[i].Valor.Trim(), "string");
                            }

                            // Value (Rebundancia al elemento en si)
                            else if (data.Split('.')[1].ToLower() == "value")
                            {
                                return new(Base[i].Valor, tipo);
                            }

                            // Si no se encontro el metodo
                            else
                            {
                                estancia.NewAdvertencia($"El metodo '{data.Split('.')[1]}' no pertenece a {tipo}", "");
                                return null;
                            }


                        }


                    case "bool":
                        {
                            // Trim
                            if (data.Split('.')[1].ToLower() == "string")
                            {
                                return new(Base[i].Valor.Trim(), "string");
                            }

                            // Value (Rebundancia al elemento en si)
                            else if (data.Split('.')[1].ToLower() == "value")
                            {
                                return new(Base[i].Valor, tipo);
                            }

                            // Si no se encontro el metodo
                            else
                            {
                                estancia.NewAdvertencia($"El metodo '{data.Split('.')[1]}' no pertenece a {tipo}", "");
                                return null;
                            }


                        }

                }



            }


            // Valores logicos (True) (False)
            else if (data.ToLower() == "false" | data.ToLower() == "true")
            {
                if (data.ToLower() == "false")
                    return new Devolucion(IsValid.ValorFalse, "bool");
                else if (data.ToLower() == "true")
                    return new Devolucion(IsValid.ValorTrue, "bool");
            }


            // Ejecutador de bloques
            else if (data[0] == '(' & data.EndsWith(")") & modo == 1)
            {
                string Aux1 = data.ExtractFrom("(", ")");
                var t = Interpretes.MicroForGetItems(estancia, Aux1, accesos, Base, sett, tipoActual, Flex, By);
                return new Devolucion(t);
            }


            // Ejecutador de Listas
            else if (data[0] == '[' & data.EndsWith("]") & modo == 1)
            {

                // Obtiene la lista
                string Aux1 = data.ExtractFrom("[", "]");

                // Lista generada
                List<string> Lista = new();

                Interpretes.MicroForGenerics(estancia, Aux1, accesos, Base, sett, Lista, nececidad.tipo);

                var id = Create.CollGeneric(Base, new(), "a", nececidad.tipo, Lista);

                return new Devolucion($"SILF.Collections.{nececidad.clase}", id.ToString(), nececidad.clase);
            }


            // Ejecutador de Objetos
            else if (data[0] == '{' & data.EndsWith("}") & modo == 1)
            {

                // Obtiene el valor a compilar
                string Aux1 = data.ExtractFrom("{", "}");




            }


            //Sentencia Return de una funcion
            else if (data.ToLower().StartsWith("return ") & modo == 0 & sett.NeedReturns == true)
            {
                data = data.Remove(0, 6);
                data = data.Trim();
                var FG2 = Interpretes.Micro(estancia, data, accesos, Base, sett, new(), sett.FuntionType, Flex.FlexibleField, By);
                sett.Recojedor.Insert(0, "retn|" + FG2.Dato);
                return null;
            }



            #endregion



            //Devuelve la cadena de string
            else if (data[0] == '"' & data.EndsWith("\"") & modo == 1)
            {
                data = data.ExtractFrom("\"", "\"") ?? "";
                return new Devolucion(data, "string");
            }


            //Develve una cadena interpolada
            else if (data.StartsWith("#\"") & data.EndsWith("\"") & modo == 1)
            {

                string aux = data.ExtractFrom("\"", "\"") ?? "";
                StringBuilder Final = new StringBuilder();
                //Obtiene los datos
                {
                    var BD1 = false; // Si esta recojiendo datos a interpretar
                    var cr = 0;
                    var Col = "";
                    foreach (char d in aux)
                    {
                        if (d == '{')
                        {
                            BD1 = true;
                            cr += 1;
                            continue;
                        }
                        else if (d == '}')
                        {
                            BD1 = false;
                            cr -= 1;
                            if (cr == 0)
                            {
                                Col = Col.Trim();
                                var f = Interpretes.Micro(estancia, Col, accesos, Base, sett, new(), "string", Flex.FlexibleField, ByQ.Byvalue);
                                Final.Append(f.Dato);
                                Col = "";
                            }
                            continue;
                        }

                        if (BD1 == false)
                            Final.Append(d);
                        else
                            Col += d;
                    }

                    return new Devolucion(Final.ToString(), "string");
                }

            }


            //Exprecion Exit For
            else if (data.ToLower().Replace(" ", "") == "exitfor")
            {
                sett.Recojedor.Add("exit-for");
                return null;
            }


            //Si es un numero
            else if (data.IsNumeric() & modo == 1)
            {
                if (data.Contains(".") | data.Contains(","))
                    return new Devolucion(data, "float");
                else
                    return new Devolucion(data, "int");
            }




            //Si no encontro el elemento
            else
            {
                List<string> t = data.Sepa();

                //Operador ==
                if (t[1] == "==")
                {
                    var a = Interpretes.Micro(estancia, t[0], accesos, Base, sett, new(), "", Flex.Nulo, ByQ.Byvalue);
                    var b = Interpretes.Micro(estancia, t[2], accesos, Base, sett, new(), "", Flex.Nulo, ByQ.Byvalue);


                    if (a.Dato == b.Dato)
                    {
                        return new Devolucion(IsValid.ValorTrue, "bool");
                    }
                    else
                    {
                        return new Devolucion(IsValid.ValorFalse, "bool");
                    }


                }

                //Operador !=
                else if (t[1] == "!=")
                {
                    var a = Interpretes.Micro(estancia, t[0], accesos, Base, sett, new(), "", Flex.Nulo, ByQ.Byvalue); ;
                    var b = Interpretes.Micro(estancia, t[2], accesos, Base, sett, new(), "", Flex.Nulo, ByQ.Byvalue);


                    if (a.Dato != b.Dato)
                    {
                        return new Devolucion(IsValid.ValorTrue, "bool");
                    }
                    else
                    {
                        return new Devolucion(IsValid.ValorFalse, "bool");
                    }


                }


                //Funcion
                else if (data.Contains("(") & data.Contains(")"))
                {
                    string hk = "-" + data;
                    string h = hk.ExtractFrom2("-", "(") ?? "";
                    bool aux = Exist.Funcion(estancia, h);

                    if (aux == true)
                        return Interprete(estancia, "¡" + data, modo, accesos, Base, sett, new(), tipoActual, Flex, By);

                }
                estancia.NewAdvertencia($"No se encontro la funcion ({data ?? " "}) en el contexto actual ", "0");
                return null;
            }



            return null;

        }

    }
}
#pragma warning restore
namespace SILF.Script
{
    static class InterVars
    {


        public static void Variables(Instance estancia, string data, List<ID> accesos, List<Objeto> Base)
        {

            string nombre;
            string tipo;
            string valor;


            // Nombre
            bool NeedType = true;
            {
                // Encuentra cual es primero
                var aux1 = data.FirsIndex(':', '=');
                char? Crt = aux1.caracter;
                int index = aux1.index;

                // Segun el primer caracter encontrado
                switch (Crt)
                {
                    // Si el primero es el igual
                    case '=':
                        {
                            nombre = data.Substring(0, index);
                            data = data.Remove(0, nombre.Length + 1);
                            NeedType = false;
                            break;
                        }

                    // Si el primero es el de tipo
                    case ':':
                        {
                            nombre = data.Substring(0, index);
                            data = data.Remove(0, nombre.Length + 1);
                            break;
                        }

                    //Si no fue nunguno
                    default:
                        {
                            estancia.NewAdvertencia("Una variable debe tener un tipo definido", "");
                            return;
                        }
                }

            }

            // Tipo
            bool NeedInitial = true;
            {

                if (NeedType == true)
                {
                    var aux1 = data.FirsIndex('=', '=');
                    char? Crt = aux1.caracter;
                    int index = aux1.index;

                    // Segun el primer caracter encontrado
                    switch (Crt)
                    {

                        // Si el primero es el de tipo
                        case '=':
                            {
                                tipo = data.Substring(0, index);
                                data = data.Remove(0, tipo.Length + 1);
                                break;
                            }

                        //Si no fue nunguno
                        default:
                            {
                                tipo = data;
                                NeedInitial = false;
                                break;
                            }
                    }

                }

                else
                {
                    tipo = "?";
                }
            }


            // Dato
            {
                if (NeedInitial == true)
                    valor = data;
                else
                    valor = "";

            }


            // Comprobaciones
            {
                nombre = nombre.Trim().ToLower();
                tipo = tipo.Trim().ToLower();
                valor = valor.Trim();

                // Nombre
                if (nombre == "")
                {
                    estancia.NewAdvertencia("Una variable debe de tener un nombre valido", "");
                    return;
                }

                // Tipo
                if (tipo == "" & NeedType == true)
                {
                    estancia.NewAdvertencia("Una variable debe de tener un tipo valido", "");
                    return;
                }

                // Valor
                if (valor == "" & NeedInitial == true)
                {
                    estancia.NewAdvertencia("Una variable debe de tener un valor de inicio valido", "");
                    return;
                }

            }


            //Invalid chars
            {
                var ty = IsValid.Name(nombre);


                // Si es valido
                if (ty == IsValid.Er.Si)
                {
                }

                // Si inicia con un caracter
                else if (ty == IsValid.Er.StarInvalidChar)
                {
                    estancia.NewAdvertencia("Una varible no debe iniciar con un simbolo", "");
                    return;
                }

                // Si contiene un caracter
                else if (ty == IsValid.Er.InvalidChar)
                {
                    estancia.NewAdvertencia("Una varible no debe contener simbolos", "");
                    return;
                }

                // Si inicia con numero
                else if (ty == IsValid.Er.IniciaConNumero)
                {
                    estancia.NewAdvertencia("Una varible no debe iniciar con un numero", "");
                    return;
                }



            }


            //Comprueba si ya existe la variable nueva
            if (Exist.Variable(nombre, accesos))
            {
                estancia.OnDebug($"Ya existe un elemento con el nombre '{nombre}'");
                return;
            }


        reset:


            // Variable 
            if (tipo == "string" | tipo == "int" | tipo == "float" | tipo == "bool")
            {
                // Interpreta el valor
                valor = Interpretes.Micro(estancia, valor, accesos, Base, new(false, ""), new(), tipo, Flex.FlexibleField, ByQ.Byvalue).Dato ?? "";

                // Crea el elemento
                Create.Elemento(Base, accesos, nombre, tipo, Seguridad.var, valor);

                // Escribe en el debug
                estancia.OnDebug($"Se creo la variable tipo<{tipo}> con el nombre '{nombre}'");
                return;
            }


            // Si es dinamico y tiene { } (Objeto anonimo)
            else if (tipo == "?" & valor.StartsWith("{") & valor.EndsWith("}"))
            {
                tipo = "element";
                goto reset;
            }


            // Si es dinamico y tiene [ ] (array)
            else if (tipo == "?" & valor.StartsWith("[") & valor.EndsWith("]"))
            {
                tipo = "array";
                goto reset;
            }


            // Si es dinamico Generico
            else if (tipo == "?" | tipo == "dynamic")
            {
                // Interpreta el valor
                valor = Interpretes.Micro(estancia, valor, accesos, Base, new(false, ""), new(), "obj", Flex.Nulo, ByQ.Byvalue).Dato ?? "";

                // Crea el elemento
                Create.Elemento(Base, accesos, nombre, "dynamic", Seguridad.var, valor, true);

                // Escribe en el debug
                estancia.OnDebug($"Se creo la variable tipo Dynamic con el nombre '{nombre}'");
                return;
            }


            // Si es un array
            else if (tipo == "array")
            {

                // Valores inesesarios
                if (valor.StartsWith("[") & valor.EndsWith("]"))
                {
                    valor = valor.Remove(0, 1);
                    valor = valor.Reverse();
                    valor = valor.Remove(0, 1);
                    valor = valor.Reverse();
                }
                else
                    valor = "";


                List<string> ListaA = new();
                // Interpreta el valor
                Interpretes.MicroForArray(estancia, valor, accesos, Base, new(false, ""), ListaA);

                // Crea el elemento
                Create.Array(Base, accesos, nombre, ListaA);

                // Escribe en el debug
                estancia.OnDebug($"Se creo la variable tipo <{tipo}> con el nombre '{nombre}'");
                return;
            }


            // Si es un array
            else if (tipo.StartsWith("array<") | tipo.StartsWith("list<"))
            {

                string generic = tipo?.ExtractFrom("<", ">", null)?.ToLower() ?? "obj";

                if (valor.StartsWith("[") & valor.EndsWith("]"))
                {
                    valor = valor.Remove(0, 1);
                    valor = valor.Reverse();
                    valor = valor.Remove(0, 1);
                    valor = valor.Reverse();
                }
                else
                    valor = "";


                if (generic == "")
                {
                    estancia.NewAdvertencia("El tipo generico de una no debe estar vacio, por defecto sera Obj", "");
                    generic = "obj";
                }


                List<string> ListaA = new();
                // Interpreta el valor
                Interpretes.MicroForGenerics(estancia, valor, accesos, Base, new(false, ""), ListaA, generic);

                // Crea el elemento
                Create.CollGeneric(Base, accesos, nombre, generic, ListaA);

                // Escribe en el debug
                estancia.OnDebug($"Se creo la lista generica tipo <{generic}> con el nombre '{nombre}'");
                return;
            }


            // Si es un Objeto
            else if (tipo == "element")
            {


                if (valor.StartsWith("{") == false & valor.EndsWith("}") == false)
                {
                    estancia.NewAdvertencia("Un objeto anonimo <Element> debe tener propiedades definidas", "");
                    return;
                }

                List<string> ListaA = new();
                // Interpreta el valor
                // Micro for Objets (Pendiente)

                // Crea el elemento
                // Crear el objeto pendiente

                // Escribe en el debug
                estancia.OnDebug($"Se creo el objeto anonimo con el nombre '{nombre}'");
                return;
            }




        }


        public static void Constantes(Instance estancia, string data, List<ID> accesos, List<Objeto> Base)
        {

            string nombre;
            string tipo;
            string valor;


            // Nombre
            const bool NeedType = true;
            {
                // Encuentra cual es primero
                var aux1 = data.FirsIndex(':', '=');
                char? Crt = aux1.caracter;
                int index = aux1.index;

                // Segun el primer caracter encontrado
                switch (Crt)
                {
                    // Si el primero es el igual
                    case '=':
                        {
                            estancia.NewAdvertencia("Una contante debe tener un tipo definido", "");
                            return;
                        }

                    // Si el primero es el de tipo
                    case ':':
                        {
                            nombre = data.Substring(0, index);
                            data = data.Remove(0, nombre.Length + 1);
                            break;
                        }

                    //Si no fue nunguno
                    default:
                        {
                            estancia.NewAdvertencia("Una contante debe tener un tipo definido", "");
                            return;
                        }
                }

            }

            // Tipo
            const bool NeedInitial = true;
            {

                if (NeedType == true)
                {
                    var aux1 = data.FirsIndex('=', '=');
                    char? Crt = aux1.caracter;
                    int index = aux1.index;

                    // Segun el primer caracter encontrado
                    switch (Crt)
                    {

                        // Si el primero es el de tipo
                        case '=':
                            {
                                tipo = data.Substring(0, index);
                                data = data.Remove(0, tipo.Length + 1);
                                break;
                            }

                        //Si no fue nunguno
                        default:
                            {
                                estancia.NewAdvertencia("Una contante debe tener un valor constante", "");
                                return;
                            }
                    }

                }

                else
                {
                    tipo = "?";
                }
            }


            // Dato
            {
                valor = data;
            }


            // Comprobaciones
            {
                nombre = nombre.Trim().ToLower();
                tipo = tipo.Trim().ToLower();
                valor = valor.Trim();

                // Nombre
                if (nombre == "")
                {
                    estancia.NewAdvertencia("Una constante debe de tener un nombre valido", "");
                    return;
                }

                // Tipo
                if (tipo == "" & NeedType == true)
                {
                    estancia.NewAdvertencia("Una constante debe de tener un tipo valido", "");
                    return;
                }

                // Valor
                if (valor == "" & NeedInitial == true)
                {
                    estancia.NewAdvertencia("Una constante debe de tener un valor de inicio valido", "");
                    return;
                }

            }


            //Invalid chars
            {
                var ty = IsValid.Name(nombre);


                // Si es valido
                if (ty == IsValid.Er.Si)
                {
                }

                // Si inicia con un caracter
                else if (ty == IsValid.Er.StarInvalidChar)
                {
                    estancia.NewAdvertencia("Una constante no debe iniciar con un simbolo", "");
                    return;
                }

                // Si contiene un caracter
                else if (ty == IsValid.Er.InvalidChar)
                {
                    estancia.NewAdvertencia("Una constante no debe contener simbolos", "");
                    return;
                }

                // Si inicia con numero
                else if (ty == IsValid.Er.IniciaConNumero)
                {
                    estancia.NewAdvertencia("Una constante no debe iniciar con un numero", "");
                    return;
                }



            }


            //Comprueba si ya existe la variable nueva
            if (Exist.Variable(nombre, accesos))
            {
                estancia.OnDebug($"Ya existe un elemento con el nombre '{nombre}'");
                return;
            }

            // Variable 
            if (tipo == "string" | tipo == "int" | tipo == "float" | tipo == "bool")
            {
                // Interpreta el valor
                valor = Interpretes.Micro(estancia, valor, accesos, Base, new(false, ""), new(), tipo, Flex.FlexibleField, ByQ.Byvalue).Dato ?? "";

                // Crea el elemento
                Create.Elemento(Base, accesos, nombre, tipo, Seguridad.constant, valor);

                // Escribe en el debug
                estancia.OnDebug($"Se creo la constante tipo<{tipo}> con el nombre '{nombre}'");
                return;
            }

            // Si es un array
            else if (tipo.StartsWith("set<"))
            {

                string generic = tipo?.ExtractFrom("<", ">", null)?.ToLower() ?? "obj";

                if (valor.StartsWith("[") & valor.EndsWith("]"))
                {
                    valor = valor.Remove(0, 1);
                    valor = valor.Reverse();
                    valor = valor.Remove(0, 1);
                    valor = valor.Reverse();
                }
                else
                    valor = "";


                if (generic == "")
                {
                    estancia.NewAdvertencia("El tipo generico de una no debe estar vacio, por defecto sera Obj", "");
                    generic = "obj";
                }


                List<string> ListaA = new();
                // Interpreta el valor
                Interpretes.MicroForGenerics(estancia, valor, accesos, Base, new(false, ""), ListaA, generic);

                // Crea el elemento
                Create.CollGeneric(Base, accesos, nombre, generic, ListaA);

                // Escribe en el debug
                estancia.OnDebug($"Se creo la lista generica inmutable tipo <{generic}> con el nombre '{nombre}'");
                return;
            }







        }


    }
}

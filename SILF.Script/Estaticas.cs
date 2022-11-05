namespace SILF.Script
{


    public static class Estaticas
    {

        public static List<Error> Comprobar(RichTextBox richTextBox1)
        {

            List<Error> Total = new();

            // Variables
            Total.AddRange(variables(richTextBox1));


            return Total;

        }


        private static List<Error> variables(RichTextBox richTextBox1)
        {

            List<Error> Total = new();

            // Lista de variables
            List<string> a = new();

            // Nuevo rich
            var code = new RichTextBox();

            // Trimea las lineas
            foreach (var t in richTextBox1.Lines)
                code.Text += t.Trim() + Environment.NewLine;


            // Recorre las lineas
            foreach (var t in code.Lines)
            {

                // Si es una variable
                if (t.StartsWith("let "))
                {
                    var res = Vars(t);

                    if (res.Sintax == false)
                    {
                        var one = new Error();
                        one.titulo = "Invalido";
                        one.error = res.SintaxError;
                        Total.Add(one);
                    }
                    else
                    {

                        if (res.isAdvertency == true)
                        {
                            var one = new Error();
                            one.titulo = "Advertencia: ";
                            one.error = res.advertency;
                            Total.Add(one);
                        }

                        if (a.Contains(res.name.ToLower()))
                        {
                            var one = new Error();
                            one.titulo = "Nombre Invalido";
                            one.error = $"Ya existe una variable con el nombre '{res.name}'";
                            Total.Add(one);
                        }
                        else
                        {
                            a.Add(res.name.ToLower());
                        }
                    }


                }


            }

            return Total;

        }




        public static ResultOfStatics Vars(string data)
        {
            data = data.Remove(0, 3).Trim();

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
                            ResultOfStatics res = new();
                            res.SintaxError = "Una variable debe tener un tipo definido";
                            res.Sintax = false;
                            return res;
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


            // Obtiene el dato
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
                if (nombre == "" | IsValid.Name(nombre) != IsValid.Er.Si)
                {
                    ResultOfStatics res = new();
                    res.SintaxError = "Una variable debe tener un nombre valido";
                    res.Sintax = false;
                    return res;
                }

                // Tipo
                if (tipo == "" & NeedType == true)
                {
                    ResultOfStatics res = new();
                    res.SintaxError = "Una variable debe tener un tipo valido";
                    res.Sintax = false;
                    return res;
                }

                // Valor
                if (valor == "" & NeedInitial == true)
                {
                    ResultOfStatics res = new();
                    res.SintaxError = "Una variable debe tener un valor de inicio valido";
                    res.Sintax = false;
                    return res;
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
                    ResultOfStatics res = new();
                    res.Sintax = false;
                    res.SintaxError = "Un campo no debe iniciar con un caracter especial";
                    return res;
                }

                // Si contiene un caracter
                else if (ty == IsValid.Er.InvalidChar)
                {
                    ResultOfStatics res = new();
                    res.Sintax = false;
                    res.SintaxError = "Un campo no debe contener con un caracter especial o espacios";
                    return res;
                }

                // Si inicia con numero
                else if (ty == IsValid.Er.IniciaConNumero)
                {
                    ResultOfStatics res = new();
                    res.Sintax = false;
                    res.SintaxError = "Un campo no debe iniciar con un numero";
                    return res;
                }



            }




        reset:


            // Variable 
            if (tipo == "string" | tipo == "int" | tipo == "float" | tipo == "bool")
            {
                ResultOfStatics res = new();
                res.contenido = valor;
                res.name = nombre;
                res.tipo = tipo;
                res.Sintax = true;
                return res;
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
                ResultOfStatics res = new();
                res.contenido = valor;
                res.name = nombre;
                res.tipo = "obj";
                res.Sintax = true;
                return res;
            }


            // Si es un array
            else if (tipo == "array")
            {
                ResultOfStatics res = new();
                res.contenido = valor;
                res.name = nombre;
                res.tipo = tipo;
                res.Sintax = true;
                res.isAdvertency = true;
                res.advertency = "<Array> es obsoleto en su lugar use 'List<obj>'";
                return res;
            }


            // Si es un array
            else if (tipo.StartsWith("array<") | tipo.StartsWith("list<"))
            {
                ResultOfStatics res = new();
                res.contenido = valor;
                res.name = nombre;
                res.tipo = tipo;
                res.Sintax = true;
                return res;
            }


            // Si es un Objeto
            else if (tipo == "element")
            {


                if (valor.StartsWith("{") == false & valor.EndsWith("}") == false)
                {
                    ResultOfStatics res = new();
                    res.name = nombre;
                    res.tipo = tipo;
                    res.Sintax = false;
                    res.SintaxError = "un objeto anonimo debe tener propiedades definidas";
                    return res;
                }

                {
                    ResultOfStatics res = new();
                    res.contenido = valor;
                    res.name = nombre;
                    res.tipo = tipo;
                    res.Sintax = true;
                    return res;
                }

            }


            else
            {
                ResultOfStatics res = new();
                res.contenido = valor;
                res.name = nombre;
                res.tipo = tipo;
                res.Sintax = true;
                return res;
            }


        }
    }






    public class Error
    {
        public string titulo = "";
        public string error = "";
    }


    public class ResultOfStatics
    {
        public string name = "";
        public string tipo = "";
        public string contenido = "";
        public bool Sintax = true;
        public string SintaxError = "";
        public bool isAdvertency = false;
        public string advertency = "";

    }


}

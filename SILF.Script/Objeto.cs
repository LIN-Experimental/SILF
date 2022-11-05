namespace SILF.Script
{

    /// <summary>
    /// Enum de Seguridad
    /// </summary>
    public enum Seguridad
    {
        /// <summary>
        /// Si es variable
        /// </summary>
        var = 0,
        /// <summary>
        /// Si es constante
        /// </summary>
        constant = 1,
        /// <summary>
        /// Si es interna
        /// </summary>
        interna = 2,
    }




    /// <summary>
    /// Elemento base de SILF.Core
    /// </summary>
    public class Objeto : IBaseObj
    {

        private protected string _tipo;
        private protected string _tipogenerico;
        private protected string _valor;
        private protected Seguridad _seguridad;
        private protected bool _isDynamic;

        public List<string> Array = new();




        /// <summary>
        /// Crea un nuevo objeto
        /// Seguridad es variable (Var) por defecto
        /// Dynamic es false por defecto
        /// </summary>
        /// <param name="Valor">Valor del objeto</param>
        /// <param name="Tipo">Tipo del objeto</param>
        public Objeto(string Valor, string Tipo)
        {
            this.Tipo = Tipo;
            TipoGenerico = "";
            this.Valor = Valor;
            Seguridad = Seguridad.var;
            IsDynamic = false;
        }




        /// <summary>
        /// Crea un nuevo objeto
        /// </summary>
        /// <param name="Valor">Valor del objeto</param>
        /// <param name="Tipo">Tipo del objeto</param>
        /// <param name="IsDynamic">Si la variable es dinamica o no</param>
        public Objeto(string Valor, string Tipo, bool IsDynamic)
        {
            this.Tipo = Tipo;
            TipoGenerico = "";
            this.Valor = Valor;
            Seguridad = Seguridad.var;
            this.IsDynamic = IsDynamic;
        }




        /// <summary>
        /// Crea un nuevo objeto
        /// </summary>
        /// <param name="Valor">Valor del objeto</param>
        /// <param name="Tipo">Tipo del objeto</param>
        /// <param name="IsDynamic">Si la variable es dinamica o no</param>
        /// <param name="seguridad">Seguridad del objeto</param>
        public Objeto(string Valor, string Tipo, bool IsDynamic, Seguridad seguridad)
        {
            this.Tipo = Tipo;
            TipoGenerico = "";
            this.Valor = Valor;
            Seguridad = seguridad;
            this.IsDynamic = IsDynamic;
        }




        /// <summary>
        /// Crea una nueva lista o stack generico
        /// </summary>
        /// <param name="Value">Lista nueva</param>
        /// <param name="TipoGen">Tipo generico de la lista</param>
        /// <param name="Tipo">Tipo de lista, por lo generial List o Stak</param>
        public Objeto(List<string> Value, string TipoGen, string Tipo)
        {
            this.Tipo = Tipo;
            TipoGenerico = TipoGen;
            Valor = "";
            Seguridad = Seguridad.var;
            IsDynamic = false;
            Array = Value;
        }




        /// <summary>
        /// Crea una nueva Coleccion
        /// </summary>
        /// <param name="Value">Lista nueva</param>
        /// <param name="Tipo">Tipo de lista, por lo generial Array o Tuple</param>
        public Objeto(List<string> Value, string Tipo)
        {
            this.Tipo = Tipo;
            TipoGenerico = "";
            Valor = "";
            Seguridad = Seguridad.var;
            IsDynamic = false;
            Array = Value;
        }





        /// <summary>
        /// Obtiene o establece el tipo
        /// </summary>
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value.ToLower().Trim(); }
        }



        /// <summary>
        /// Obtiene o establece el valor
        /// </summary>
        public string Valor
        {
            get
            {
                if (Tipo == "array")
                    return "silf.collections.array";

                if (Tipo == "list")
                    return "silf.collections.list";

                if (Tipo == "tuple")
                    return "silf.collections.tuple";

                if (Tipo == "stack")
                    return "silf.collections.stack";

                return _valor;

            }
            set => _valor = value;
        }


        /// <summary>
        /// Obtiene o establece la seguridad del objeto
        /// </summary>
        public Seguridad Seguridad
        {
            get { return _seguridad; }
            set { _seguridad = value; }
        }


        /// <summary>
        /// Obtiene o establece si la variable es dinamica
        /// </summary>
        public bool IsDynamic
        {
            get { return _isDynamic; }
            set { _isDynamic = value; }
        }


        /// <summary>
        /// Obtiene o establece el tipo generico de un objeto Lista o Stck
        /// </summary>
        public string TipoGenerico
        {
            get { return _tipogenerico; }
            set { _tipogenerico = value.ToLower().Trim(); }
        }

    }


}
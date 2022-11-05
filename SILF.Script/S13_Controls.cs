namespace SILF.Script
{


    public class Devolucion
    {

        private int _referencia = 0;
        public BloqueList Lista;

        private protected string _Dato;
        private protected string _Tipo;
        private protected bool _CointainBlocks;



        /// <summary>
        /// Genera un nuevo retorno
        /// </summary>
        /// <param name="Dato">Dato que se quiere entregar</param>
        /// <param name="Tipo">Tipo que se quiere entregar</param>
        public Devolucion(string Dato, string Tipo)
        {
            Lista = new();
            this.Dato = Dato;
            this.Tipo = Tipo;
            ContainsBlocks = false;
            referencia = "0";
        }


        public Devolucion(string Dato, string referencia, string Tipo)
        {
            Lista = new();
            this.Dato = Dato;
            this.Tipo = Tipo;
            ContainsBlocks = false;
            this.referencia = referencia;

        }



        /// <summary>
        /// Genera un nuevo retorno con una lista de bloques compilados dentro
        /// </summary>
        /// <param name="ListaContain"></param>
        public Devolucion(BloqueList ListaContain)
        {
            Lista = ListaContain;
            Dato = "";
            Tipo = "";
            ContainsBlocks = true;
            referencia = "0";
        }




        /// <summary>
        /// Obtiene o establece el Dato (Valor)
        /// </summary>
        public string Dato
        {
            get { return _Dato; }
            set { _Dato = value ?? ""; }
        }


        /// <summary>
        /// Obtiene o establece el Tipo
        /// </summary>
        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value.ToLower().Trim() ?? ""; }
        }


        /// <summary>
        /// Obtiene o establce si contine bloques
        /// </summary>
        public bool ContainsBlocks
        {
            get { return _CointainBlocks; }
            set { _CointainBlocks = value; }
        }


        public string referencia
        {
            set
            {
                int _s = 0;
                bool ss = int.TryParse(value, out _s);

                if (_s >= 0)
                    _referencia = _s;
                else
                    _referencia = 0;

            }

            get
            {
                return _referencia.ToString();
            }
        }


    }


}

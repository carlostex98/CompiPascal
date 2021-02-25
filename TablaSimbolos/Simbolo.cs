using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.TablaSimbolos
{
    public class Simbolo
    {
        public enum Tipo
        {
            STRING,
            BOOLEAN,
            ARRAY, 
            TYPE
        }

        private Tipo tipo;
        private String id;
        private Object valor;


        public Simbolo(String id, Tipo tipo)
        {
            this.tipo = tipo;
            this.id = id;
        }
        public String getId()
        {
            return id;
        }

        public Object getValor()
        {
            return valor;
        }

        public void setValor(Object v)
        {
            valor = v;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;


namespace CompiPascal.TablaSimbolos
{
    public class Simbolo
    {
        
        public enum tipo{ 
            STRING,
            INTEGER,
            REAL,
            BOOLEAN
        };


        private string id;
        public Primitivo valor;
        private tipo Tipo;


        public Simbolo(String id, tipo t, Primitivo v)
        {
            this.valor = v;
            this.id = id;
            this.Tipo = t;
        }
        public String getId()
        {
            return id;
        }

        public Primitivo getValor()
        {
            return valor;
        }

        public void setValor(Primitivo v)
        {
            valor = v;
        }

        public tipo GetTipo()
        {
            return this.Tipo;
        }

    }
}

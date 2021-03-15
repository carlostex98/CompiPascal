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


        public string ide;
        public Primitivo valor;
        public tipo Tipo;


        public Simbolo(String idx, tipo t, Primitivo v)
        {
            this.valor = v;
            this.ide = idx;
            this.Tipo = t;
        }
        public String getId()
        {
            return ide;
        }

        public Primitivo getValor()
        {
            return valor;
        }

        public void setValor(Primitivo v)
        {
            this.valor = v;
        }

        public tipo GetTipo()
        {
            return this.Tipo;
        }

    }
}

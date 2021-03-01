using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;


namespace CompiPascal.TablaSimbolos
{
    public class Simbolo
    {
        
        private String id;
        private Primitivo valor;


        public Simbolo(String id, Primitivo v)
        {
            this.valor = v;
            this.id = id;
        }
        public String getId()
        {
            return id;
        }

        public Primitivo getValor()
        {
            return valor;
        }

        public void setValor(Object v)
        {
            //valor = v;
        }

    }
}

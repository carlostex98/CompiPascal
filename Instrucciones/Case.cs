using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.Instrucciones
{
    public class Case
    {

        //este solo es un tipo de dato
        private LinkedList<Instruccion> lista_ins;
        private Operacion derecho;

        public Case(Operacion d, LinkedList<Instruccion> lst)
        {
            this.lista_ins = lst;
            this.derecho = d;
        }

        public LinkedList<Instruccion> getInstrucciones()
        {
            return this.lista_ins;
        }


        public Operacion getOperacion()
        {
            return this.derecho;
        }


    }
}

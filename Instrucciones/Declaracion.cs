using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class Declaracion: Instruccion
    {

        private string nombre;
        private Primitivo valor;
        private Operacion opr;
        private Simbolo.tipo tip;
        private LinkedList<string> nombres = new LinkedList<string>();

        public Declaracion(string a, Simbolo.tipo t , Operacion op)
        {
            this.nombre = a;
            this.opr = op;
            this.tip = t;
            this.nombres = null;
        }


        public Declaracion(LinkedList<string> b, Simbolo.tipo t, Operacion op)
        {
            //this.nombre = a;
            this.opr = op;
            this.tip = t;
            this.nombres = b;
        }


        public Object ejecutar(TSimbolo ts)
        {

            if (opr == null)
            {
                //valor nulo
                //this.valor = (Primitivo)this.opr.ejecutar(ts);

                foreach (string t in nombres)
                {
                    ts.agregar(t, (Simbolo)null);
                }
                
                return null;
            }

            this.valor = (Primitivo)this.opr.ejecutar(ts);
            Simbolo e = new Simbolo(nombre, tip, valor);
            foreach (string t in nombres)
            {
                ts.agregar(t, e);
            }
            return null;
        }
    }
}

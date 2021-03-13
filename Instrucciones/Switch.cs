using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class Switch:Instruccion
    {
        private LinkedList<Case> casos;
        private LinkedList<Instruccion> instr_else;
        private Operacion izquierdo;
        private int linea;
        private int columna;

        public Switch(Operacion i, LinkedList<Case> cs, LinkedList<Instruccion> ins, int ln, int cl)
        {
            this.izquierdo = i;
            this.casos = cs;
            this.instr_else = ins;
            this.linea = ln;
            this.columna = cl;
        }


        public Switch(Operacion i, LinkedList<Case> cs, int ln, int cl)
        {
            this.izquierdo = i;
            this.casos = cs;
            this.instr_else = null;
            this.linea = ln;
            this.columna = cl;
        }


        public Object ejecutar(TSimbolo ts)
        {
            Primitivo d = (Primitivo)izquierdo.ejecutar(ts);
            TSimbolo local = new TSimbolo(ts);
            
            foreach(Case t in casos)
            {
                //recorremos los casos

                Primitivo i = (Primitivo)t.getOperacion().ejecutar(local);
                if (i.valor.Equals(d.valor))
                {
                    //ejecuta el coso del caso
                    foreach (Instruccion s in t.getInstrucciones())
                    {
                        s.ejecutar(local);
                    }
                }

            }

            if (this.instr_else != null)
            {
                foreach (Instruccion s in this.instr_else)
                {
                    s.ejecutar(local);
                }
            }



            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class RepeatUntil:Instruccion
    {

        private Operacion condicion;
        private LinkedList<Instruccion> listaInstrucciones;

        public RepeatUntil(LinkedList<Instruccion> ins, Operacion cond)
        {
            this.condicion = cond;
            this.listaInstrucciones = ins;
        }

        public Object ejecutar(TSimbolo ts)
        {

            TSimbolo tablaLocal = new TSimbolo(ts);
            Primitivo local_cond = null;

            do
            {
                foreach (Instruccion ins in listaInstrucciones)
                {
                    ins.ejecutar(tablaLocal);
                }
                local_cond = (Primitivo)condicion.ejecutar(ts);

            } while ((Boolean)local_cond.valor);

            return null;
        }
    }
}

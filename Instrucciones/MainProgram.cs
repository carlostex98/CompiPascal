using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    public class MainProgram: Instruccion
    {

        private LinkedList<Instruccion> listaInstrucciones;

        public MainProgram(LinkedList<Instruccion> ins)
        {
            this.listaInstrucciones = ins;
        }

        public Object ejecutar(TSimbolo ts)
        {
            TSimbolo tablaLocal = new TSimbolo(ts);
            foreach (Instruccion ins in listaInstrucciones)
            {
               _ =  ins.ejecutar(tablaLocal);
            }

            return null;
        }
    }
}

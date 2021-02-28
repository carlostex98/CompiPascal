using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.Instrucciones;
using CompiPascal.TablaSimbolos;


namespace CompiPascal.Instrucciones
{
    class While:Instruccion
    {
        private Operacion condicion;
        private LinkedList<Instruccion> listaInstrucciones;

        public While(Operacion x, LinkedList<Instruccion> y)
        {
            this.condicion = x;
            this.listaInstrucciones = y;
        }

        public Object ejecutar(TSimbolo ts) 
        {
            while ((Boolean)condicion.ejecutar(ts))
            {
                TSimbolo tablaLocal = new TSimbolo(ts);//le pasamos los valores actuales a la copia de tabla de simbolos
                
                foreach (Instruccion ins in listaInstrucciones)
                {
                    ins.ejecutar(tablaLocal);
                }
            }
            return null;
        }
    }
}

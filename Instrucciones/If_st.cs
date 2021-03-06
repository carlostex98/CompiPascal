using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class If_st: Instruccion
    {

        private Operacion condicion;
        private LinkedList<Instruccion> listaInstrucciones;
        private LinkedList<Instruccion> listaInstruccionesElse;


        public If_st(Operacion x, LinkedList<Instruccion> y)
        {
            this.condicion = x;
            this.listaInstrucciones = y;
            this.listaInstruccionesElse = null;
        }


        public If_st(Operacion x, LinkedList<Instruccion> y, LinkedList<Instruccion> z)
        {
            this.condicion = x;
            this.listaInstrucciones = y;
            this.listaInstruccionesElse = z;
        }



        public Object ejecutar(TSimbolo ts)
        {
            Primitivo condres = (Primitivo)condicion.ejecutar(ts);

            if ((Boolean)(condres.valor))
            {
                TSimbolo tablaLocal = new TSimbolo(ts);//le pasamos los valores actuales a la copia de tabla de simbolos

                foreach (Instruccion ins in listaInstrucciones)
                {
                    ins.ejecutar(tablaLocal);
                }
            }
            else
            {
                if (this.listaInstruccionesElse != null)
                {
                    //contiene else con almenos una instruccion
                    TSimbolo tablaLocal = new TSimbolo(ts);

                    foreach (Instruccion ins in listaInstruccionesElse)
                    {
                        ins.ejecutar(tablaLocal);
                    }
                }
            }

            return null;
        }
    }
}

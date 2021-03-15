using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class MainProgram: Instruccion
    {

        private LinkedList<Instruccion> listaInstrucciones;
        private int linea;
        private int columna;

        public MainProgram(LinkedList<Instruccion> ins, int ln, int cl)
        {
            this.listaInstrucciones = ins;
            this.linea = ln;
            this.columna = cl;
        }

        public Object ejecutar(TSimbolo ts)
        {
            TSimbolo tablaLocal = new TSimbolo(ts);
            foreach (Instruccion ins in listaInstrucciones)
            {

                try
                {
                    _ = ins.ejecutar(tablaLocal);
                }
                catch (Error x)
                {
                    Maestro.Instance.addError(x);
                    Maestro.Instance.addOutput(x.getDescripcion());
                    //System.Diagnostics.Debug.WriteLine("eeeeeee");
                }

               
            }

            return null;
        }
    }
}

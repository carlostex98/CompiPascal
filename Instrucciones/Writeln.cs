﻿using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
using CompiPascal.TablaSimbolos;


namespace CompiPascal.Instrucciones
{
    public class Writeln: Instruccion
    {

        private LinkedList<Operacion> contenido;
        private int linea;
        private int columna;
        //cambiar a lista de instrucciones :)

        public Writeln(LinkedList<Operacion> c, int ln, int cl)
        {
            this.contenido = c;
            this.linea = ln;
            this.columna = cl;
        }

        public Object ejecutar(TSimbolo ts)
        {

            foreach (Operacion cc in contenido)
            {
                Primitivo x = (Primitivo)cc.ejecutar(ts);

                Maestro.Instance.addOutput(x.valor.ToString());
                //System.Diagnostics.Debug.WriteLine(x.valor.ToString());
            }

            
            return null;
        }

    }
}

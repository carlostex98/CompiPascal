using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.SuperClass
{
    public abstract class AbsNodo
    {
        private int fila;
        private int columna;
        public AbsNodo(int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
        }
        public int getFila()
        {
            return this.fila;
        }
        public int getColumna()
        {
            return this.columna;
        }

        public abstract Objeto ejecutar();

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.Analizador
{

    public class Error
    {
        public enum tipos
        {
            LEXICO, SINTACTICO, SEMANTICO
        }

        private int linea, columna;
        private string descripcion, ambito;
        private tipos tipo_;



        public Error(int linea, int columna, string descripcion, tipos tipo_)
        {
            this.linea = linea;
            this.columna = columna;
            this.descripcion = descripcion;
            this.tipo_ = tipo_;
            this.ambito = ""; 
        }

        public int getLinea()
        {
            return this.linea;
        }
        public int getColumna()
        {
            return this.columna;
        }
        public string getDescripcion()
        {
            return this.descripcion;
        }

    }
}

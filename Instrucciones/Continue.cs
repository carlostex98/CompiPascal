using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class Continue:Instruccion
    {
        private int linea;
        private int columna;

        public Continue(int ln, int cl)
        {
            this.linea = ln;
            this.columna = cl;
        }


        public Object ejecutar(TSimbolo ts)
        {
            return new Retorno(Retorno.tipoRetorno.BREAK, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class Exit:Instruccion
    {

        private Operacion op;
        private int linea;
        private int columna;

        public Exit(Operacion opr, int ln, int cl)
        {
            this.op = opr;
            this.linea = ln;
            this.columna = cl;
        }


        public Object ejecutar(TSimbolo ts)
        {
            Primitivo e = (Primitivo)this.op.ejecutar(ts);

            return new Retorno(Retorno.tipoRetorno.EXIT, e);
            
        }


    }
}

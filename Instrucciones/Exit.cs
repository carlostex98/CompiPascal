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

        public Exit(Operacion opr)
        {
            this.op = opr;
        }


        public Object ejecutar(TSimbolo ts)
        {
            Primitivo e = (Primitivo)this.op.ejecutar(ts);

            return new Retorno(Retorno.tipoRetorno.EXIT, e);
            
        }


    }
}

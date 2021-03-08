using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class Continue:Instruccion
    {

        public Continue()
        {

        }


        public Object ejecutar(TSimbolo ts)
        {
            return new Retorno(Retorno.tipoRetorno.BREAK, null);
        }
    }
}

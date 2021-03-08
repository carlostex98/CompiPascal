using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class Break:Instruccion
    {

        public Break()
        {

        }

        public Object ejecutar(TSimbolo ts)
        {
            return new Retorno(Retorno.tipoRetorno.BREAK, null);
        }

    }
}

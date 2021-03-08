using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{

    class Retorno
    {
        public enum tipoRetorno
        {
            EXIT,
            BREAK,
            CONTINUE
        }

        public tipoRetorno t_val;
        public Primitivo valor;


        public Retorno(tipoRetorno tip, Primitivo v)
        {
            this.t_val = tip;
            this.valor = v;
        }



    }
}

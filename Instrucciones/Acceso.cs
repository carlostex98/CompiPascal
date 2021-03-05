using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class Acceso:Instruccion
    {

        private string variable;

        public Acceso(string id)
        {
            this.variable = id;
        }

        public Object ejecutar(TSimbolo ts)
        {
            Simbolo s = ts.obtener(variable);

            Primitivo p = s.getValor();

            return p;
        }

    }
}

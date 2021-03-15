using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class GraficarTS:Instruccion
    {

        public GraficarTS()
        {
            // nada jaja
        }

        public Object ejecutar(TSimbolo ts)
        {
            //le decimos a la ts que hacer
            Maestro.Instance.separarGrafica();
            while (ts != null)
            {
                foreach (KeyValuePair<string, Simbolo> t in ts.variables)
                {
                    Maestro.Instance.agragarGrafica(t.Key, t.Value.Tipo.ToString(), t.Value.valor.valor.ToString());
                }
                ts = ts.heredado;
            }

            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class Writeln: Instruccion
    {

        private Instruccion contenido;
        //cambiar a lista de instrucciones :)

        public Writeln(Instruccion c)
        {
            this.contenido = c;
        }

        public Object ejecutar(TSimbolo ts)
        {
            
            Primitivo x = (Primitivo)contenido.ejecutar(ts);

            Maestro.Instance.addOutput(x.valor.ToString());
            return null;
        }

    }
}

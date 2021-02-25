using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
using CompiPascal.TablaSimbolos;
using System.Linq;
using System.Threading.Tasks;

namespace CompiPascal.Instrucciones
{
    class Writeln: Instruccion
    {

        private Instruccion contenido;

        public Writeln(Instruccion c)
        {
            this.contenido = c;
            
        }

        public Object ejecutar(TSimbolo ts)
        {

            string e = contenido.ejecutar(ts).ToString();
            Maestro.Instance.addOutput(e);
            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
using CompiPascal.TablaSimbolos;


namespace CompiPascal.Instrucciones
{
    public class Writeln: Instruccion
    {

        private Operacion contenido;
        //cambiar a lista de instrucciones :)

        public Writeln(Operacion c)
        {
            this.contenido = c;
        }

        public Object ejecutar(TSimbolo ts)
        {
            
            Primitivo x = (Primitivo)contenido.ejecutar(ts);

            Maestro.Instance.addOutput(x.valor.ToString());
            System.Diagnostics.Debug.WriteLine(x.valor.ToString());
            return null;
        }

    }
}

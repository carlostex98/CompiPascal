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
        private int linea;
        private int columna;
        //cambiar a lista de instrucciones :)

        public Writeln(Operacion c, int ln, int cl)
        {
            this.contenido = c;
            this.linea = ln;
            this.columna = cl;
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

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class Asignacion: Instruccion
    {

        public String id;
        private Operacion valor;

        public Asignacion(String a, Operacion b)
        {
            this.id = a;
            this.valor = b;
        }

        public Object ejecutar(TSimbolo ts)
        {
            Primitivo e = (Primitivo)valor.ejecutar(ts);

            ts.redefinir(id, e);
            return null;
        }
    }
}

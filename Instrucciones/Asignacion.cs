using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class Asignacion: Instruccion
    {

        private String id;
        private Operacion valor;

        public Asignacion(String a, Operacion b)
        {
            this.id = a;
            this.valor = b;
        }

        public Object ejecutar(TSimbolo ts)
        {
            //ts.redefinir(id, valor.ejecutar);
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class Declaracion: Instruccion
    {

        private string nombre;
        private Primitivo valor;
        private Operacion opr;

        public Declaracion(string a, Operacion op)
        {
            this.nombre = a;
            this.opr = op;
        }

        public Object ejecutar(TSimbolo ts)
        {
            this.valor = (Primitivo)this.opr.ejecutar(ts);
            Simbolo e = new Simbolo(nombre, valor);
            ts.agregar(nombre, e);
            return null;
        }
    }
}

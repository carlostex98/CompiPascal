using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.SuperClass;
using CompiPascal.Objetos;

namespace CompiPascal.Expresiones
{
    class Constante:AbsNodo
    {
        Objeto valor;
        public Constante(int fila, int columna, Objeto valor) : base(fila, columna)
        {
            this.valor = valor;
        }
        public override Objeto ejecutar()
        {
            return this.valor;
        }
    }
}

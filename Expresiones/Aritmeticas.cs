using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.SuperClass;

namespace CompiPascal.Expresiones
{
    class Aritmeticas: AbsNodo
    {

        public enum TipoA { SUMA, RESTA, MULTIPLICACION, DIVISION }
        TipoA tipoA;
        private AbsNodo left;
        private AbsNodo right;
        public Aritmeticas(int fila, int columna, TipoA tipoA, AbsNodo left, AbsNodo right) : base(fila, columna)
        {
            this.tipoA = tipoA;
            this.left = left;
            this.right = right;
        }

        public override Objeto ejecutar()
        {
            Objeto res_left = this.left.ejecutar(); //5
            Objeto res_right = this.right.ejecutar();   //5


            if (this.tipoA == TipoA.SUMA)
            {
                return new Primitivo(Objeto.Tipo.INTEGER, Int16.Parse(res_left.getValue().ToString()) + Int16.Parse(res_right.getValue().ToString()));
            }
            else if (this.tipoA == TipoA.RESTA)
            {
                return new Primitivo(Objeto.Tipo.INTEGER, Int16.Parse(res_left.getValue().ToString()) - Int16.Parse(res_right.getValue().ToString()));
            }
            else if (this.tipoA == TipoA.MULTIPLICACION)
            {
                return new Primitivo(Objeto.Tipo.INTEGER, Int16.Parse(res_left.getValue().ToString()) * Int16.Parse(res_right.getValue().ToString()));
            }
            else if (this.tipoA == TipoA.DIVISION)
            {
                return new Primitivo(Objeto.Tipo.INTEGER, Int16.Parse(res_left.getValue().ToString()) / Int16.Parse(res_right.getValue().ToString()));
            }
            else
            {
                return null;
            }
        }


    }
}

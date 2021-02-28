using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class Operacion: Instruccion
    {
        public enum Tipo_operacion
        {
            SUMA,
            RESTA,
            MULTIPLICACION,
            DIVISION,
            NEGATIVO,
            NUMERO,
            IDENTIFICADOR,
            CADENA,
            MAYOR_QUE,
            MENOR_QUE,
            CONCATENACION,
            YY,
            OO,
            AUMENTO,
            DECREMENTO,
            MAYOR_I,
            MENOR_I,
            NEGACION
        }

        private Tipo_operacion tipo;
        private Operacion operadorIzq;
        private Operacion operadorDer;
        private Object valor;

        // suma, resta, multiplicacion, division, concat, mayor, menor, or, and
        public Operacion(Operacion operadorIzq, Operacion operadorDer, Tipo_operacion tipo)
        {
            this.tipo = tipo;
            this.operadorIzq = operadorIzq;
            this.operadorDer = operadorDer;
        }

        //++,--,-val, not
        public Operacion(Operacion operadorIzq, Tipo_operacion tipo)
        {
            this.tipo = tipo;
            this.operadorIzq = operadorIzq;
        }


        //id, cadena
        public Operacion(String a, Tipo_operacion tipo)
        {
            this.valor = a;
            this.tipo = tipo;
        }

        //int, decimal
        public Operacion(Double a)
        {
            this.valor = a;
            this.tipo = Tipo_operacion.NUMERO;
        }


        public Object ejecutar(TSimbolo ts)
        {
            if (tipo == Tipo_operacion.DIVISION)
            {
                return (Double)operadorIzq.ejecutar(ts) / (Double)operadorDer.ejecutar(ts);
            }
            else if (tipo == Tipo_operacion.MULTIPLICACION)
            {
                return (Double)operadorIzq.ejecutar(ts) * (Double)operadorDer.ejecutar(ts);
            }
            else if (tipo == Tipo_operacion.RESTA)
            {
                return (Double)operadorIzq.ejecutar(ts) - (Double)operadorDer.ejecutar(ts);
            }
            else if (tipo == Tipo_operacion.SUMA)
            {
                return (Double)operadorIzq.ejecutar(ts) + (Double)operadorDer.ejecutar(ts);
            }
            else if (tipo == Tipo_operacion.NEGATIVO)
            {
                return (Double)operadorIzq.ejecutar(ts) * -1;
            }
            else if (tipo == Tipo_operacion.NEGACION)
            {
                return !(Boolean)operadorIzq.ejecutar(ts) ;
            }
            else if (tipo == Tipo_operacion.NUMERO)
            {
                return Double.Parse(valor.ToString());
            }
            else if (tipo == Tipo_operacion.IDENTIFICADOR)
            {
                //return ts.getValor(valor.ToString());
                return ts.obtener(valor.ToString());
            }
            else if (tipo == Tipo_operacion.CADENA)
            {
                return valor.ToString();
            }
            else if (tipo == Tipo_operacion.MAYOR_QUE)
            {
                return ((Double)operadorIzq.ejecutar(ts)) > ((Double)operadorDer.ejecutar(ts));
            }
            else if (tipo == Tipo_operacion.MENOR_QUE)
            {
                return ((Double)operadorIzq.ejecutar(ts)) < ((Double)operadorDer.ejecutar(ts));
            }
            else if (tipo == Tipo_operacion.MAYOR_I)
            {
                return ((Double)operadorIzq.ejecutar(ts)) >= ((Double)operadorDer.ejecutar(ts));
            }
            else if (tipo == Tipo_operacion.MENOR_I)
            {
                return ((Double)operadorIzq.ejecutar(ts)) <= ((Double)operadorDer.ejecutar(ts));
            }
            else if (tipo == Tipo_operacion.CONCATENACION)
            {
                return operadorIzq.ejecutar(ts).ToString() + operadorDer.ejecutar(ts).ToString();
            }
            else if (tipo == Tipo_operacion.OO)
            {
                return ((Boolean)operadorIzq.ejecutar(ts)) || ((Boolean)operadorDer.ejecutar(ts));
            }
            else if (tipo == Tipo_operacion.YY)
            {
                return ((Boolean)operadorIzq.ejecutar(ts)) && ((Boolean)operadorDer.ejecutar(ts));
            }
            else if (tipo == Tipo_operacion.AUMENTO)
            {
                return (Double)operadorIzq.ejecutar(ts) + 1;
            }
            else if (tipo == Tipo_operacion.DECREMENTO)
            {
                return (Double)operadorIzq.ejecutar(ts) - 1;
            }
            else
            {
                return null;
            }
        }

    }
}

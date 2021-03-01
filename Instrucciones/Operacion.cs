using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
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
            NEGACION,
            PRIMITIVO
        }

        private Tipo_operacion tipo;
        private Operacion operadorIzq;
        private Operacion operadorDer;
        private Object valor; //pendiente eliminar
        private Primitivo val;
       
        // maneja todas
        public Operacion(Operacion operadorIzq, Operacion operadorDer, Tipo_operacion tipo)
        {
            this.tipo = tipo;
            this.operadorIzq = operadorIzq;
            this.operadorDer = operadorDer;
        }

        //solo es un valor
        public Operacion(Primitivo prim)
        {
            this.tipo = Tipo_operacion.PRIMITIVO;
            this.val = prim;
        }


        //retorna simbolo

        public Object ejecutar(TSimbolo ts)
        {
            Object der = operadorIzq.ejecutar(ts);
            Object izq = operadorDer.ejecutar(ts);

            Primitivo a = (Primitivo)izq;
            Primitivo b = (Primitivo)der;

            if (tipo == Tipo_operacion.DIVISION)
            {   
                return new Primitivo(Primitivo.tipo_val.INT, (object)((Double)a.valor / (Double)b.valor));
            }
            else if (tipo == Tipo_operacion.MULTIPLICACION)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)((Double)a.valor * (Double)b.valor));
            }
            else if (tipo == Tipo_operacion.RESTA)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)((Double)a.valor - (Double)b.valor));
            }
            else if (tipo == Tipo_operacion.SUMA)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)((Double)a.valor + (Double)b.valor));
            }
            else if (tipo == Tipo_operacion.NEGATIVO)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)((Double)a.valor / (Double)(-1))); //pendiente
            }
            else if (tipo == Tipo_operacion.NEGACION)
            {
                return !(Boolean)operadorIzq.ejecutar(ts) ; //pendiente
            }
            else if (tipo == Tipo_operacion.MAYOR_QUE)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)((Double)a.valor > (Double)(b.valor)));
            }
            else if (tipo == Tipo_operacion.MENOR_QUE)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)((Double)a.valor < (Double)(b.valor)));
            }
            else if (tipo == Tipo_operacion.MAYOR_I)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)((Double)a.valor >= (Double)(b.valor)));
            }
            else if (tipo == Tipo_operacion.MENOR_I)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)((Double)a.valor <= (Double)(b.valor)));
            }
            else if (tipo == Tipo_operacion.CONCATENACION)
            {
                return new Primitivo(Primitivo.tipo_val.CADENA, (object)((String)a.valor + (String)(b.valor)));
            }
            else if (tipo == Tipo_operacion.OO)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)((Boolean)a.valor || (Boolean)(b.valor)));
            }
            else if (tipo == Tipo_operacion.YY)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)((Boolean)a.valor && (Boolean)(b.valor)));
            }
            else if (tipo == Tipo_operacion.PRIMITIVO)
            {
                return this.val; // si es el minimo valor
            }
            else
            {
                return null;
            }
        }

    }
}

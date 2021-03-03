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
            PRIMITIVO,
            UNICO,
            MODULO,
            EQUIVALENCIA
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

        public Operacion(Operacion prim)
        {
            this.tipo = Tipo_operacion.UNICO;
            this.operadorIzq = prim;
        }


        //retorna simbolo

        public Object ejecutar(TSimbolo ts)
        {
            //relizar validacion de tipos
            

            Object der = new Object();
            Object izq = new Object();
            izq = null;
            der = null;


            if (tipo == Tipo_operacion.NEGATIVO || tipo == Tipo_operacion.NEGACION || tipo == Tipo_operacion.UNICO)
            {
                //solo nos interesa el izquierdo
                izq = operadorIzq.ejecutar(ts);
                der = null;
            }
            else 
            {
                if (tipo != Tipo_operacion.PRIMITIVO)
                {
                    izq = operadorIzq.ejecutar(ts);
                    der = operadorDer.ejecutar(ts);
                }
                
                
            }

            Primitivo a = (Primitivo)izq;
            Primitivo b = (Primitivo)der;

            if (tipo == Tipo_operacion.DIVISION)
            {   
                return new Primitivo(Primitivo.tipo_val.INT, (object)(Convert.ToDouble(a.valor) / Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.MULTIPLICACION)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)(Convert.ToDouble(a.valor) * Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.RESTA)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)(Convert.ToDouble(a.valor) - Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.MODULO)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)(Convert.ToDouble(a.valor) % Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.SUMA)
            {
                //si los dos son string hace la concatenacion
                if (a.t_val == Primitivo.tipo_val.CADENA && b.t_val == Primitivo.tipo_val.CADENA)
                {
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(a.valor));
                    return new Primitivo(Primitivo.tipo_val.CADENA, (object)(Convert.ToString(a.valor) + Convert.ToString(b.valor)));
                }
                else
                {
                    return new Primitivo(Primitivo.tipo_val.INT, (object)( Convert.ToDouble(a.valor) + Convert.ToDouble(b.valor)));
                }

            }
            else if (tipo == Tipo_operacion.NEGATIVO)
            {
                return new Primitivo(Primitivo.tipo_val.INT, (object)(Convert.ToDouble(a.valor) + Convert.ToDouble(-1)));
            }
            else if (tipo == Tipo_operacion.NEGACION)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)!(Convert.ToBoolean(a.valor)));
            }
            else if (tipo == Tipo_operacion.MAYOR_QUE)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(Convert.ToDouble(a.valor) > Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.MENOR_QUE)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(Convert.ToDouble(a.valor) < Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.MAYOR_I)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(Convert.ToDouble(a.valor) >= Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.MENOR_I)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(Convert.ToDouble(a.valor) <= Convert.ToDouble(b.valor)));
            }
            else if (tipo == Tipo_operacion.OO)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(Convert.ToBoolean(a.valor) || Convert.ToBoolean(b.valor)));
            }
            else if (tipo == Tipo_operacion.YY)
            {
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(Convert.ToBoolean(a.valor) && Convert.ToBoolean(b.valor)));
            }
            else if (tipo == Tipo_operacion.EQUIVALENCIA)
            {
                
                return new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)(a.valor.Equals(b.valor)));
            }
            else if (tipo == Tipo_operacion.PRIMITIVO)
            {
                return this.val; // si es el minimo valor
            }
            else if (tipo == Tipo_operacion.UNICO)
            {
                return new Primitivo(a.t_val, (object)a.valor);
            }
            else
            {
                return null;
            }
        }

    }
}

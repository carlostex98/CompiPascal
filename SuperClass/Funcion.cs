using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.SuperClass
{
    public abstract class Funcion : AbsNodo
    {
        private LinkedList<Object> parametros_Formales; // En realidad, debe ser de tipo 'param'
        private LinkedList<Objeto> parametros_Actuales; // estos son los parámetros que se van a setear en la 'Llamada a función' --> FunctionCall
        private string nombre;

        public Funcion(LinkedList<Object> parametros_Formales, string nombre, int fila, int columna) : base(fila, columna)
        {
            this.parametros_Actuales = new LinkedList<Objeto>();
            this.parametros_Formales = parametros_Formales;
            this.nombre = nombre;
        }

        public LinkedList<Objeto> getParametrosActuales()
        {
            return this.parametros_Actuales;
        }

        public LinkedList<Object> getParametrosFormales()
        {
            return this.parametros_Formales;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public void setParametrosActuales(LinkedList<Objeto> tmp)
        {
            this.parametros_Actuales = tmp;
        }

        public abstract Objeto ejecutarFuncion(LinkedList<Objeto> parametros_actuales);
    }
}

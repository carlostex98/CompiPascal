using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.SuperClass;

namespace CompiPascal.Funciones
{
    public class CallFuncion:AbsNodo
    {
        private LinkedList<AbsNodo> parametros;     // lista de parámetros que el usuario le quiere enviar
        private string id;      //nombre de la función que quiero llamar

        public CallFuncion(LinkedList<AbsNodo> parametros, string id, int fila, int columna) : base(fila, columna)
        {
            this.parametros = parametros;
            this.id = id;
        }



        public override Objeto ejecutar()
        {
            //aqui vamos a llamar a la función que solicito, luego la ejecuto. 
            Funcion tmp = Maestro.Instance.getFuncion(this.id);

            LinkedList<Objeto> resultados = new LinkedList<Objeto>();
            foreach (AbsNodo nodo in this.parametros)
            {
                resultados.AddLast(nodo.ejecutar());
            }
            return tmp.ejecutarFuncion(resultados); //le mandamos los parametros actuales y la mandamos a ejecutar. 
        }
    }
}

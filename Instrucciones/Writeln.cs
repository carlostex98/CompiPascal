using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.SuperClass;

namespace CompiPascal.Instrucciones
{
    public class Writeln:Funcion
    {
        public Writeln(int fila, int columna) : base(new LinkedList<object>(), "writeln", fila, columna)
        {
        }

        /*Esta función abstracta es para la declaración de la función. La hago como que si se tratara de una instrucción más. 
         * **/
        public override Objeto ejecutar()
        {
            // aqui agrego la función a la lista de funciones de la singleton
            bool res = Maestro.Instance.addFunction(this);
            /*
            
            ... agregar validaciones

             */
            return null;

        }

        public override Objeto ejecutarFuncion(LinkedList<Objeto> parametros_actuales)
        {
            Maestro.Instance.addOutput(parametros_actuales.First.Value.getValue().ToString());
            return null;
        }

    }
}

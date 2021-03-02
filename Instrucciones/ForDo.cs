using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class ForDo:Instruccion
    {
        private LinkedList<Instruccion> listaInstrucciones;
        private string identififcador;
        private Operacion val_inicio;
        private Operacion val_limite;

        public ForDo(string id_var, Operacion a, Operacion b, LinkedList<Instruccion> lst)
        {
            //recibimos un string, operacion, operacion y lista instrucciones
            this.identififcador = id_var;
            this.val_inicio = a;
            this.val_limite = b;
            this.listaInstrucciones = lst;
        }


        public Object ejecutar(TSimbolo ts)
        {
            Declaracion inc = new Declaracion(identififcador, val_inicio);
            inc.ejecutar(ts);

            return null;
        }

    }
}

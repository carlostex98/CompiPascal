using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.Instrucciones;

namespace CompiPascal.General
{
    public class FuncionDato
    {

        private LinkedList<Instruccion> listaInstrucciones;
        private LinkedList<Declaracion> variables;
        private string nombre;
        public enum tipoF { FUNCION, PROCEDIMINETO };

        private tipoF tipo;

        public FuncionDato(string n, LinkedList<Instruccion> lst, LinkedList<Declaracion> vars, tipoF t)
        {
            this.nombre = n;
            this.listaInstrucciones = lst;
            this.variables = vars;
            this.tipo = t;
        }


        public LinkedList<Instruccion> retornarInstrucciones()
        {
            return this.listaInstrucciones;
        }

        public LinkedList<Declaracion> retornarVars()
        {
            return this.variables;
        }


    }
}

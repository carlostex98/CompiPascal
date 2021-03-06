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

        public FuncionDato(string n, LinkedList<Instruccion> lst, LinkedList<Declaracion> vars)
        {
            this.nombre = n;
            this.listaInstrucciones = lst;
            this.variables = vars;
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

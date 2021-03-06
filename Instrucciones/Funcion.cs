using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class Funcion: Instruccion
    {

        private LinkedList<Instruccion> listaInstrucciones;
        private LinkedList<Declaracion> variables;
        private string nombre;
        private TSimbolo auxiliar;


        public Funcion(string n, LinkedList<Instruccion> lst, LinkedList<Declaracion> vars)
        {
            this.nombre = n;
            this.listaInstrucciones = lst;
            this.variables = vars;
        }




        public Object ejecutar(TSimbolo ts)
        {
            //se guarda en la singleton
            FuncionDato g = new FuncionDato(nombre, listaInstrucciones, variables);
            Maestro.Instance.guardarFuncion(nombre, g);

            return null;
        }

    }
}

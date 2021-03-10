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
        private FuncionDato.tipoF tipo;


        public Funcion(string n, LinkedList<Instruccion> lst, LinkedList<Declaracion> vars, FuncionDato.tipoF t)
        {
            this.nombre = n;
            this.listaInstrucciones = lst;
            this.variables = vars;
            this.tipo = t;
        }




        public Object ejecutar(TSimbolo ts)
        {
            //se guarda en la singleton
            FuncionDato g = new FuncionDato(nombre, listaInstrucciones, variables, FuncionDato.tipoF.FUNCION);
            Maestro.Instance.guardarFuncion(nombre, g);

            return null;
        }

    }
}

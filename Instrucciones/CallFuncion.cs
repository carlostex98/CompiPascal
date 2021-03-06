using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    class CallFuncion : Instruccion
    {

        private string nombre;
        private LinkedList<Operacion> parametros;
        private LinkedList<Primitivo> final;

        public CallFuncion(string n, LinkedList<Operacion> p)
        {
            this.nombre = n;
            this.parametros = p;
        }


        public Object ejecutar(TSimbolo ts)
        {
            FuncionDato f = Maestro.Instance.AccederFuncion(this.nombre);
            TSimbolo local = new TSimbolo(ts);
            TSimbolo aux = new TSimbolo();


            LinkedList<string> nombres = new LinkedList<string>();


            if (parametros != null)
            {

                foreach (Operacion p in parametros)
                {
                    final.AddLast((Primitivo)p.ejecutar(local));
                }

                foreach (Declaracion t in f.retornarVars())
                {
                    t.ejecutar(aux);
                }


                if (final.Count == aux.variables.Count)
                {
                    //si la cantidad de variables de llamada son a las declaradas
                    foreach (Declaracion t in f.retornarVars())
                    {
                        t.ejecutar(local);
                    }

                    foreach (KeyValuePair<string, Simbolo> t in local.variables)
                    {
                        nombres.AddLast(t.Key.ToString());
                    }

                    int i = 0;
                    int x = 0;
                    foreach (Primitivo t in this.final)
                    {
                        string nx;
                        foreach (string no in nombres)
                        {
                            if (x == i)
                            {
                                nx = no;
                                break;
                            }
                            x++;
                        }

                        Asignacion a = new Asignacion(nombre, new Operacion(t));
                        a.ejecutar(local);
                        i++;
                    }

                    //ya etsn las variables y asignaciones en la llamada

                    foreach (Instruccion ins in f.retornarInstrucciones())
                    {
                        ins.ejecutar(local);
                    }

                }

            }
            else
            {
                foreach (Instruccion ins in f.retornarInstrucciones())
                {
                    ins.ejecutar(local);
                }
            }

            

            return null;
        }

    }
}

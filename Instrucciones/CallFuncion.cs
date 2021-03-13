using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;
using CompiPascal.TablaSimbolos;

namespace CompiPascal.Instrucciones
{
    public class CallFuncion : Instruccion
    {

        private string nombre;
        private LinkedList<Operacion> parametros;
        private int linea;
        private int columna;

        public CallFuncion(string n, LinkedList<Operacion> p, int ln, int cl)
        {
            this.nombre = n;
            this.parametros = p;
            this.linea = ln;
            this.columna = cl;
        }


        public Object ejecutar(TSimbolo ts)
        {
            bool ss = Maestro.Instance.verificarFuncion(this.nombre);
            if (!ss)
            {
                throw new Error(linea, columna, "Funcion: "+nombre+", no existe", Error.Tipo_error.SINTACTICO);
            }

            FuncionDato f = Maestro.Instance.AccederFuncion(this.nombre);
            TSimbolo local = new TSimbolo(ts);
            TSimbolo aux = new TSimbolo();


            LinkedList<string> nombres = new LinkedList<string>();


            if (parametros != null)
            {

                LinkedList<Primitivo> final = new LinkedList<Primitivo>();

                foreach (Operacion p in parametros)
                {
                    final.AddLast((Primitivo)p.ejecutar(local));
                }

                //System.Diagnostics.Debug.WriteLine(f.retornarVars().Count);

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
                    foreach (Primitivo t in final)
                    {
                        string nx = "";
                        foreach (string no in nombres)
                        {
                            if (x == i)
                            {
                                nx = no;
                                break;
                            }
                            x++;
                        }

                        Asignacion a = new Asignacion(nx, new Operacion(t), linea, columna);
                        a.ejecutar(local);
                        i++;
                    }

                    //ya etsn las variables y asignaciones en la llamada

                }

            }
            

            foreach (Instruccion ins in f.retornarInstrucciones())
            {
                Retorno r = (Retorno)ins.ejecutar(local);
                if (r != null)
                {
                    if (r.t_val == Retorno.tipoRetorno.EXIT)
                    {
                        if (f.t_retorno == FuncionDato.tipoR.VOID)
                        {
                            throw new Error(linea, columna, "Una funcion tipo void no acepta retorno", Error.Tipo_error.SEMANTICO);
                        }

                        if (f.tipo == FuncionDato.tipoF.PROCEDIMINETO)
                        {
                            throw new Error(linea, columna, "Un procedimiento no puede reotornar un valor", Error.Tipo_error.SEMANTICO);
                        }

                        Primitivo s = r.valor;
                        if (f.t_retorno == FuncionDato.tipoR.BOOLEAN)
                        {
                            if (s.t_val != Primitivo.tipo_val.BOOLEANO)
                            {
                                throw new Error(linea, columna, "Tipo de retorno y funcion incompatible, se esperaba booleano", Error.Tipo_error.SEMANTICO);
                            }
                        } 
                        else if (f.t_retorno == FuncionDato.tipoR.STRING)
                        {
                            if (s.t_val != Primitivo.tipo_val.CADENA)
                            {
                                throw new Error(linea, columna, "Tipo de retorno y funcion incompatible, se esperaba cadena", Error.Tipo_error.SEMANTICO);
                            }
                        }
                        else if (f.t_retorno == FuncionDato.tipoR.REAL)
                        {
                            if (s.t_val != Primitivo.tipo_val.INT && s.t_val != Primitivo.tipo_val.DECIMAL)
                            {
                                throw new Error(linea, columna, "Tipo de retorno y funcion incompatible, se esperaba un numero", Error.Tipo_error.SEMANTICO);
                            }
                        }
                        else if (f.t_retorno == FuncionDato.tipoR.INTEGER)
                        {
                            if (s.t_val != Primitivo.tipo_val.INT && s.t_val != Primitivo.tipo_val.DECIMAL)
                            {
                                throw new Error(linea, columna, "Tipo de retorno y funcion incompatible, se esperaba un numero", Error.Tipo_error.SEMANTICO);
                            }
                        }


                        return r;
                    }
                    //break y continue deben arrojar error
                }
            }

            return null;
        }

    }
}

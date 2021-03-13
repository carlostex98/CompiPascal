using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class If_st: Instruccion
    {

        private Operacion condicion;
        private LinkedList<Instruccion> listaInstrucciones;
        private LinkedList<Instruccion> listaInstruccionesElse;
        private int linea;
        private int columna;

        public If_st(Operacion x, LinkedList<Instruccion> y, int ln, int cl)
        {
            this.condicion = x;
            this.listaInstrucciones = y;
            this.listaInstruccionesElse = null;
            this.linea = ln;
            this.columna = cl;
        }


        public If_st(Operacion x, LinkedList<Instruccion> y, LinkedList<Instruccion> z, int ln, int cl)
        {
            this.condicion = x;
            this.listaInstrucciones = y;
            this.listaInstruccionesElse = z;
            this.linea = ln;
            this.columna = cl;
        }



        public Object ejecutar(TSimbolo ts)
        {
            Primitivo condres = (Primitivo)condicion.ejecutar(ts);

            if ((Boolean)(condres.valor))
            {
                TSimbolo tablaLocal = new TSimbolo(ts);//le pasamos los valores actuales a la copia de tabla de simbolos

                foreach (Instruccion ins in listaInstrucciones)
                {
                    
                    Retorno r = (Retorno)ins.ejecutar(tablaLocal);
                    if (r != null)
                    {
                        if (r.t_val == Retorno.tipoRetorno.EXIT)
                        {
                            return r;
                        }
                        else if(r.t_val == Retorno.tipoRetorno.BREAK || r.t_val == Retorno.tipoRetorno.CONTINUE)
                        {
                            return r;
                        }
                        
                    }
                }
            }
            else
            {
                if (this.listaInstruccionesElse != null)
                {
                    //contiene else con almenos una instruccion
                    TSimbolo tablaLocal = new TSimbolo(ts);

                    foreach (Instruccion ins in listaInstruccionesElse)
                    {
                        Retorno r = (Retorno)ins.ejecutar(tablaLocal);
                        if (r != null)
                        {
                            if (r.t_val == Retorno.tipoRetorno.EXIT)
                            {
                                return r;
                            }
                            else if (r.t_val == Retorno.tipoRetorno.BREAK || r.t_val == Retorno.tipoRetorno.CONTINUE)
                            {
                                return r;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}

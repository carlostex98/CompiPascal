﻿using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    public class Asignacion: Instruccion
    {

        public string id;
        private Operacion valor;
        private int linea;
        private int columna;

        public Asignacion(string a, Operacion b, int ln, int cl)
        {
            this.id = a;
            this.valor = b;
            this.linea = ln;
            this.columna = cl; 
        }

        public Object ejecutar(TSimbolo ts)
        {
            Primitivo e = (Primitivo)valor.ejecutar(ts);
            

            Simbolo s = ts.obtener(id);


            if (s==null)
            {
                throw new Error(linea, columna, "variable: " + id + ", no existe", Error.Tipo_error.SINTACTICO);
            }
            else
            {
                //comparamos el tipo
                if (s.GetTipo() == Simbolo.tipo.STRING)
                {
                    if (e.t_val != Primitivo.tipo_val.CADENA)
                    {
                        throw new Error(linea, columna, "Se recibió otro valor en variable tipo cadena", Error.Tipo_error.SINTACTICO);
                    }


                }
                else if (s.GetTipo() == Simbolo.tipo.INTEGER)
                {
                    if (e.t_val != Primitivo.tipo_val.INT && e.t_val != Primitivo.tipo_val.DECIMAL)
                    {
                        throw new Error(linea, columna, "Se recibió otro valor en variable tipo entero", Error.Tipo_error.SINTACTICO);
                    }


                }
                else if (s.GetTipo() == Simbolo.tipo.REAL)
                {
                    if (e.t_val != Primitivo.tipo_val.INT && e.t_val != Primitivo.tipo_val.DECIMAL)
                    {
                        throw new Error(linea, columna, "Se recibió otro valor en variable tipo real", Error.Tipo_error.SINTACTICO);
                    }


                }
                else if (s.GetTipo() == Simbolo.tipo.BOOLEAN)
                {
                    if (e.t_val != Primitivo.tipo_val.BOOLEANO)
                    {
                        throw new Error(linea, columna, "Se recibió otro valor en variable tipo booleano", Error.Tipo_error.SINTACTICO);
                    }

                }

                bool x = ts.esEspecial(id);

                ts.redefinir(id, e);
                Simbolo xd = ts.obtener(id);
                //System.Diagnostics.Debug.WriteLine(e.valor);
                

                if (x)
                {
                    //retorno
                    Primitivo sm = ts.obtener(id).valor;
                    //return sm;
                    return new Retorno(Retorno.tipoRetorno.EXIT, sm);
                }
            }

           

            return null;
        }
    }
}

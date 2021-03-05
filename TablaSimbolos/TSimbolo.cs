using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.General;

namespace CompiPascal.TablaSimbolos
{
    public class TSimbolo
    {
        public TSimbolo heredado;
        public Dictionary<string, Simbolo> variables = new Dictionary<string, Simbolo>();

        public TSimbolo(TSimbolo ts = null)
        {
            this.heredado = ts;
        }


        public bool agregar(string nombre, Simbolo sb)
        {
            //la funcion verifica si existe la variable
            if (!this.variables.ContainsKey(nombre))
            {
                this.variables.Add(nombre, sb);
                return true;
            }
            return false;
        }


        public Simbolo obtener(string nombre)
        {
            if (this.variables.ContainsKey(nombre))
            {
                Simbolo obj;
                this.variables.TryGetValue(nombre, out obj);
                return obj;
            }
            else
            {
                //recorremos los demas contextos
                TSimbolo aux = this.heredado;
                while (aux != null)
                {
                    if (aux.variables.ContainsKey(nombre))
                    {
                        Simbolo obj;
                        aux.variables.TryGetValue(nombre, out obj);
                        return obj;
                    }
                    aux = aux.heredado;
                }


            }
            return null; //variable no existente en el contexto
        }


        public bool redefinir(string nombre, Primitivo sb)
        {
            //verificamos que exista, de lo contrario retorna false
            //todo: varificar que sean del mismo tipo

            if (this.variables.ContainsKey(nombre))
            {
                //this.variables[nombre] = sb;
                Simbolo s;
                this.variables.TryGetValue(nombre, out s);
                s.setValor(sb);
                this.variables[nombre] = s;
                return true;
            }
            else
            {
                //recorremos los demas contextos
                TSimbolo aux = this.heredado;
                while (aux != null)
                {
                    if (aux.variables.ContainsKey(nombre))
                    {
                        //aux.variables[nombre] = sb;
                        Simbolo s;
                        aux.variables.TryGetValue(nombre, out s);
                        s.setValor(sb);
                        aux.variables[nombre] = s;
                        return true;
                    }
                    aux = aux.heredado;
                }

            }



            return false;
        }



    }
}

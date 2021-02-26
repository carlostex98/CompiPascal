﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.TablaSimbolos
{
    public class TSimbolo
    {
        public TSimbolo heredado;
        public Dictionary<string, Simbolo> variables = new Dictionary<string, Simbolo>();

        public TSimbolo(TSimbolo ts)
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
            return null; //variable no existente en el contexto
        }


        public bool redefinir(string nombre, Simbolo sb)
        {
            //verificamos que exista, de lo contrario retorna null
            //todo: varificar que sean del mismo tipo

            if (this.variables.ContainsKey(nombre))
            {
                this.variables[nombre] = sb;
                return true;
            }
            return false;
        }



    }
}
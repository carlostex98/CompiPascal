using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.SuperClass;

namespace CompiPascal.Simbolos
{
    class Ambiente
    {

        public Ambiente anterior;
        private Dictionary<string, Objeto> variables = new Dictionary<string, Objeto>();

        public Ambiente(Ambiente recibido = null)
        {
            this.anterior = recibido;
        }

        public Objeto getVar(string id)
        {
            Objeto res;

            if (variables.TryGetValue(id, out res))
            {
                return res;
            }

            return null; //valida en llamada 
        }

        public void setVar(string id, Objeto obj)
        {
            //cool
            variables.Add(id, obj);
        }




    }
}

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

        public void getVar()
        {

        }

        public void setVar()
        {

        }




    }
}

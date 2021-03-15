﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.Instrucciones
{
    public class MultiElse
    {
        public Operacion opr;
        public LinkedList<Instruccion> listado;

        public MultiElse(Operacion o, LinkedList<Instruccion> listado)
        {
            this.opr = o;
            this.listado = listado;
        }
    }
}

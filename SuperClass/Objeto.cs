using System;
using System.Collections.Generic;
using System.Text;

namespace CompiPascal.SuperClass
{
    public abstract class Objeto
    {
        public enum Tipo
        {
            INTEGER,
            BOOLEAN,
            STRING,
            REAL,
            ARRAY
        }

        public Tipo tipo;

        public Objeto(Tipo tipo)
        {
            this.tipo = tipo;
        }

        public abstract string toString();
        public abstract object getValue();

    }
}

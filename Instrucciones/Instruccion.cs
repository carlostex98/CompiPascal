using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using System.Linq;
using System.Threading.Tasks;

namespace CompiPascal.Instrucciones
{
    public interface Instruccion
    {
        Object ejecutar(TSimbolo ts);
    }
}

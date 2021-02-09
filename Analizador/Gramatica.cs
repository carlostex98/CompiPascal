using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;


namespace CompiPascal.Analizador
{
    class Gramatica: Grammar
    {



        public Gramatica()
        {
            
            #region ER
            var NUMERO = new NumberLiteral("Numero");
            #endregion

            #region Terminales
            var REVALUAR = ToTerm("Evaluar");
            var PTCOMA = ToTerm(";");
            var PARIZQ = ToTerm("(");
            var PARDER = ToTerm(")");
            var CORIZQ = ToTerm("[");
            var CORDER = ToTerm("]");
            var MAS = ToTerm("+");
            var MENOS = ToTerm("-");
            var POR = ToTerm("*");
            var DIVIDIDO = ToTerm("/");

            RegisterOperators(1, MAS, MENOS);
            RegisterOperators(2, POR, DIVIDIDO);

            #endregion


        }
    }
}

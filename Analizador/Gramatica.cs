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
            var numero = new NumberLiteral("numero");
            var identificador = new IdentifierTerminal("identificador");
            var cadena = new StringLiteral("cadena", "\'");
            var comentario_uno= new CommentTerminal("comentario_uno", "//", "\n", "\r\n");
            var comentario_multi = new CommentTerminal("comentario_multi", "(*", "*)");
            var comentario_multi_ = new CommentTerminal("comentario_multi", "{", "}");
            #endregion

            #region Terminales
            
            //operadores aritmeticos
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var dividir = ToTerm("/");
            var modulo = ToTerm("%");

            //operadores logicos
            var mayor = ToTerm(">");
            var menor = ToTerm("<");

            //asignacion
            var igual = ToTerm("=");

            //negacion
            var negacion = ToTerm("!");

            //agrupacion
            var pizq = ToTerm("(");
            var pder = ToTerm(")");
            var cizq = ToTerm("[");
            var cder = ToTerm("]");

            //simbolos
            var punto = ToTerm(".");
            var dpunto = ToTerm(":");
            var coma = ToTerm(",");
            var ptcoma = ToTerm(";");

            //palabras clave 
            var begin_ = ToTerm("begin");
            var program_ = ToTerm("program");
            var var_ = ToTerm("var");
            var end_ = ToTerm("end");
            var function_ = ToTerm("function");
            var procedure_ = ToTerm("procedure");
            var then_ = ToTerm("then");
            var else_ = ToTerm("else");
            var if_ = ToTerm("if");
            var writeln_ = ToTerm("writeln");

            var exit_ = ToTerm("Exit");

            var integer_ = ToTerm("integer");
            var string_ = ToTerm("string");
            var boolean_ = ToTerm("boolean");
            var real_ = ToTerm("real");


            RegisterOperators(1, mas, menos);
            RegisterOperators(2, por, dividir);

            #endregion




            #region CONFIG
            NonGrammarTerminals.Add(comentario_uno);
            NonGrammarTerminals.Add(comentario_multi);
            NonGrammarTerminals.Add(comentario_multi_);
            #endregion


        }
    }
}

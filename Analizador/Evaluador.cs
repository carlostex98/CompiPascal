using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace CompiPascal.Analizador
{
    class Evaluador
    {
        //constructor
        public Evaluador() { }

        public void analizar_arbol(String entrada)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz_grogram = arbol.Root;

            if (raiz_grogram == null)
            {
                //indica error
            }
            else
            {
                //mandamos a llamar a los metodos de instrucciones
            }



        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;


namespace CompiPascal.Analizador
{
    class Evaluador
    {
        //constructor
        public Evaluador() { }
        private string grafo = "";   //aqui voy a ir guardando todo el codigo en dot
        private int contador = 0;

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
                /*Maestro.Instance.addMessage("Entrada incorrecta");
                foreach (Irony.LogMessage a in arbol.ParserMessages)
                {
                    Error tmp = new Error(
                            a.Location.Line + 1, a.Location.Column + 1, a.Message, Error.tipos.SINTACTICO
                        );
                    Maestro.Instance.addError(tmp);
                }*/
            }
            else
            {
                //mandamos a llamar a los metodos de instrucciones
                //Maestro.Instance.addMessage("Todo correcto");
                _ = this.generarImagen(raiz_grogram); //se usa el simbolo de descarte
            }

        }

        private void getDot(ParseTreeNode raiz)
        {
            grafo = "digraph G {";
            grafo += "nodo0[label=\"" + raiz.ToString() + "\"];\n";
            contador = 1;
            recorrerAST("nodo0", raiz);
            grafo += "}";
        }

        private void recorrerAST(String padre, ParseTreeNode hijos)
        {
            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                string nombreHijo = "nodo" + contador.ToString();
                grafo += nombreHijo + "[label=\"" + hijo.ToString() + "\"];\n";
                grafo += padre + "->" + nombreHijo + ";\n";
                contador++;
                recorrerAST(nombreHijo, hijo);
            }
        }

        public async Task generarImagen(ParseTreeNode raiz)
        {
            this.getDot(raiz);
            //DOT dot = new DOT();
            //BinaryImage img = dot.ToPNG(this.grafo);
            //img.Save("C:\\compiladores2\\AST.png");
            await File.WriteAllTextAsync("C:\\compiladores2\\AST.txt", this.grafo);
        }

        public void evaluarInstrucciones(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 2)
            {
                evaluarInstruccion(ps.ChildNodes[0]);
                evaluarInstrucciones(ps.ChildNodes[0]);
            }
            else 
            {
                evaluarInstruccion(ps.ChildNodes[0]);
            }
        }


        public void evaluarInstruccion(ParseTreeNode ps)
        {
            //aca se leeee
            switch (ps.ChildNodes[0].Term.Name)
            {
                case "funcion":
                    //llama a funcion
                    break;
                case "programa":
                    //evaluamos la sentencia programa, solo hace un print a la consola virtual
                    ParseTreeNode aux = ps.ChildNodes[0].ChildNodes[1];
                    Maestro.Instance.addOutput("PROGRAM " + aux.Token.ValueString);
                    break;
                case "declaracion":
                    //registramos en los simbolos, en este caso el contexto general
                    break;
                case "procedimiento":
                    // 
                    break;
                case "main_":
                    //
                    break;
            }
        }

        public void evalFuncion()
        {
            //encargado de cuardar la funcion
        }

        public void evalDeclaracion()
        {

        }

        public void evalProcedimiento()
        {

        }

        public void evalMain()
        {

        }




    }
}

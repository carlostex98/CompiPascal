using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompiPascal.General;
using CompiPascal.Instrucciones;
using CompiPascal.TablaSimbolos;


namespace CompiPascal.Analizador
{
    class Evaluador
    {
        //constructor
        public Evaluador() { }
        private string grafo = "";   //aqui voy a ir guardando todo el codigo en dot
        private int contador = 0;
        LinkedList<Instruccion> instrucciones_globales = new LinkedList<Instruccion>();
        

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

                TSimbolo global = new TSimbolo(null);
                //mandar a llamar ejecutar con try catch

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


        //metodo maestro
        public void evaluarInstrucciones(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 2)
            {
                this.instrucciones_globales.AddLast(evaluarInstruccion(ps.ChildNodes[0]));
                evaluarInstrucciones(ps.ChildNodes[1]);
            }
            else 
            {
                //hace push de la isntruccion
                this.instrucciones_globales.AddLast(evaluarInstruccion(ps.ChildNodes[0]));

            }
        }


        public Instruccion evaluarInstruccion(ParseTreeNode ps)
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
                    //                      main           listaInstr    
                    ParseTreeNode auxx = ps.ChildNodes[0].ChildNodes[1];
                    return new MainProgram(null);
            }

            return null;
        }

        public LinkedList<Instruccion> evaluar_general(ParseTreeNode ps)
        {
            //recibimos lista instr

            if (ps.ChildNodes.Count == 3)
            {
                //multiples
                LinkedList<Instruccion> temporal = new LinkedList<Instruccion>();
                temporal.AddLast(unitaria(ps.ChildNodes[0]));

                LinkedList<Instruccion> t1 = new LinkedList<Instruccion>(evaluar_general(ps.ChildNodes[2]));
                foreach(Instruccion item in t1)
                {
                    temporal.AddLast(item);
                }
                return temporal;
            }
            else
            {
                //instruccion unitaria
                LinkedList<Instruccion> temporal = new LinkedList<Instruccion>();
                temporal.AddLast(unitaria(ps.ChildNodes[0]));
                return temporal;
            }

            //return null;
        }

        public Instruccion unitaria(ParseTreeNode ps)
        {
            //recibo instr normal
            ParseTreeNode aux = ps.ChildNodes[0];
            if (aux.Term.Name == "print_")
            {
                //write ln
            }

            return null;
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

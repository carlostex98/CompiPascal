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

        public enum tipo
        {
            STRING,
            INTEGER,
            REAL,
            BOOLEAN
        };


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
                //Maestro.Instance.addMessage("Entrada incorrecta");
                foreach (Irony.LogMessage a in arbol.ParserMessages)
                {
                    System.Diagnostics.Debug.WriteLine(a.Message, a.Location.Line, a.Location.Column);
                    System.Diagnostics.Debug.WriteLine(a.Location.Line);
                    System.Diagnostics.Debug.WriteLine(a.Location.Column);
                    System.Diagnostics.Debug.WriteLine("-----");
                }
                System.Diagnostics.Debug.WriteLine("compilado con errores");
            }
            else
            {
                //mandamos a llamar a los metodos de instrucciones
                //Maestro.Instance.addMessage("Todo correcto");
                //_ = this.generarImagen(raiz_grogram); //se usa el simbolo de descarte

                this.evaluarInstrucciones(raiz_grogram.ChildNodes[0]);

                TSimbolo global = new TSimbolo(null);
                //mandar a llamar ejecutar con try catch

                foreach(Instruccion t in instrucciones_globales)
                {
                    //ejecucion maestra
                    _ = t.ejecutar(global);
                }

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
            //System.Diagnostics.Debug.WriteLine(ps.ChildNodes.Count); 

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
                    //llama a funcion(declaracion)
                    return evalFuncDec(ps.ChildNodes[0]);
                    break;
                case "programa":
                    //evaluamos la sentencia programa, solo hace un print a la consola virtual
                    ParseTreeNode aux = ps.ChildNodes[1];
                    Maestro.Instance.addOutput("PROGRAM " + aux.Token.ValueString);
                    break;
                case "declaracion":
                    //registramos en los simbolos, en este caso el contexto general
                    ParseTreeNode mx = ps.ChildNodes[0];
                    return declaracionVariable(mx);
                    break;
                case "procedimiento":
                    // 
                    break;
                case "main":
                    //                      main           listaInstr    
                    ParseTreeNode auxx = ps.ChildNodes[0].ChildNodes[1];
                    
                    return new MainProgram(evaluar_general(auxx));
            }

            return null;
        }

        public Instruccion evalFuncDec(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 8)
            {
                return new Funcion(ps.ChildNodes[1].Token.ValueString, evaluar_general(ps.ChildNodes[5]), null);
            }


            return null;
        }

        public Instruccion declaracionVariable(ParseTreeNode ps)
        {
            if (ps.ChildNodes[0].Term.Name == "var")
            {
                LinkedList<string> nombres = listaVar(ps.ChildNodes[1]);
                
                //variable
                if (ps.ChildNodes.Count == 5)
                {
                    //System.Diagnostics.Debug.WriteLine(ps.ChildNodes[3].ChildNodes[0].Term.Name);
                    return new Declaracion(nombres, calcularTipo(ps.ChildNodes[3].ChildNodes[0].Term.Name), (Operacion)null );
                }
                else{
                    //variable inicializada
                    return new Declaracion(nombres, calcularTipo(ps.ChildNodes[3].ChildNodes[0].Term.Name), evalOpr(ps.ChildNodes[5]));
                }
            }


            return null;
        }


        public Simbolo.tipo calcularTipo(string v)
        {
            if (v == "string")
            {
                return Simbolo.tipo.STRING;
            } 
            else if (v == "integer")
            {
                return Simbolo.tipo.INTEGER;
            }
            else if (v == "real")
            {
                return Simbolo.tipo.REAL;
            }
            else if (v == "boolean")
            {
                return Simbolo.tipo.BOOLEAN;
            }
            return Simbolo.tipo.STRING;
        }



        public LinkedList<string> listaVar(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 3)
            {
                LinkedList<string> temporal = new LinkedList<string>();
                temporal.AddLast(ps.ChildNodes[0].Token.ValueString);

                LinkedList<string> t1 = new LinkedList<string>(listaVar(ps.ChildNodes[2]));
                foreach(string item in t1)
                {
                    temporal.AddLast(item);
                }
                return temporal;

            }
            else
            {
                LinkedList<string> temporal = new LinkedList<string>();
                temporal.AddLast(ps.ChildNodes[0].Token.ValueString);
                return temporal;
            }
        }


        public LinkedList<Instruccion> evaluar_general(ParseTreeNode ps)
        {
            //recibimos lista instr

            if (ps.ChildNodes.Count == 2)
            {
                //multiples
                LinkedList<Instruccion> temporal = new LinkedList<Instruccion>();
                temporal.AddLast(unitaria(ps.ChildNodes[0]));

                LinkedList<Instruccion> t1 = new LinkedList<Instruccion>(evaluar_general(ps.ChildNodes[1]));
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
            if (aux.Term.Name == "print")
            {
                return new Writeln(evalOpr(aux.ChildNodes[2]));
            } 
            else if (aux.Term.Name == "if_then")
            {
                //evalualos la expresion
                //System.Diagnostics.Debug.WriteLine(aux.ChildNodes.Count);
                return evalIf(aux);
            }
            else if (aux.Term.Name == "redefinir")
            {
                //evalualos la expresion
                //System.Diagnostics.Debug.WriteLine(aux.ChildNodes[0].Token.ValueString);
                //return evalIf(aux);
                return new Asignacion(aux.ChildNodes[0].Token.ValueString, evalOpr(aux.ChildNodes[3]));
            }
            else if (aux.Term.Name == "while_do")
            {
                return evalWhile(aux);
            }
            else if (aux.Term.Name == "for_do")
            {
                return evalFor(aux);
            }
            else if (aux.Term.Name == "repeat_until")
            {
                return new RepeatUntil(evaluar_general(aux.ChildNodes[1]), evalOpr(aux.ChildNodes[3]));
            }
            else if (aux.Term.Name == "function_call")
            {
                //return new CallFuncion(,);
                return evalCall(aux);
            }

            return null;
        }

        
        public Instruccion evalCall(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 4)
            {
                return new CallFuncion(ps.ChildNodes[0].Token.ValueString, null);
            }

            return null;
        }


        public Instruccion evalFor(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 9)
            {

                Asignacion t1 = new Asignacion(ps.ChildNodes[1].Token.ValueString, evalOpr(ps.ChildNodes[4]));
                LinkedList<Instruccion> t3 = new LinkedList<Instruccion>();
                t3.AddLast(unitaria(ps.ChildNodes[8]));
                return new ForDo(t1, evalOpr(ps.ChildNodes[6]), t3);
            }
            else
            {
                Asignacion t1 = new Asignacion(ps.ChildNodes[1].Token.ValueString, evalOpr(ps.ChildNodes[4]));
                return new ForDo(t1, evalOpr(ps.ChildNodes[6]), evaluar_general(ps.ChildNodes[9]));
            }

            //return null;
        }


        public Instruccion evalWhile(ParseTreeNode ps)
        {

            
            return new While(evalOpr(ps.ChildNodes[1]), evaluar_general(ps.ChildNodes[4]));

            //return null;
        }
        public Instruccion evalIf(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 4)
            {
                
                return new If_st(evalOpr(ps.ChildNodes[1]), evalBloque(ps.ChildNodes[3]));
            }
            else
            {
                //tiene else
                
                return new If_st(evalOpr(ps.ChildNodes[1]), evalBloque(ps.ChildNodes[3]), evalBloque(ps.ChildNodes[5]));

            }

        }

        public LinkedList<Instruccion> evalBloque(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 4)
            {
                return evaluar_general(ps.ChildNodes[1]);
            }
            else
            {
                LinkedList<Instruccion> t = new LinkedList<Instruccion>();
                t.AddLast(unitaria(ps.ChildNodes[0]));
                return t;
            }

        }


        public Operacion evalOpr(ParseTreeNode ps)
        {
            //evaluamos la expresion
            if (ps.ChildNodes.Count == 3)
            {
                //es op + signo + op
                string opr_tipo = ps.ChildNodes[1].Term.Name;
                if (opr_tipo == "+")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.SUMA);
                }
                else if (opr_tipo == "-")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.RESTA);
                }
                else if (opr_tipo == "*")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.MULTIPLICACION);
                }
                else if (opr_tipo == "/")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.DIVISION);
                }
                else if (opr_tipo == "%")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.MODULO);
                }
                else if (opr_tipo == ">")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.MAYOR_QUE);
                }
                else if (opr_tipo == "<")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.MENOR_QUE);
                }
                else if (opr_tipo == "<")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.MENOR_QUE);
                }
                else if (opr_tipo == "and")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.YY);
                }
                else if (opr_tipo == "or")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.OO);
                }

            }
            else if (ps.ChildNodes.Count == 4)
            {
                //mayor igual, menor igual, igual igual
                string opr_tipo = ps.ChildNodes[1].Term.Name;

                if (opr_tipo == ">")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[3]), Operacion.Tipo_operacion.MAYOR_I);
                }
                else if (opr_tipo == "<")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[3]), Operacion.Tipo_operacion.MENOR_I);
                }
                else if (opr_tipo == "=")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[3]), Operacion.Tipo_operacion.EQUIVALENCIA);
                }

            }
            else if (ps.ChildNodes.Count == 2)
            {
                //dos, negacion o negativo
                string opr_tipo = ps.ChildNodes[0].Term.Name;
                if (opr_tipo == "!")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[1]), null, Operacion.Tipo_operacion.NEGACION);
                } 
                else if (opr_tipo == "-")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[1]), null, Operacion.Tipo_operacion.NEGATIVO);
                }
                
            }
            else
            {
                //es valor
                return new Operacion(valor_unico(ps.ChildNodes[0]));
            }
            return null;
        }

        public Operacion valor_unico(ParseTreeNode ps)
        {
            //es cadena, numero
            if (ps.ChildNodes.Count == 3)
            {
                //parentesis
                return new Operacion(evalOpr(ps.ChildNodes[1]));
            }
            else
            {
                //es primitivo
                ParseTreeNode aux = ps.ChildNodes[0]; //
                if (aux.Term.Name == "numero")
                {
                    Primitivo p = new Primitivo(Primitivo.tipo_val.INT, aux.Token.Value);
                    return new Operacion(p);
                }
                else if (aux.Term.Name == "cadena")
                {
                    Primitivo p = new Primitivo(Primitivo.tipo_val.CADENA, aux.Token.Value);
                    return new Operacion(p);
                }
                else if (aux.Term.Name == "true")
                {
                    Primitivo p = new Primitivo(Primitivo.tipo_val.CADENA, (object)true );
                    return new Operacion(p);
                }
                else if (aux.Term.Name == "false")
                {
                    Primitivo p = new Primitivo(Primitivo.tipo_val.CADENA, (object)false);
                    return new Operacion(p);
                }
                else if (aux.Term.Name == "identificador")
                {
                    //Primitivo p = new Primitivo(Primitivo.tipo_val.CADENA, (object)false);
                    Acceso a = new Acceso(aux.Token.Value.ToString());
                    return new Operacion(a);
                }

            }

            return null;
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

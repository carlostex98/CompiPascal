﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using CompiPascal.General;
using CompiPascal.Instrucciones;
using CompiPascal.TablaSimbolos;
using System.Diagnostics;

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
                    //System.Diagnostics.Debug.WriteLine(a.Message, a.Location.Line, a.Location.Column);
                    //System.Diagnostics.Debug.WriteLine(a.Location.Line);
                    //System.Diagnostics.Debug.WriteLine(a.Location.Column);
                    //System.Diagnostics.Debug.WriteLine("-----");
                    
                    Maestro.Instance.addError(new Error(a.Location.Line, a.Location.Column, a.Message, Error.Tipo_error.LEXICO) );
                }
                Maestro.Instance.addOutput("No se puede ejecutar, ver tabla de errores");
            }
            else
            {
                //mandamos a llamar a los metodos de instrucciones
                //Maestro.Instance.addMessage("Todo correcto");
                _ = this.generarImagen(raiz_grogram);

                

                this.evaluarInstrucciones(raiz_grogram.ChildNodes[0]);

                TSimbolo global = new TSimbolo(null);
                //mandar a llamar ejecutar con try catch

                foreach(Instruccion t in instrucciones_globales)
                {
                    try
                    {
                        _ = t.ejecutar(global);
                    }
                    catch (Error x)
                    {
                        Maestro.Instance.addError(x);
                        Maestro.Instance.addOutput(x.getDescripcion());
                        //System.Diagnostics.Debug.WriteLine("eeeeeee");
                    }
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
                    ParseTreeNode aux = ps.ChildNodes[0].ChildNodes[1];
                    //Maestro.Instance.addOutput("PROGRAM " + aux.Token.ValueString);
                    Operacion op = new Operacion(new Primitivo(Primitivo.tipo_val.CADENA, (object)("PROGRAM " + aux.Token.ValueString)));
                    LinkedList<Operacion> f = new LinkedList<Operacion>();
                    f.AddLast(op);
                    return new Writeln(f,0, 0);
                    break;
                case "declaracion":
                    //registramos en los simbolos, en este caso el contexto general
                    ParseTreeNode mx = ps.ChildNodes[0];
                    return declaracionVariable(mx);
                    break;
                case "procedimiento":
                    // usa lo mismo que la funcion
                    return evalProdDec(ps.ChildNodes[0]);
                    break;

                case "graficar_ts":
                    // usa lo mismo que la funcion
                    return new GraficarTS();
                    break;
                case "main":
                    //                      main           listaInstr    
                    ParseTreeNode auxx = ps.ChildNodes[0].ChildNodes[1];
                    
                    return new MainProgram(evaluar_general(auxx), ps.ChildNodes[0].ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].ChildNodes[0].Token.Location.Column);
            }

            return null;
        }

        public Instruccion evlDecFunc(ParseTreeNode ps)
        {
            if (ps.Term.Name == "declaracion")
            {
                return declaracionVariable(ps.ChildNodes[0]);
            }

            return null;
        }

        public Instruccion declaracionVariable(ParseTreeNode ps)
        {
            if (ps.ChildNodes[0].Term.Name == "var")
            {
                return new MultiDeclaracion(declaracionUno(ps.ChildNodes[1]));
            }
            else
            {
                return new MultiDeclaracion(declaracionUno(ps.ChildNodes[1]));
            }

            //return null;
        }

        public LinkedList<Declaracion> declaracionUno(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 5)
            {
                //sin definir con lista
                //uno inicializado y lista
                int ln = ps.ChildNodes[1].Token.Location.Line;
                int cl = ps.ChildNodes[1].Token.Location.Column;
                LinkedList<Declaracion> temporal = new LinkedList<Declaracion>();
                temporal.AddLast(new Declaracion(listaVar(ps.ChildNodes[0]), calcularTipo(ps.ChildNodes[2].ChildNodes[0].Term.Name), (Operacion)null, ln, cl));
                LinkedList<Declaracion> t1 = new LinkedList<Declaracion>(declaracionUno(ps.ChildNodes[4]));
                foreach (Declaracion s in t1)
                {
                    temporal.AddLast(s);
                }
                return temporal;

            } 
            else if (ps.ChildNodes.Count == 4)
            {
                //sin definir sin lista

                int ln = ps.ChildNodes[1].Token.Location.Line;
                int cl = ps.ChildNodes[1].Token.Location.Column;
                LinkedList<Declaracion> temporal = new LinkedList<Declaracion>();
                temporal.AddLast(new Declaracion(listaVar(ps.ChildNodes[0]), calcularTipo(ps.ChildNodes[2].ChildNodes[0].Term.Name), (Operacion)null, ln, cl));
                return temporal;
            }
            else if (ps.ChildNodes.Count == 7)
            {
                //uno inicializado y lista
                int ln = ps.ChildNodes[1].Token.Location.Line;
                int cl = ps.ChildNodes[1].Token.Location.Column;
                LinkedList<Declaracion> temporal = new LinkedList<Declaracion>();
                temporal.AddLast(new Declaracion(listaVar(ps.ChildNodes[0]), calcularTipo(ps.ChildNodes[2].ChildNodes[0].Term.Name), evalOpr(ps.ChildNodes[4]), ln, cl));
                LinkedList<Declaracion> t1 = new LinkedList<Declaracion>(declaracionUno(ps.ChildNodes[6]));
                foreach (Declaracion s in t1)
                {
                    temporal.AddLast(s);
                }
                return temporal;

            }
            else if (ps.ChildNodes.Count == 6)
            {
                //uno, inicializado
                int ln = ps.ChildNodes[1].Token.Location.Line;
                int cl = ps.ChildNodes[1].Token.Location.Column;
                LinkedList<Declaracion> temporal = new LinkedList<Declaracion>();
                temporal.AddLast(new Declaracion(listaVar(ps.ChildNodes[0]), calcularTipo(ps.ChildNodes[2].ChildNodes[0].Term.Name), evalOpr(ps.ChildNodes[4]), ln, cl));
                return temporal;
            }

            return null;
        }


        public Instruccion evalFuncDec(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 12)
            {

                LinkedList<Instruccion> tmp = new LinkedList<Instruccion>(evaluar_general(ps.ChildNodes[9]));
                if (evlDecFunc(ps.ChildNodes[7]) != null)
                {
                    tmp.AddFirst(evlDecFunc(ps.ChildNodes[7]));
                }
                //tmp.AddFirst(evlDecFunc(ps.ChildNodes[7]));

                return new Funcion(ps.ChildNodes[1].Token.ValueString, tmp, null, FuncionDato.tipoF.FUNCION, evaluarRet(ps.ChildNodes[5].ChildNodes[0].Token.ValueString),ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
            }
            else
            {
                //funcion con parametros
                //System.Diagnostics.Debug.WriteLine(ps.ChildNodes[6].ChildNodes[0].Token.ValueString);
                LinkedList<Instruccion> tmp = new LinkedList<Instruccion>(evaluar_general(ps.ChildNodes[10]));
                if (evlDecFunc(ps.ChildNodes[8]) != null)
                {
                    tmp.AddFirst(evlDecFunc(ps.ChildNodes[8]));
                }

                return new Funcion(ps.ChildNodes[1].Token.ValueString, tmp, evalParamDec(ps.ChildNodes[3]), FuncionDato.tipoF.FUNCION, evaluarRet(ps.ChildNodes[6].ChildNodes[0].Token.ValueString), ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
            }

            //return null;
        }

        public FuncionDato.tipoR evaluarRet(string v)
        {
            if (v == "string")
            {
                return FuncionDato.tipoR.STRING;
            }
            else if (v == "integer")
            {
                return FuncionDato.tipoR.INTEGER;
            }
            else if (v == "real")
            {
                return FuncionDato.tipoR.REAL;
            }
            else if (v == "boolean")
            {
                return FuncionDato.tipoR.BOOLEAN;
            }
            else if (v == "void")
            {
                return FuncionDato.tipoR.VOID;
            }
            return FuncionDato.tipoR.STRING;
        }


        public Instruccion evalProdDec(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 10)
            {
                LinkedList<Instruccion> tmp = new LinkedList<Instruccion>(evaluar_general(ps.ChildNodes[7]));
                if (evlDecFunc(ps.ChildNodes[5])!=null)
                {
                    tmp.AddFirst(evlDecFunc(ps.ChildNodes[5]));
                }

                return new Funcion(ps.ChildNodes[1].Token.ValueString, tmp, null, FuncionDato.tipoF.PROCEDIMINETO, FuncionDato.tipoR.VOID, ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
            }
            else
            {
                LinkedList<Instruccion> tmp = new LinkedList<Instruccion>(evaluar_general(ps.ChildNodes[8]));
                if (evlDecFunc(ps.ChildNodes[6]) != null)
                {
                    tmp.AddFirst(evlDecFunc(ps.ChildNodes[6]));
                }
                //funcion con parametros
                return new Funcion(ps.ChildNodes[1].Token.ValueString, tmp, evalParamDec(ps.ChildNodes[3]), FuncionDato.tipoF.PROCEDIMINETO, FuncionDato.tipoR.VOID,ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
            }

            //return null;
        }


        public LinkedList<Declaracion> evalParamDec(ParseTreeNode ps)
        {

            if (ps.ChildNodes.Count == 5)
            {

                int ln = ps.ChildNodes[2].ChildNodes[0].Token.Location.Line;
                int cl = ps.ChildNodes[2].ChildNodes[0].Token.Location.Column;

                // mas listas
                Declaracion ts = new Declaracion(listaVar(ps.ChildNodes[0]), calcularTipo(ps.ChildNodes[2].ChildNodes[0].Token.ValueString), null, ln, cl);
                LinkedList<Declaracion> temporal = new LinkedList<Declaracion>();
                temporal.AddLast(ts);

                LinkedList<Declaracion> t1 = new LinkedList<Declaracion>(evalParamDec(ps.ChildNodes[4]));

                foreach (Declaracion t in t1)
                {
                    temporal.AddLast(t);
                }

                return temporal;
            }
            else
            {
                int ln = ps.ChildNodes[2].ChildNodes[0].Token.Location.Line;
                int cl = ps.ChildNodes[2].ChildNodes[0].Token.Location.Column;

                //un elemento
                Declaracion ts = new Declaracion(listaVar(ps.ChildNodes[0]), calcularTipo(ps.ChildNodes[2].ChildNodes[0].Token.ValueString), null, ln,cl);
                LinkedList<Declaracion> temporal = new LinkedList<Declaracion>();
                temporal.AddLast(ts);
                return temporal;
            }

            
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
                return new Writeln(prtParam(aux.ChildNodes[2]), aux.ChildNodes[0].Token.Location.Line, aux.ChildNodes[0].Token.Location.Column);
            }
            else if (aux.Term.Name == "print2")
            {
                //evalualos la expresion
                //System.Diagnostics.Debug.WriteLine(aux.ChildNodes.Count);
                return new WriteNor(prtParam(aux.ChildNodes[2]), aux.ChildNodes[0].Token.Location.Line, aux.ChildNodes[0].Token.Location.Column);
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
                return new Asignacion(aux.ChildNodes[0].Token.ValueString, evalOpr(aux.ChildNodes[3]), aux.ChildNodes[0].Token.Location.Line, aux.ChildNodes[0].Token.Location.Column);
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
                return new RepeatUntil(evaluar_general(aux.ChildNodes[1]), evalOpr(aux.ChildNodes[3]), aux.ChildNodes[0].Token.Location.Line, aux.ChildNodes[0].Token.Location.Column);
            }
            else if (aux.Term.Name == "function_call")
            {
                return evalCall(aux);
            }
            else if (aux.Term.Name == "Exit_")
            {
                return new Exit(evalOpr(aux.ChildNodes[2]), aux.ChildNodes[0].Token.Location.Line, aux.ChildNodes[0].Token.Location.Column);
            }
            else if (aux.Term.Name == "break")
            {
                return new Break(aux.Token.Location.Line, aux.Token.Location.Column);
            }
            else if (aux.Term.Name == "continue")
            {
                return new Continue(aux.Token.Location.Line, aux.Token.Location.Column);
            }
            else if (aux.Term.Name == "declaracion")
            {
                //ParseTreeNode mx = ps.ChildNodes[0];
                return declaracionVariable(aux);
            }
            else if (aux.Term.Name == "cases")
            {
                
                return evalSwitch(aux);
            }
            else if (aux.Term.Name == "graficar_ts")
            {

                return new GraficarTS();
            }

            return null;
        }



        public LinkedList<Operacion> prtParam(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 3)
            {
                //uno y lista
                LinkedList<Operacion> temporal = new LinkedList<Operacion>();
                temporal.AddLast(evalOpr(ps.ChildNodes[0]));

                LinkedList<Operacion> t1 = new LinkedList<Operacion>(prtParam(ps.ChildNodes[2]));
                foreach (Operacion s in t1)
                {
                    temporal.AddLast(s);
                }
                return temporal;
            }
            else
            {
                //solo uno
                LinkedList<Operacion> temporal = new LinkedList<Operacion>();
                temporal.AddLast(evalOpr(ps.ChildNodes[0]));
                return temporal;
            }

            //return null;
        }


        public Instruccion evalSwitch(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 6)
            {
                return new Instrucciones.Switch(evalOpr(ps.ChildNodes[1]), evalCaso(ps.ChildNodes[3]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);
            }
            else
            {
                //tiene else
                return new Instrucciones.Switch(evalOpr(ps.ChildNodes[1]), evalCaso(ps.ChildNodes[3]), evaluar_general(ps.ChildNodes[6]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);
            }
            //return null;
        }


        public LinkedList<Case> evalCaso(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 7)
            {
                LinkedList<Case> temporal = new LinkedList<Case>();
                temporal.AddLast(new Case(evalOpr(ps.ChildNodes[0]), evaluar_general(ps.ChildNodes[3]), ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column));

                LinkedList<Case> t1 = new LinkedList<Case>(evalCaso(ps.ChildNodes[6]));
                foreach (Case item in t1)
                {
                    temporal.AddLast(item);
                }

                return temporal;
                // uno y mas
            }
            else
            {
                LinkedList<Case> temporal = new LinkedList<Case>();
                temporal.AddLast(new Case(evalOpr(ps.ChildNodes[0]), evaluar_general(ps.ChildNodes[3]), ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column));
                return temporal;
                //solo uno
            }

            //return null;
        }

        
        public Instruccion evalCall(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 3)
            {
                return new CallFuncion(ps.ChildNodes[0].Token.ValueString, null, ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);
            }
            else
            {
                //con parametros
                return new CallFuncion(ps.ChildNodes[0].Token.ValueString, evalParametrosCall(ps.ChildNodes[2]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);
            }

            //return null;
        }

        public LinkedList<Operacion> evalParametrosCall(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 3)
            {
                LinkedList<Operacion> temporal = new LinkedList<Operacion>();
                temporal.AddLast(evalOpr(ps.ChildNodes[0]));

                LinkedList<Operacion> t1 = new LinkedList<Operacion>(evalParametrosCall(ps.ChildNodes[2]));
                foreach (Operacion item in t1)
                {
                    temporal.AddLast(item);
                }
                return temporal;

            }
            else
            {
                LinkedList<Operacion> temporal = new LinkedList<Operacion>();
                temporal.AddLast(evalOpr(ps.ChildNodes[0]));
                return temporal;
            }


            //return null;
        }



        public Instruccion evalFor(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 9)
            {

                Asignacion t1 = new Asignacion(ps.ChildNodes[1].Token.ValueString, evalOpr(ps.ChildNodes[4]), ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
                LinkedList<Instruccion> t3 = new LinkedList<Instruccion>();
                t3.AddLast(unitaria(ps.ChildNodes[8]));
                return new ForDo(t1, evalOpr(ps.ChildNodes[6]), t3, ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
            }
            else
            {
                Asignacion t1 = new Asignacion(ps.ChildNodes[1].Token.ValueString, evalOpr(ps.ChildNodes[4]), ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
                return new ForDo(t1, evalOpr(ps.ChildNodes[6]), evaluar_general(ps.ChildNodes[9]), ps.ChildNodes[1].Token.Location.Line, ps.ChildNodes[1].Token.Location.Column);
            }

            //return null;
        }


        public Instruccion evalWhile(ParseTreeNode ps)
        {

            
            return new While(evalOpr(ps.ChildNodes[1]), evaluar_general(ps.ChildNodes[4]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);

            //return null;
        }
        public Instruccion evalIf(ParseTreeNode ps)
        {

            if (ps.ChildNodes.Count == 4)
            {
                //solo if
                return new If_st(evalOpr(ps.ChildNodes[1]), evalBloque(ps.ChildNodes[3]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);
            }
            else if(ps.ChildNodes.Count == 6)
            {
                //tiene else
                
                return new If_st(evalOpr(ps.ChildNodes[1]), evalBloque(ps.ChildNodes[3]), evalBloque(ps.ChildNodes[5]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);

            }
            else if (ps.ChildNodes.Count == 5)
            {
                //if -else if-

                return new If_st(evalOpr(ps.ChildNodes[1]), evalBloque(ps.ChildNodes[3]), evalElseIf(ps.ChildNodes[4]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);

            }

            else if (ps.ChildNodes.Count == 7)
            {
                //if -else if- else

                return new If_st(evalOpr(ps.ChildNodes[1]), evalBloque(ps.ChildNodes[3]), evalElseIf(ps.ChildNodes[4]), evalBloque(ps.ChildNodes[6]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column);

            }

            return null;

        }


        public LinkedList<MultiElse> evalElseIf(ParseTreeNode ps)
        {

            if (ps.ChildNodes.Count == 5)
            {
                LinkedList<MultiElse> temporal = new LinkedList<MultiElse>();
                temporal.AddLast(new MultiElse(evalOpr(ps.ChildNodes[2]), evalBloque(ps.ChildNodes[3])));
                LinkedList<MultiElse> t1 = new LinkedList<MultiElse>(evalElseIf(ps.ChildNodes[4]));
                foreach (MultiElse item in t1)
                {
                    temporal.AddLast(item);
                }
                return temporal;
            }
            else
            {
                LinkedList<MultiElse> temporal = new LinkedList<MultiElse>();
                temporal.AddLast(new MultiElse(evalOpr(ps.ChildNodes[2]), evalBloque(ps.ChildNodes[3])));
                return temporal;
            }

            return null;
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
                else if (opr_tipo == "AND")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.YY);
                }
                else if (opr_tipo == "OR")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.OO);
                }
                else if (opr_tipo == "=")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[2]), Operacion.Tipo_operacion.EQUIVALENCIA);
                }

            }
            else if (ps.ChildNodes.Count == 4)
            {
                //mayor igual, menor igual, igual igual
                string opr_tipo = ps.ChildNodes[1].Term.Name;
                string op2 = ps.ChildNodes[2].Term.Name;

                if (opr_tipo == "<" && op2 == ">")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[3]), Operacion.Tipo_operacion.DIFERENCIA);
                }

                if (opr_tipo == ">")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[3]), Operacion.Tipo_operacion.MAYOR_I);
                }
                else if (opr_tipo == "<")
                {
                    return new Operacion(evalOpr(ps.ChildNodes[0]), evalOpr(ps.ChildNodes[3]), Operacion.Tipo_operacion.MENOR_I);
                }
                

            }
            else if (ps.ChildNodes.Count == 2)
            {
                //dos, negacion o negativo
                string opr_tipo = ps.ChildNodes[0].Term.Name;
                if (opr_tipo == "not")
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
                    Primitivo p = new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)true );
                    return new Operacion(p);
                }
                else if (aux.Term.Name == "false")
                {
                    Primitivo p = new Primitivo(Primitivo.tipo_val.BOOLEANO, (object)false);
                    return new Operacion(p);
                }
                else if (aux.Term.Name == "identificador")
                {
                    //Primitivo p = new Primitivo(Primitivo.tipo_val.CADENA, (object)false);
                    Acceso a = new Acceso(aux.Token.Value.ToString(), aux.Token.Location.Line, aux.Token.Location.Column);
                    return new Operacion(a);
                }
                else if (aux.Term.Name == "function_call")
                {
                    //Primitivo p = new Primitivo(Primitivo.tipo_val.CADENA, (object)false);
                    return evalCallOp(aux);
                }

            }

            return null;
        }


        public Operacion evalCallOp(ParseTreeNode ps)
        {
            if (ps.ChildNodes.Count == 3)
            {
                return new Operacion(new CallFuncion(ps.ChildNodes[0].Token.ValueString, null, ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column));
            }
            else
            {
                //con parametros
                return new Operacion(new CallFuncion(ps.ChildNodes[0].Token.ValueString, evalParametrosCall(ps.ChildNodes[2]), ps.ChildNodes[0].Token.Location.Line, ps.ChildNodes[0].Token.Location.Column));
            }
        }


 
       



    }
}

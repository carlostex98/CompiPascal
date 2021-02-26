﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace CompiPascal.General
{

    

    public sealed class Maestro
    {
        private string output = "";
        private string mensajes = "";
        private static readonly Maestro instance = new Maestro();
        private LinkedList<Error> errores = new LinkedList<Error>();
        private string grafo = "";   //aqui voy a ir guardando todo el codigo en dot
        private int contador = 0;

        static Maestro() { }
        private Maestro() { }
        public static Maestro Instance
        {
            get
            {
                return instance;
            }
        }

        public void addError(Error error)
        {
            this.errores.AddLast(error);
        }

        public LinkedList<Error> getErrores()
        {
            return this.errores;
        }

        public void addMessage(string mensaje)
        {
            this.mensajes += "\n" + mensaje;
        }
        public string getMessages()
        {
            return this.mensajes;
        }

        public void addOutput(string mensaje)
        {
            this.output += "\n" + mensaje;
        }
        public string getOutput()
        {
            return this.output;
        }


        public void clear()
        {
            this.output = "";
            this.mensajes = "";
            //this.instrucciones = new LinkedList<NodoAST>();
            this.grafo = "";
            this.contador = 0;
            this.errores = new LinkedList<Error>();
            //this.funciones = new Dictionary<string, Funcion>();
            //this.agregarNativas();  //agregamos las funciones nativas...
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

        public void ejecutarPrograma()
        {
            /*
             recorre la lista de isntrucciones con un for each, cada instruccion detecta los errores
            y si existe uno hará un throw, dentro del for aach hay un try catch para capturar los eerores dentro de la ejecucion
             */


        }



    }
}
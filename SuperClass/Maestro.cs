using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.Analizador;

namespace CompiPascal.SuperClass
{
    public sealed  class Maestro
    {
        private string output = "";
        private string mensajes = "";
        private LinkedList<AbsNodo> instrucciones = new LinkedList<AbsNodo>();
        private LinkedList<Error> errores = new LinkedList<Error>();
        private Dictionary<string, Funcion> funciones = new Dictionary<string, Funcion>();   //aqui guardo mis funciones

        private static readonly Maestro instance = new Maestro();

        private string grafo = "";   //aqui voy a ir guardando todo el codigo en dot
        private int contador = 0;     //identificador incrementable de cada nodo

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

        public Dictionary<string, Funcion> getFunciones()
        {
            return this.funciones;
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
            this.instrucciones = new LinkedList<AbsNodo>();
            this.grafo = "";
            this.contador = 0;
            this.errores = new LinkedList<Error>();
            this.funciones = new Dictionary<string, Funcion>();
            this.agregarNativas();  //agregamos las funciones nativas...
        }

        public void addInstruction(AbsNodo nodo)
        {
            this.instrucciones.AddLast(nodo);
        }

        public int getCantidad()
        {
            return this.instrucciones.Count;
        }

        public void ejecutar()
        {
            if (this.errores.Count > 0)
            {
                foreach (Error e in errores)
                {
                    this.addOutput(e.getDescripcion() + "-> ln: " + e.getLinea() + ", col: " + e.getColumna());
                }
            }
            else
            {
                foreach (AbsNodo nodo in instrucciones)
                {
                    //nodo.ejecutar();
                }
            }

        }

        public bool addFunction(Funcion tmp)
        {
            string name = tmp.getNombre().ToLower();
            if (!this.funciones.ContainsKey(name))
            {
                //significa que ya hay una bajo ese nombre!
                this.funciones.Add(name, tmp);
                return true;
            }
            return false;

        }

        public Funcion getFuncion(string id)
        {
            string name = id.ToLower();
            if (this.funciones.ContainsKey(name))
            {
                //significa que ya hay una bajo ese nombre!
                Funcion val;
                this.funciones.TryGetValue(name, out val);
                return val;

            }
            return null;
        }


        private void agregarNativas()
        {
            LinkedList<AbsNodo> nativas = new LinkedList<AbsNodo>();

            nativas.AddLast(new Writeln(0, 0)); //agrego la función nativa Writeln


            foreach (AbsNodo tmp in nativas)
            {
                tmp.ejecutar(); //al ejecutarlas, las agregamos a la lista de funciones :)
            }
        }


    }
}

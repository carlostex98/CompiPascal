using System;
using System.Collections.Generic;
using System.Text;
using CompiPascal.TablaSimbolos;
using CompiPascal.General;

namespace CompiPascal.Instrucciones
{
    class ForDo:Instruccion
    {
        private LinkedList<Instruccion> listaInstrucciones;
        private Operacion val_limite;
        private Asignacion inicio;

        public ForDo(Asignacion f, Operacion b, LinkedList<Instruccion> lst)
        {
            this.inicio = f;
            this.val_limite = b;
            this.listaInstrucciones = lst;
            
        }


        public Object ejecutar(TSimbolo ts)
        {
            TSimbolo tablaLocal = new TSimbolo(ts);

            string nomb_var = inicio.id; //variable para el aumento
            inicio.ejecutar(tablaLocal);
            Primitivo p = (Primitivo)val_limite.ejecutar(tablaLocal);
            double b = Convert.ToDouble(p.valor);

            Simbolo x = ts.obtener(nomb_var);
            Primitivo z = x.getValor();
            double a = Convert.ToDouble(z.valor);


            while (a!=b)
            {
                foreach (Instruccion t in listaInstrucciones)
                {
                    t.ejecutar(tablaLocal);
                }


                //se ejecuta el aumento

                Primitivo s = new Primitivo(Primitivo.tipo_val.INT, (object)(1));
                Acceso f = new Acceso(nomb_var);
                Operacion t1 = new Operacion(f);
                Operacion t2 = new Operacion(s);

                Operacion final = new Operacion(t1, t2, Operacion.Tipo_operacion.SUMA);

                Asignacion asig = new Asignacion(nomb_var, final);

                asig.ejecutar(tablaLocal);

                //ahora la atraccion de datos

                x = ts.obtener(nomb_var);
                z = x.getValor();
                a = Convert.ToDouble(z.valor);

            }

            


            return null;
        }

    }
}

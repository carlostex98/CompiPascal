using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompiPascal.Analizador;
using CompiPascal.General;

namespace CompiPascal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("eee");
            compile();
        }

        private void compile()
        {
            
            Evaluador eval = new Evaluador();
            eval.analizar_arbol(entrada.Text);
            salida.Text = Maestro.Instance.getOutput();
            
            //salida.Text = salida.Text + Maestro.Instance.getMessages();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            //System.Diagnostics.Debug.WriteLine("eee");
            compile();
        }

        private void compile()
        {
            Maestro.Instance.clear();
            Evaluador eval = new Evaluador();
            eval.analizar_arbol(entrada.Text);
            salida.Text = Maestro.Instance.getOutput();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            var processInfo = new ProcessStartInfo("cmd.exe", "/c "+ "dot -Tpng AST.txt -o outfile.png")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = @"C:\compiladores2\"
            };

            StringBuilder sb = new StringBuilder();
            Process p = Process.Start(processInfo);
            p.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
            p.BeginOutputReadLine();
            p.WaitForExit();
            //System.Diagnostics.Debug.WriteLine(sb.ToString());
            openImage();

            
        }


        public void openImage()
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + "outfile.png")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = @"C:\compiladores2\"
            };

            StringBuilder sb = new StringBuilder();
            Process p = Process.Start(processInfo);
            p.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
            p.BeginOutputReadLine();
            p.WaitForExit();
            //System.Diagnostics.Debug.WriteLine(sb.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Errores n = new Errores();
            n.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            grafica_ts n = new grafica_ts();
            n.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SuperSimbolos n = new SuperSimbolos();
            n.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Buscar para compilar",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt | Pascal Files (*.pas) | *.pas | Todos (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //entrada.Text = openFileDialog1.FileName;
                string text = System.IO.File.ReadAllText(openFileDialog1.FileName);
                entrada.Text = text;
            }
        }
    }
}

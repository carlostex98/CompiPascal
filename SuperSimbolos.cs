using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CompiPascal.General;

namespace CompiPascal
{
    public partial class SuperSimbolos : Form
    {
        public SuperSimbolos()
        {
            InitializeComponent();
        }

        private void SuperSimbolos_Load(object sender, EventArgs e)
        {
            foreach (string[] item in Maestro.Instance.obtenerSimbolos())
            {
                dataGridView1.Rows.Add(item[0], item[1], item[2], item[3]);
            }
        }
    }
}

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
    public partial class grafica_ts : Form
    {
        public grafica_ts()
        {
            InitializeComponent();
            //cargamos data
            foreach(string[] item in Maestro.Instance.obtenerGrafica())
            {
                dataGridView1.Rows.Add(item[0], item[1], item[2]);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grafica_ts_Load(object sender, EventArgs e)
        {

        }
    }
}

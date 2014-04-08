using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AMEditor
{
    public partial class FormParams : Form
    {
        public FormParams()
        {
            InitializeComponent();
        }

        private void FormParams_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _in = (int)numericUpDown1.Value;
            _out = (int)numericUpDown2.Value;
            this.Close();
        }
    }
}

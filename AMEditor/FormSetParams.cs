using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DBManager;

namespace AMEditor
{
    public partial class FormSetParams : Form
    {
        public FormSetParams()
        {
            InitializeComponent();

            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "SignName";
            comboBox1.DataSource = ComboBoxAssembler.Fill();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _porNumber = int.Parse(textBox1.Text);
            _function = textBox2.Text;
            _inParams = textBox3.Text;
            _outParams = textBox4.Text;
            _realization = comboBox1.SelectedValue.ToString();
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using DBManager;
using AMLib;

namespace AMEditor
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            opList = new Operators();
            myTable = new DataTable("Operators");
            myTable2 = new DataTable("Operations");
            whichEntered = 0;
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            isLoaded = false;

            myTable = GridViewAssembler.UpdateOperators();
            myTable2 = GridViewAssembler.UpdateOperations();

            dataGridView1.DataSource = myTable;
            dataGridView2.DataSource = myTable2;

            dataGridView1.Columns[4].Visible = false; //hide 4 column
            dataGridView1.Columns[3].Visible = false; //hide 3 column
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 150;
            //dataGridView1.Columns[3].Width = 150;

            dataGridView2.Columns[0].Width = 30;
            dataGridView2.Columns[1].Width = 60;
            dataGridView2.Columns[2].Width = 150;
            dataGridView2.Columns[3].Width = 150;

            isLoaded = true;

            ModelIndex = 0;
            ModelSize = 0;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoaded)
            {
                lblFoto.Text = dataGridView1.CurrentRow.Cells["AlgorithmModel"].Value.ToString();
                whichEntered = 1;
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            Operator op = new Operator();
            switch (whichEntered)
            {
                case 1:
                    op.Name = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();
                    op.AlgorithmModel = dataGridView1.CurrentRow.Cells["AlgorithmModel"].Value.ToString();
                    op.OperatorModel = dataGridView1.CurrentRow.Cells["OperatorModel"].Value.ToString();
                    break;
                case 2:
                    InitializeOperation(op);
                    break;
            }
            opList.AddToEnd(op);
            ModelSize = opList.Size;
            ModelIndex = ModelSize;
            UpdateAM();
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
            //A Numeration Begins from [1..N]  <<!!!
            Operator op = new Operator();
            switch (whichEntered)
            { 
                case 1: 
                    op.Name = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();
                    op.AlgorithmModel = dataGridView1.CurrentRow.Cells["AlgorithmModel"].Value.ToString();
                    op.OperatorModel = dataGridView1.CurrentRow.Cells["OperatorModel"].Value.ToString();
                    op.In = f1._in;
                    op.Out = f1._out;
                    AddOpetarion();
                    break;
                case 2:
                    InitializeOperation(op);
                break;
            }
            opList.AddBeforeCurrent(ModelIndex, op);

            ModelSize = opList.Size;
            ModelIndex = ModelSize;
            UpdateAM();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            //A Numeration Begins from [1..N]  <<!!!
            Operator op = new Operator();
            switch (whichEntered)
            {
                case 1:
                    op.Name = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();
                    op.AlgorithmModel = dataGridView1.CurrentRow.Cells["AlgorithmModel"].Value.ToString();
                    op.OperatorModel = dataGridView1.CurrentRow.Cells["OperatorModel"].Value.ToString();
                    break;
                case 2:
                    InitializeOperation(op);
                    break;
            }
            opList.ReplaceCurrent(ModelIndex, op);
            ModelSize = opList.Size;
            ModelIndex = ModelSize;
            UpdateAM();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtALModel.Text = "";
            opList.Clear();
            ModelSize = opList.Size;
            ModelIndex = ModelSize;
            UpdateAM();
        }

        private void btnDelCur_Click(object sender, EventArgs e)
        {
            txtALModel.Text = "";
            opList.DeleteCurrent(ModelIndex);
            ModelSize = opList.Size;
            ModelIndex = ModelSize;
            UpdateAM();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (ModelIndex > 1)
            {
                ModelIndex--;
            }
            UpdateAM();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ModelIndex < ModelSize)
            {
                ModelIndex++;
            }
            UpdateAM();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            txtALModel.Text = "";
            txtOPModel.Text = "";
            ModelIndex = 0;
            ModelSize = 0;
            opList.Clear();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // додати можливість розбору строки на оператори!!!<<<
            System.IO.Stream myStream;

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.AddExtension = true;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    System.IO.StreamReader wText = new System.IO.StreamReader(myStream);

                    txtALModel.Text = wText.ReadToEnd();
                    wText.Close();

                    myStream.Close();
                    //******************
                    string path = openFileDialog1.FileName.Substring(0, openFileDialog1.FileName.Length - 4) + ".dat";
                    
                    FileStream fs = new FileStream(path, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    opList = (Operators)bf.Deserialize(fs);
                    fs.Close();

                    ModelSize = opList.Size;
                    ModelIndex = ModelSize;
                    
                    UpdateAM();
                }
            }
            openFileDialog1.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            System.IO.Stream myStream;

            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.FileName = "Model";
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    System.IO.StreamWriter wText = new System.IO.StreamWriter(myStream);

                    wText.Write(txtALModel.Text);
                    wText.Flush();

                    myStream.Close();
                    //**************
                    string path = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.Length - 4) + ".dat";
                    FileStream fs = new FileStream(path, FileMode.Create);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, opList);
                    fs.Flush();
                    fs.Close();

                }
            }
            saveFileDialog1.Dispose();
        }

        private void UpdateAM()
        {
            txtALModel.ForeColor = System.Drawing.Color.Black;
            txtALModel.Text = "";     
            
            lblMSize.Text = opList.Size.ToString();
            lblCurElem.Text = ModelIndex.ToString();

            txtALModel.Text += opList.ToString();
        }

        private void btnOPForm_Click(object sender, EventArgs e)
        {
            if (opList.Size > 0)
            {
                ALGConverter conv = new ALGConverter();
                List<List<int>>[] pList = conv.Convert(opList);
                CreateOperatorModel(pList);
            }
            else
                MessageBox.Show("Сначала нужно ввести алг.модель, а только затем конвертировать!", "ERROR!");
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoaded)
            {
                lblFoto.Text = dataGridView2.CurrentRow.Cells["Name"].Value.ToString();
                whichEntered = 2;
            }
        }

        private void SetParamCount()
        {
            f1 = new FormParams();
            f1.ShowDialog();

        }

        private void AddOpetarion()
        {
            f2 = new FormSetParams();
            f2.ShowDialog();

        }

        private void InitializeOperation(Operator op)
        {
            SetParamCount();
            op.Name = dataGridView2.CurrentRow.Cells["Name"].Value.ToString();
            string alModel = dataGridView2.CurrentRow.Cells["AlgorithmModel"].Value.ToString();
            op.In = f1._in;
            op.Out = f1._out;
            AddOpetarion();
            op.SetFunctions(f2._function);
            //op.Function = f2._function;
            op.PorNubmer = f2._porNumber;
            op.Realization = f2._realization;
            op.SetInParams(f2._inParams);
            op.SetOutParams(f2._outParams);
            StringBuilder resStr = new StringBuilder();
            int i = 0;
            //*********************
            switch (alModel)
            {
                case "E(C;(p1);(p2))":  //ініціалізація константи
                    resStr.Append("E" + op.PorNubmer.ToString() + "(C" + op.Realization + ";");
                    resStr.Append("(" + op.GetInParam(0) + ");");
                    resStr.Append("(" + op.GetOutParam(0) + "))");
                    break;
                case "E(C:N(p1);(p1);(p2))": //обчислення функції
                    resStr.Append("E" + op.PorNubmer.ToString() + "(C" + op.Realization + ":");
                    resStr.Append(op.GetFunction(0) + ";(");
                    for (i = 0; i < op.In; i++)
                    {
                        resStr.Append(op.GetInParam(i));
                        if (i < op.In - 1)
                            resStr.Append(",");
                    }
                    resStr.Append(");(");
                    for (i = 0; i < op.Out; i++)
                    {
                        resStr.Append(op.GetOutParam(i));
                        if (i < op.Out - 1)
                            resStr.Append(",");
                    }
                    resStr.Append("))");
                    break;
                case "E(S:N,g;(p1);(p2,p3))": //вимірювання
                    resStr.Append("E" + op.PorNubmer + "(S" + op.Realization + ":");
                    resStr.Append(op.GetFunction(0) + "," + op.GetFunction(1) + ";(");
                    for (i = 0; i < op.In; i++)
                    {
                        resStr.Append(op.GetInParam(i));
                        if (i < op.In - 1)
                            resStr.Append(",");
                    }
                    resStr.Append(");(");
                    for (i = 0; i < op.Out; i++)
                    {
                        resStr.Append(op.GetOutParam(i));
                        if (i < op.Out - 1)
                            resStr.Append(",");
                    }
                    resStr.Append("))");
                    break;
                case "E(C:I(p1*b(T-t));(p1);(p2))": //затримка
                    resStr.Append("E" + op.PorNubmer + "(C" + op.Realization + ":I(");
                    resStr.Append(op.GetInParam(0) + "*b(T-t));(" + op.GetInParam(0) + ");(" + op.GetOutParam(0) + "))");
                    break;
            }

            op.AlgorithmModel = resStr.ToString();
        }

        private void btnComplexity_Click(object sender, EventArgs e)
        {
            if (opList.Size > 0)
            {
                MessageBox.Show("Total system complexity = " + ModelSize.ToString(), "Information");
            }
            else
                MessageBox.Show("Сначала нужно ввести алг.модель, а только затем конвертировать!", "ERROR!");
        }

        private bool isLast(int count, int pos)
        {
            if (pos == count)
                return true;
            return false;
        }

        private bool isFirst(int pos)
        {
            if (pos == 0)
                return true;
            return false;
        }

        private void CreateOperatorModel(List<List<int>>[] pList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (List<List<int>> curList in pList)
            {
                foreach (List<int> list in curList)
                {
                    int count = list.Count-1;
                    int pos = 0;
                    foreach (Object obj in list)
                    {
                        
                        Operator op = opList.GetOperator((int)obj);
                        switch (op.Name)
                        { 
                            case "C(p1/p2)": //инициализ.константы
                                if (!isFirst(pos))
                                {
                                    sb.Append(" [");
                                }
                                sb.Append("F" + op.PorNubmer.ToString() + " (1)");
                                if (isLast(count, pos))
                                {
                                    sb.Append("b(" + op.GetInParam(0) + ")");
                                    for (int i = 1; i < (count - 1); i++)
                                        sb.Append("]");
                                }
                                break;
                            case "F(p1,p2)": //ф-ция
                                if (!isFirst(pos))
                                {
                                    sb.Append(" [");
                                }
                                sb.Append("F" + op.PorNubmer.ToString() + "(1, " + op.GetFunction(0) + ")");
                                if (isLast(count, pos))
                                {
                                    sb.Append("b(" + op.GetInParam(0) + ")");
                                    for (int i = 1; i < (count - 1); i++)
                                        sb.Append("]");
                                }
                                break;
                            case "I(p1/p2)": //измерение
                                if (!isFirst(pos))
                                {
                                    sb.Append(" [");
                                }
                                sb.Append("F" + op.PorNubmer.ToString() + "(2,+) [b(" + op.GetOutParam(1) + "), F (n, " + op.GetFunction(1) + ") [F (1, " + op.GetFunction(0) + ") [b(" + op.GetInParam(0) + ")]]]");
                                if (isLast(count, pos))
                                {
                                    for (int i = 1; i < (count - 1); i++)
                                        sb.Append("]");
                                }
                                break;
                            case "delay(t)": //задержка
                                if (!isFirst(pos))
                                {
                                    sb.Append(" [");
                                }
                                sb.Append("F" + op.PorNubmer.ToString() + "(n, " + op.GetInParam(0) + "*b(T-t))");
                                if (isLast(count, pos))
                                {
                                    sb.Append("b(" + op.GetInParam(0) + ")");
                                    for (int i = 1; i < (count-1); i++)
                                        sb.Append("]");
                                }
                                break;
                            default: break;
                        }
                        pos++;
                    }
                    
                    sb.AppendLine();
                }
            }
            string txtPath = "operatorsResult.txt";
            FileStream fs = new FileStream(txtPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(sb.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
            if (File.Exists(txtPath))
            {
                txtOPModel.Text = File.ReadAllText(txtPath, Encoding.Default); 
            }
        }

    }
}

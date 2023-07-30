using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace NeuralBot
{
    public partial class Form2 : Form
    {
        XDate xDate = new XDate(2022, 1, 1, 10, 0, 0);
        GraphPane myPane;
        StockPointList spl;
        int Count = 0;
        int CountOHV = 0;
        List<List<List<List<double>>>> DB = new List<List<List<List<double>>>>();
        List<List<double>> TempDB = new List<List<double>>();
        public Form2()
        {
            InitializeComponent();
             myPane = zedGraphControl1.GraphPane;
             spl = new StockPointList();
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "";
            myPane.Title.Text = "";
            myPane.XAxis.Type = AxisType.DateAsOrdinal;
            comboBox2.Items.Add("Новый образец");
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CountOHV += 1;
            if (CountOHV > Convert.ToInt32(textBox6.Text))
            {
                MessageBox.Show("Количество свечей в данном образце превышает количество свечей в остальных.");
                return;
            }
            TempDB.Add(new List<double>());
            double hi = Convert.ToDouble(textBox1.Text);
            double low = Convert.ToDouble(textBox2.Text);
            double open = Convert.ToDouble(textBox3.Text);
            double close = Convert.ToDouble(textBox4.Text);
            double volume = Convert.ToDouble(textBox8.Text);
            TempDB[TempDB.Count - 1].Add(hi);
            TempDB[TempDB.Count - 1].Add(low);
            TempDB[TempDB.Count - 1].Add(open);
            TempDB[TempDB.Count - 1].Add(close);

            if (checkBox1.Checked) 
            {
                TempDB[TempDB.Count - 1].Add(volume);
                textBox7.Text += hi + ":" + low + ":" + open + ":" + close + ":" + volume + ":";
                
            }
            else
            {
                textBox7.Text += hi + ":" + low + ":" + open + ":" + close + ":";
                
            }

            checkBox1.Enabled = false;
            xDate.AddMinutes(1);
            StockPt pt = new StockPt(xDate.XLDate, hi, low, open, close, volume);
            spl.Add(pt);
            JapaneseCandleStickItem myCurve = myPane.AddJapaneseCandleStick("Свеча" + (Count+1), spl);

            if (Count == 0)
            {
                myCurve.Stick.IsAutoSize = true;
                myCurve.Stick.Color = Color.Blue;
                button7.Enabled = true;
                button2.Enabled = true;
                button9.Enabled = true;
            }
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            Count += 1;
            
        }

        void PaintOHVC() 
        {
            textBox7.Text = "";
            spl.Clear();
            myPane.CurveList.Clear();
            for (int i =0; i < TempDB.Count; i++) 
            {
                xDate.AddMinutes(1);
                StockPt pt = new StockPt(xDate.XLDate,TempDB[i][0],TempDB[i][1],TempDB[i][2],TempDB[i][3],100000);
                spl.Add(pt);
                JapaneseCandleStickItem myCurve = myPane.AddJapaneseCandleStick("Свеча" + (Count + 1), spl);
                if (checkBox1.Checked)
                {
                    textBox7.Text += TempDB[i][0] + ":" + TempDB[i][1] + ":" + TempDB[i][2] + ":" + TempDB[i][3] + ":" + TempDB[i][4] + ":";
                }
                else
                {
                    textBox7.Text += TempDB[i][0] + ":" + TempDB[i][1] + ":" + TempDB[i][2] + ":" + TempDB[i][3] + ":";
                }
                
                Count++;
                CountOHV++;
            }


            
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("Напишите имя класса " + '\n' +
                    "Пример: Молот");
                return;
            }
            
                comboBox1.Items.Add(textBox5.Text);
                textBox5.Text = "";
            
            DB.Add(new List<List<List<double>>>());

            button1.Enabled = true;
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = DB.Count-1;
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(CountOHV < Convert.ToInt32(textBox6.Text)) 
            {
                MessageBox.Show("Количество свечей в данном образце меньше чем во всех остальных.");
                return;
            }
            button4.Enabled = true;
            textBox6.Enabled = false;
            comboBox2.Items.Add("Образец " +  (DB[comboBox1.SelectedIndex].Count+1));
            

            DB[comboBox1.SelectedIndex].Add(new List<List<double>>());
            for (int i =0; i < TempDB.Count; i++) 
            {
                DB[comboBox1.SelectedIndex][DB[comboBox1.SelectedIndex].Count - 1].Add(TempDB[i]);
            }
            myPane.CurveList.Clear();
            Count = 0;
            TempDB.Clear();
            spl.Clear();
            CountOHV = 0;
            button8.Enabled = true;
            button2.Enabled = false;
            button7.Enabled = false;
            textBox7.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(TempDB.Count == 0) 
            {
                return;
            }
            TempDB.RemoveAt(TempDB.Count - 1);
            Count = 0;
            CountOHV = 0;
            PaintOHVC();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            checkBox1.Enabled = true;
            int select = comboBox1.SelectedIndex;
            myPane.CurveList.Clear();
            TempDB.Clear();
            spl.Clear();
            CountOHV = 0;
            Count = 0;
            comboBox1.Items.RemoveAt(select);
            DB.RemoveAt(select);
            if (DB.Count >0) 
            {
                comboBox1.SelectedIndex = 0;
            }
            else 
            {
                button8.Enabled = false;
            }

            /*comboBox2.Items.Clear();
            comboBox2.Items.Add("Новый образец");
            comboBox2.SelectedIndex = 0;*/

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int select = comboBox2.SelectedIndex;
            if(select == 0) 
            {
                myPane.CurveList.Clear();
                Count = 0;
                TempDB.Clear();
                spl.Clear();
                CountOHV = 0;
                return; 
            }
            TempDB.Clear();
            for(int i=0; i < DB[comboBox1.SelectedIndex][select-1].Count; i++) 
            {
                TempDB.Add(new List<double>());
                TempDB[TempDB.Count - 1].Add(DB[comboBox1.SelectedIndex][select-1][i][0]);
                TempDB[TempDB.Count - 1].Add(DB[comboBox1.SelectedIndex][select-1][i][1]);
                TempDB[TempDB.Count - 1].Add(DB[comboBox1.SelectedIndex][select-1][i][2]);
                TempDB[TempDB.Count - 1].Add(DB[comboBox1.SelectedIndex][select-1][i][3]);
                if(checkBox1.Checked)
                TempDB[TempDB.Count - 1].Add(DB[comboBox1.SelectedIndex][select - 1][i][4]);
            }

            Count = 0;
            CountOHV = 0;
            PaintOHVC();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int select = comboBox1.SelectedIndex;
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Новый образец");
            comboBox2.SelectedIndex = 0;
            myPane.CurveList.Clear();
            Count = 0;
            TempDB.Clear();
            spl.Clear();
            CountOHV = 0;
            if(DB.Count!=0)
            if(DB[select].Count == 0) 
            {
                return;
            } 

            for(int i =0; i < DB[select].Count; i++) 
            {
                comboBox2.Items.Add("Образец " + (i + 1));
            }



        }

        private void button4_Click(object sender, EventArgs e)
        {
            string file = "";
            string[] d = new string[comboBox1.Items.Count];
            for(int i =0; i < comboBox1.Items.Count; i++) 
            {
                d[i] = "";
                for(int j =0; j< comboBox1.Items.Count; j++) 
                {
                    if (j == i)
                    {
                        d[i] += "1,";
                    }
                    else
                    {
                        d[i] += "0,";
                    }
                }
                d[i] += "/" + comboBox1.Items[i];
            }
            for(int i =0; i < DB.Count; i++) 
            {
                file += d[i] + '\n' + "&" + '\n';
                for(int j =0; j < DB[i].Count; j++) 
                {
                    for(int h =0; h < DB[i][j].Count; h++) 
                    {
                        for(int u =0; u < DB[i][j][h].Count; u++) 
                        {
                            file += Convert.ToString(DB[i][j][h][u]) + ":";
                        }
                    }
                    file += ";" + '\n';
                }
                file += "&"+ '\n';
            }

            
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string filename = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(filename, file);
            MessageBox.Show("Сохранено");



        }

        private void button9_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            if(DB[comboBox1.SelectedIndex].Count == 0) 
            {
                for(int h =0; h < Convert.ToInt32(textBox9.Text); h++) 
                {
                    DB[comboBox1.SelectedIndex].Add(new List<List<double>>());
                    comboBox2.Items.Add("Образец " + (h + 1));
                    for (int i = 0; i < TempDB.Count; i++)
                    {
                        DB[comboBox1.SelectedIndex][DB[comboBox1.SelectedIndex].Count-1].Add(new List<double>());
                        for (int j = 0; j < TempDB[i].Count; j++)
                        {
                            DB[comboBox1.SelectedIndex][DB[comboBox1.SelectedIndex].Count - 1][i].Add(TempDB[i][j] + rnd.Next(-(Convert.ToInt32(TempDB[i][j]/10)), Convert.ToInt32(TempDB[i][j]/10)));
                        }

                    }
                }
            }
            else 
            {

                MessageBox.Show("Класс должен быть пустым");
                
            }

            button8.Enabled = true;
            button4.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

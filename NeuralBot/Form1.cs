using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace NeuralBot
{
    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        string strX = "";
        List<List<double>> W = new List<List<double>>();
        double coefficientA = 0.8; // Коэффициент a
        double learningRate = 0.6; // Скороть обучения
        int CountIteration = 4000; // Количество итераций при обучении
        List<int> CountHidenSloi = new List<int>(); // Количество слоев в скрытом слое и нейронов в них
        int OneSloi = 0;           //Количество нейронов в первом слое 


        private void button2_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                string filename = openFileDialog1.FileName;
                strX = System.IO.File.ReadAllText(filename);
                label6.Text = openFileDialog1.SafeFileName;
                label15.Text = Convert.ToString(strX.Split('&')[0].Split(',').Length-1);
                label16.Text = Convert.ToString(strX.Split('&')[1].Split(';')[0].Split(':').Length-1);
                label17.Text = Convert.ToString(Convert.ToDouble(label15.Text) * (Convert.ToDouble(label16.Text) + 1));
                button3.Enabled = true;




                //MessageBox.Show("файл загружен");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label20.Text = "Идет обучение подождите";
            label20.Update();
            coefficientA = Convert.ToDouble(textBox1.Text);
            learningRate = Convert.ToDouble(textBox2.Text);
            CountIteration = Convert.ToInt32(textBox3.Text);
            List<string> tempHidenSloi = ConvertMasStr(textBox6.Text.Split(','), 0);
            CountHidenSloi.Clear();
            if(Convert.ToInt32(tempHidenSloi[0]) != 0) 
            {
                CountHidenSloi.Add(0);
            }
            else
            {
                for (int i = 0; i < tempHidenSloi.Count; i++)
                {
                    CountHidenSloi.Add(Convert.ToInt32(tempHidenSloi[i]));
                }
            }
            OneSloi = Convert.ToInt32(textBox7.Text);
            Neurons Ne = new Neurons();
            Ne.Param(coefficientA, learningRate, CountIteration,CountHidenSloi,OneSloi);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            W = Ne.StartEducation(strX);
            stopwatch.Stop();
            label18.Text = Convert.ToString(stopwatch.ElapsedMilliseconds + "mls");
            button4.Enabled = true;
            button5.Enabled = true;
            label20.Text = "Нейронная сеть обучена";
            label20.ForeColor = Color.Green;
            MessageBox.Show("Обучение завершено");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Neurons Ne = new Neurons();
            Ne.Param(coefficientA, learningRate, CountIteration, CountHidenSloi, OneSloi);
            if(textBox5.Text[textBox5.Text.Length-1] == ':') 
            {
                textBox5.Text.Remove(textBox5.Text.Length - 1);
            }
            List<double> Xinput = ConvertStrList(ConvertMasStr(textBox5.Text.Split(':'), 0));
            List<double> Output = Ne.Recognition(W,Xinput);

            string textout = "";
            for(int i =0; i < Output.Count; i++) 
            {
                textout += Math.Round(Output[i],3) + " ";
            }
            stopwatch.Stop();
            if(stopwatch.ElapsedMilliseconds == 0) 
            {
                label19.Text = Convert.ToString("<" + stopwatch.ElapsedMilliseconds + "mls");
            }
            else
            {
                label19.Text = Convert.ToString(stopwatch.ElapsedMilliseconds + "mls");
            }
            MessageBox.Show(textout);
        }
        List<double> ConvertStrList(List<string> strList)
        {
            List<double> final = new List<double>();
            for (int i = 0; i < strList.Count; i++)
            {
                //final.Add(Convert.ToDouble(strList[i]));
                if (strList[i] == "") { continue; }
                final.Add(double.Parse(strList[i], System.Globalization.CultureInfo.InvariantCulture));
            }
            return final;
        }
        List<string> ConvertMasStr(string[] Mas, int pop)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < Mas.Length - pop; i++)
            {
                list.Add(Mas[i]);
            }
            return list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NeuronFile;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                NeuronFile = System.IO.File.ReadAllText(filename);
                label1.Text = openFileDialog1.SafeFileName;

                string[] NF = NeuronFile.Split('\n');
                coefficientA = Convert.ToDouble(NF[1].Split(':')[1]);
                textBox1.Text = Convert.ToString(coefficientA);
                learningRate = Convert.ToDouble(NF[2].Split(':')[1]);
                textBox2.Text = Convert.ToString(learningRate);
                CountIteration = Convert.ToInt32(NF[3].Split(':')[1]);
                textBox3.Text = Convert.ToString(CountIteration);
                OneSloi = Convert.ToInt32(NF[4].Split(':')[1]);
                textBox7.Text = Convert.ToString(OneSloi);
                string[] tempM = NF[5].Split(':')[1].Split(',');
                CountHidenSloi.Clear();
                for (int i =0; i < tempM.Length-1; i++)
                {
                    CountHidenSloi.Add(Convert.ToInt32(tempM[i]));
                }
                W.Clear();
                string[] NFstroka;
                for(int i =8; i < NF.Length; i++) 
                {
                    if (NF[i] == "") continue;
                    W.Add(new List<double>());
                    NFstroka = NF[i].Split('&');
                    for (int j =0; j < NFstroka.Length; j++) 
                    {
                        if (NFstroka[j] == "") continue;
                        W[i-8].Add(Convert.ToDouble(NFstroka[j]));
                    }
                }
                label15.Text = Convert.ToString(W.Count);
                label16.Text = Convert.ToString(W[0].Count - 1);
                label17.Text = label17.Text = Convert.ToString(Convert.ToDouble(label15.Text) * (Convert.ToDouble(label16.Text) + 1));
                button5.Enabled = true;
                label20.Text = "Нейронная сеть загружена";
                label20.ForeColor = Color.Green;
            }
                
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string colNS = "";

            for(int i =0; i < CountHidenSloi.Count; i++)
            {
                colNS += CountHidenSloi[i] + ",";
            }


            string save = "Структура документа" + '\n' + "Коэффициент a: "+ coefficientA + '\n' + "Скороть обучения: " + learningRate 
                + '\n' + "Количество итераций при обучении: " + CountIteration
                 + '\n' + "Количество нейронов в первом слое:" + OneSloi
                 + '\n' + "Количество скрытых слоев и нейронов в них:" + colNS
                + '\n' + "Веса обученой нейронной сети" + '\n' + "#################################################" + '\n';

            for(int i =0; i < W.Count; i++) 
            {
                for(int j=0; j < W[i].Count; j++) 
                {
                    if (j == W[i].Count - 1)
                    {
                        save += W[i][j].ToString() + '\n';
                    }
                    else
                    {
                        save += W[i][j].ToString() + '&';
                    }
                }
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(filename, save);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}

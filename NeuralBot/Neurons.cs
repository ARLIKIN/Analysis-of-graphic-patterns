using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBot
{
    internal class Neurons
    {
        double a = 0.8;
        int KolYoutput = 0;
        int KolYinput = 0;
        double LearningRate = 0.6;
        int FinalCount = 1000;
        int SystemCount = 0;
        int SystemCount2 = 0;
        int KolX = 0;
        int KolV = 0;
        int tic = 0;
        bool NoHiden = false;
        bool NoInput = false;
        int KolSloi;

        List<List<double>> Y = new List<List<double>>();
        List<List<double>> W = new List<List<double>>();
        List<string> MD = new List<string>();
        List<string> MX = new List<string>();
        List<string> XD = new List<string>();
        List<string> LMX = new List<string>();
        List<double> X = new List<double>();
        List<double> d = new List<double>();
        List<double> err = new List<double>();
        List<double> error = new List<double>();
        List<string> NameClass = new List<string>();
        List<int> KolYHidenSloi = new List<int>();
        List<int> Windex = new List<int>();
        
        


        public void Param(double a, double LearningRate, int countIteration, List<int> HidenSloi, int OneSloi) 
        {
            this.a = a;
            this.LearningRate = LearningRate;
            FinalCount = countIteration;
            KolYHidenSloi = HidenSloi;
            KolYinput = OneSloi;

        }

        public List<List<double>> StartEducation(string Xstr) 
        {
            ReadstrX(Xstr); // Чтение входных данных
            GenerateInputSloi();//формирование нейронов и весов
            KolSloi = KolYHidenSloi.Count;
            for(int i =0; i < KolSloi; i++)
            {
                GenerateHidenSloi(i+1);
            }
            GenerateOutputSloi();


            while (true) //обучение
            {
                if(tic>= FinalCount) { break; }
                if(tic == 2000)
                {
                    Console.Write("hi");
                }
                Korrekt(X,false);
                SystemClass();
                tic += 1;
            }

            return W;
        }

        void ReadstrX(string Xstr) 
        {
            MD.Clear();
            MX.Clear();
            XD = ConvertMasStr(Xstr.Split('&'),1);
            for(int i = 0; i < XD.Count; i++) 
            {
                if((i+1)%2 != 0) 
                {
                    MD.Add(XD[i]);
                }
                else 
                {
                    MX.Add(XD[i]);
                }
            }
            List<string> temp;
            for (int i =0; i < MD.Count; i++) 
            {
               temp = (ConvertMasStr(MD[i].Split('/'), 0));
                NameClass.Add(temp[1]);
                MD[i] = temp[0];
            }
            


            d = ConvertStrList(ConvertMasStr(MD[0].Split(','),0));
            LMX = ConvertMasStr(MX[0].Split(';'),1);
            X = ConvertStrList(ConvertMasStr(LMX[0].Split(':'),0));
            KolYoutput = d.Count;
            KolX = X.Count;
            
            List<double> ConvertStrList(List<string> strList) 
            {
                List<double> final = new List<double>();
                for(int i =0; i < strList.Count; i++) 
                {
                    //final.Add(Convert.ToDouble(strList[i]));
                    if(strList[i] == "") { continue; }
                    final.Add(double.Parse(strList[i],System.Globalization.CultureInfo.InvariantCulture));
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
        }

        void GenerateInputSloi() 
        {

            
            if(KolYinput <= 0) 
            {
                NoInput = true;
                return;
            }
            Y.Add(new List<double>());
            GenerateWight(KolYinput,0);//генерация весов
            for(int i =0; i < KolYinput; i++) 
            {
                Y[Y.Count-1].Add(Neuron(X, i));
                
            }
        }

        void GenerateHidenSloi(int nm) 
        {
           if(KolYHidenSloi[0] == 0) 
            {
                NoHiden = true;
                return;
            }
            Y.Add(new List<double>());
            KolV = KolYHidenSloi[nm];
            GenerateWight(KolV, 1);

            for(int i =0; i < KolYHidenSloi[nm-1]; i++)
            {
                Y[Y.Count].Add(Neuron(Y[Y.Count - 1], W.Count + i));
            }
        }

        void GenerateOutputSloi() 
        {
            Y.Add(new List<double>());
            if (!NoInput)
            {
                if(KolYoutput <= 0) { KolYoutput = 1; }
                GenerateWight(KolYoutput, 1);
                for(int i =0; i < KolYoutput; i++) 
                {
                    Y[Y.Count-1].Add(Neuron(Y[Y.Count - 1], W.Count-KolYoutput + i));
                }

            }
            else
            {
                GenerateWight(KolYoutput, 0);
                for(int i =0; i < KolYoutput; i++) 
                {
                    Y[Y.Count - 1].Add(Neuron(X,i));
                }
            }
        }

        void GenerateWight(int KolY, int t) 
        {
            Random rnd = new Random();
            if (t == 1)
            {
                for(int i =0; i < KolY; i++) 
                {
                    W.Add(new List<double>());
                    for(int j=0; j < Y[Y.Count - 2].Count + 1; j++) 
                    {
                        W[W.Count-1].Add((rnd.Next(-500,500)/1000d));
                    }
                }
            }
            else
            {
                for (int i = 0; i < KolY; i++)
                {
                    W.Add(new List<double>());
                    for (int j = 0; j < KolX + 1; j++)
                    {
                        W[i].Add(rnd.Next(-500, 500) / 1000d);
                    }
                }
            }
        }

        double Neuron(List<double> X,int m) 
        {
            double y = 0;
            double u = W[m][0];
            for(int i =0; i < X.Count; i++) 
            {
                u += X[i] * W[m][i+1];
            }
            y = 1 / (1 + Math.Exp(-a * u));
            return y;
        }

        void Korrekt(List<double> X, bool flag) 
        {
            if (flag) { return; }

            int Ylength = Y.Count;
            int Wlength = W.Count;
            int abc = 0;

            if (!NoInput)
            {
                for(int i =0; i < KolYinput; i++)
                {
                    Y[0][i] = Neuron(X, i);
                }

                if (!NoHiden)
                {
                    for(int i =0; i < KolYHidenSloi.Count; i++)
                    {
                        for(int j =0; j < KolYHidenSloi[i]; j++) 
                        {
                            Y[i + 1][j] = (Neuron(Y[i], j + KolYinput + abc));
                        }
                        abc += KolYHidenSloi[i];
                    }
                }

                for (int i = 0; i < KolYoutput; i++)
                {
                    Y[Y.Count - 1][i] = (Neuron(Y[Y.Count - 2], i + (W.Count - KolYoutput)));
                }
            }
            else
            {
                for(int i =0; i < KolYoutput; i++)
                {
                    Y[Y.Count - 1][i] = (Neuron(X, i));
                }
            }


            int Index;
            err.Clear();
            error.Clear();
            for (int i = 0; i <= d.Count - 1; i++)
            {
                err.Add(0);
            }

            for (int i = 0; i < W.Count; i++)
            {
                error.Add(0);
            }
            for (int i =0; i < d.Count; i++) 
            {
               
                
                err[d.Count - 1 - i] = Minus(Y[Y.Count - 1], i);
                err[d.Count - 1 - i] = Derivative(Y[Y.Count-1][i], err,d.Count-1-i);
                error[W.Count - KolYoutput + i] = err[d.Count - 1 - i]; 
                if (!NoInput)
                {
                    if (!NoHiden) 
                    {
                        Index = KolYHidenSloi[KolYHidenSloi.Count - 1];
                    }
                    else 
                    {
                        Index = KolYinput;
                    }

                    for (int j = 0; j <= Index; j++)//= не нужно
                    {
                        if (j == 0)
                        {
                            W[W.Count - Y[Y.Count - 1].Count + i][j] += err[d.Count - 1 - i] * LearningRate;
                        }
                        else
                        {
                            W[W.Count - Y[Y.Count - 1].Count + i][j] += err[d.Count - 1 - i] * LearningRate * Y[Y.Count - 2][j - 1];
                        }
                    }
                }
                else
                {
                    for(int j =0; j < W[0].Count; j++) //!!
                    {
                        if (j == 0)
                        {
                            W[W.Count - Y[Y.Count - 1].Count + i][j] += err[d.Count - 1 - i] * LearningRate;

                        }
                        else
                        {
                            W[W.Count - Y[Y.Count - 1].Count + i][j] += err[d.Count - 1 - i] * LearningRate * X[j - 1];
                        }
                    }
                }



            }

            if (!NoInput)
                {
                Windex.Clear();
                    for(int j = W.Count-1-KolYoutput; j >= 0; j--) 
                    {
                        Windex.Insert(0, j);
                    }
                    KorrektHiddenSloi(Windex);
                }
                else 
                {
                    return;
                }

            

            double Minus(List<double> input,int index) 
            {
                return d[index] - input[index];
            }

            double Derivative(double y, List<double> err, int i)
            {
                return ((1 - y) * y) * err[i];
            }

            void KorrektHiddenSloi(List<int> err) 
            {
                int mob;
                List<int> KOlYhidesl = KolYHidenSloi;//!!!
                KOlYhidesl.Insert(0, KolYinput);
                int o = KOlYhidesl.Count - 1;
                int hob = 0;
                int countSloi = KolSloi + 1;
                int kolNSloi = 0;
                int errLeng = error.Count;
                mob = Y[o].Count;
                if (NoHiden)
                {
                    countSloi -= 1;
                    o = 0;
                }
                for(int i =err.Count-1; i >= 0; i--)
                {
                    error[i] = KorrektError(error, hob, countSloi, kolNSloi, errLeng);
                    hob++;
                    for(int p = W[err[i]].Count-1; p>=0; p--) 
                    {
                        if(i <= KolYinput - 1)
                        {
                            if (p == 0) { W[err[i]][p] += Derivative(Y[o][i], error, i) * LearningRate; continue;}
                            W[i][p] += Derivative(Y[0][i], error, i) * LearningRate * X[p - 1];
                        }else if(p == 0)
                        {
                            W[err[i]][p] += Derivative(Y[o][mob - 1], error, i) * LearningRate;
                        }
                        else
                        {
                            W[err[i]][p] += Derivative(Y[o][mob - 1], error, i) * LearningRate * Y[o - 1][p - 1];
                        }
                    }
                    mob--;
                        if(hob == KOlYhidesl[o] && i != 0) 
                    {
                        errLeng -= Y[o + 1].Count;
                        o--;
                        kolNSloi += Y[countSloi].Count;
                        countSloi--;
                        hob = 0;
                        mob = Y[o].Count;
                    }

                }

                KolYHidenSloi.RemoveAt(0);
            }

            double KorrektError(List<double> error, int hob, int countSloi, int kolNSloi,int errLeng) 
            {
                int l = errLeng;
                double sum = 0;
                List<int> Index2 = new List<int>();
                List<double> ERORW = new List<double>();
                int h = 0;
                int b = 0;
                for(int i = W.Count-1-kolNSloi; i >=0; i--, b++) 
                {
                    if (b < Y[countSloi].Count)
                    {
                        Index2.Insert(0, i);
                    }
                }

                for(int i = Index2.Count-1; i >= Index2.Count-Y[countSloi].Count; i--, h++)
                {
                    ERORW.Insert(0, W[Index2[i]][W[Index2[i]].Count - 1 - hob] * error[l - 1 - h]);
                }

                for(int i =0; i <ERORW.Count; i++) 
                {
                    sum += ERORW[i];
                }
                return sum;
            }

        }

        void SystemClass() 
        {
            SystemCount += 1;
            if(SystemCount < LMX.Count) 
            {
                X = ConvertStrList(ConvertMasStr(LMX[SystemCount].Split(':'),0));
            }
            else 
            {
                if(SystemCount2 >= MD.Count - 1) {SystemCount2 = 0;}
                else { SystemCount2 += 1; }
                d = ConvertStrList(ConvertMasStr(MD[SystemCount2].Split(','), 0));
                SystemCount = 0;
                LMX = ConvertMasStr(MX[SystemCount2].Split(';'),1);
                X = ConvertStrList(ConvertMasStr(LMX[SystemCount].Split(':'), 0));
            }

            KolYoutput = d.Count;
            KolX = X.Count;

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
        }

        public List<double> Recognition(List<List<double>> WInput, List<double> Xinput) 
        {
             W = WInput;
             X = Xinput;
             Y.Clear();
            int count = 0;
            int CountN;

            if(KolYinput != 0)
            {
                CountN = KolYinput;
            }
            else
            {
                CountN = W.Count;
            }
            Y.Add(new List<double>());
             for(int i =0; i < CountN; i++) //первый слой
             {
                Y[Y.Count-1].Add(Neuron(X, i));
                count += 1;
             }
            
             if(KolYHidenSloi[0] != 0) //Скрытые слои
            {
                for(int i =0; i < KolYHidenSloi.Count; i++)
                {
                    Y.Add(new List<double>());
                    for (int j =0; j < KolYHidenSloi[i]; j++) 
                    {
                        Y[Y.Count - 1].Add(Neuron(Y[i], count));
                        count += 1;
                    }
                }
            }

             if(KolYinput != 0)
            {
                Y.Add(new List<double>());
                for (int i =0; i < W.Count - KolYinput; i++) 
                {
                    Y[Y.Count - 1].Add(Neuron(Y[Y.Count - 2], count));
                    count += 1;
                }
            }
             
             return Y[Y.Count-1];
            
        }






    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AMLib
{
    public class ALGConverter
    {
        private int[,] Sumizhnosti;
        private int[,] Incydencii;
        private int kilkDug;
        private int kilkVershyn;
        private List<List<int>>[] pOPList;
        private int listSize;

        public ALGConverter()
        {
            kilkDug = 0;
            kilkVershyn = 0;
            listSize = 0;
        }

        public List<List<int>>[] Convert(Operators opList)
        {
            SetSumizhnostiMatrix(opList);
            ConvertSumizhnostiToIncedance();
            CreateOperatorList();

            return pOPList;
        }

        private void CreateOperatorList()
        {
            int i = 0, j = 0, count = 0, zerocount = 0;
            int[,] mitky = new int[kilkVershyn, 3];
            bool f;
            listSize = 0;
            for (i = 0; i < kilkVershyn; i++)
             {
                count = 0;
                zerocount = 0;
                f = false;
                for (j = 0; j < kilkDug; j++)
                {
                    if (Incydencii[i, j] == 1) 
                    {
                        f = true; 
                        break; 
                    }
                    if (Incydencii[i, j] == -1) count++;
                    if (Incydencii[i, j] == 0) zerocount++;
                }
                if (zerocount == kilkDug)
                {
                    mitky[i, 1] = 1;
                    mitky[i, 2] = 1;
                }
                if (count > 0) 
                {
                    mitky[i, 1] = count;
                    if(!f)
                        mitky[i, 0] = 1; 
                     
                }
            
            }
            for (i = 0; i < kilkVershyn; i++)
            {
                if(mitky[i, 0] == 1 || mitky[i, 2] == 1 || (mitky[i, 1] > 1 && mitky[i, 0] == 0 ))
                    listSize += mitky[i, 1];
            }
            pOPList = new List<List<int>>[listSize];
            for (i = 0; i < listSize; i++)
                pOPList[i] = new List<List<int>>();
            //*******Creating chain*********
            StringBuilder sb = new StringBuilder();
            List<int> list;
            int y = 0, col = 0;
            bool isMoreThanOneMinus = false;
            int listIterator = 0;
            while (FindOneRowInMitky(mitky) != -1) //was TRUE
            {
                int row = FindOneRowInMitky(mitky);
                int temp = row;

                list = new List<int>();
                list.Add(row);

                isMoreThanOneMinus = false;
                col = FindMinusOneColumn(row);
                //якщо багато -1ць то ставимо мітку, яка буде вказівником на те, що одну -1 треба удалити
                if (mitky[temp, 1] > 1 && !isMoreThanOneMinus)
                {
                    isMoreThanOneMinus = true;
                }
                //цикл формування цепочки операторів
                int c_todel = 0, r_todel = 0;
                bool wasDeleted = false;
                while (FindMinusOneColumn(row) != -1)
                {
                    y = FindMinusOneColumn(row);
                    c_todel = y;
                    int x = FindOneRow(y);
                    
                    if (mitky[r_todel, 1] > 1 && mitky[r_todel, 0] !=1)
                    {
                        mitky[r_todel, 1] -= 1;
                        Incydencii[r_todel, c_todel] = 0;
                        wasDeleted = true;
                    }
                    r_todel = x;
                    if (x > 0)
                    {
                        list.Add(x);
                    }
                    else 
                    {
                        list.Add(x);
                        break;
                    }
                    row = x;
                }
                //якщо в стрічці знах. тільки -1. тобто 1 немає взагалі!!
                if (isMoreThanOneMinus) 
                {
                    mitky[temp, 1] -= 1;
                    Incydencii[temp, col] = 0;
                }
                //якщо тільки одна -1, то обнуляємо її після використ.
                if ((mitky[temp, 1] == 1 && !isMoreThanOneMinus) && (!wasDeleted)) 
                {
                    mitky[temp, 1] = 0;
                    mitky[temp, 0] = 0;
                }
                foreach (Object ls in list)
                {
                    sb.Append("F");
                    sb.Append(ls.ToString());
                    sb.Append(" -> ");
                }
                sb.AppendLine();
                pOPList[listIterator].Add(list);
                listIterator++;
            }
            list = new List<int>();
            for (i = 0; i < kilkVershyn; i++)
            {
                if (mitky[i, 1] > 0 && mitky[i, 2] > 0)
                {
                    list.Add(i);
                    sb.Append("F" + i.ToString());
                    sb.AppendLine();
                    pOPList[listIterator].Add(list);
                    listIterator++;
                }
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("P.S: Сформовані цепочки операторів будуються НЕ ПО НОМЕРАХ ОПЕРАТОРІВ, а по ЇХ ПОРЯДКОВОМУ НОМЕРУ У СПИСКУ!!");

            FileStream fs = new FileStream("opModDebug.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(sb.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
            
        }

        private int FindMinusOneColumn(int row)
        {
            int i = row;
            for (int j = kilkDug-1; j >= 0; j--)
                if (Incydencii[i, j] < 0)
                    return j;
            return -1;
        }

        private int FindOneRow(int col)
        {
            int j = col;
            for (int i = kilkVershyn-1; i >= 0; i--)
                if (Incydencii[i, j] > 0)
                    return i;
            return -1;
        }

        private int FindOneRowInMitky(int[,] Mitky)
        {
            for (int i = kilkVershyn-1; i >= 0; i--)
                if (Mitky[i, 0] > 0)
                    return i;
            return -1;
        }

        public void SetSumizhnostiMatrix(Operators opList)
        {
            kilkVershyn = opList.Size;
            Sumizhnosti = new int[kilkVershyn, kilkVershyn];
            int i = 0, j = 0, k = 0;
            List<Operator> list = opList.GetList();
            while (k < kilkVershyn)
            {
                j = 0;
                string str_out = list[k].GetOutParam(0);
                foreach (Operator op in list)
                {
                    for (i = 0; i < op.In; i++)
                    {
                        if (str_out == op.GetInParam(i))
                        {
                            Sumizhnosti[k, j] = 1;
                            kilkDug++;
                            break;
                        }
                    }
                    j++;
                }
                k++;
            }
            /*
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < kilkVershyn; i++)
            {
                for (j = 0; j < kilkVershyn; j++)
                {
                    //Sumizhnosti[i, i] = 0;
                    sb.Append(Sumizhnosti[i, j]);

                }
                sb.AppendLine();
            }
            System.IO.FileStream fs = new System.IO.FileStream("1.txt", System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
            */
        }

        private void ConvertSumizhnostiToIncedance()
        {
            int i = 0, j = 0, k = 0;
            int[,] Dug = new int[kilkDug, 2];
            Incydencii = new int[kilkVershyn, kilkDug];
            //***Convert to Dug Matix****
            for (i = 0; i < kilkVershyn; i++)
            {
                for (j = 0; j < kilkVershyn; j++)
                {
                    if (Sumizhnosti[i, j] == 1)
                    {
                        Dug[k, 0] = i;
                        Dug[k, 1] = j;
                        k++;
                    }
                }
            }
            /*
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < kilkDug; i++)
            {
                sb.Append("L" + i.ToString() + " " + Dug[i, 0] + "  " + Dug[i, 1]);
                sb.AppendLine();
            }
            System.IO.FileStream fs2 = new System.IO.FileStream("2.txt", System.IO.FileMode.Create);
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(fs2);
            sw2.Write(sb.ToString());
            sw2.Flush();
            sw2.Close();
            fs2.Close();*/
            //**********Convert to Incedance Matrix************
            for (i = 0; i < kilkDug; i++)
            {
                Incydencii[Dug[i, 0], i] = 1;
                Incydencii[Dug[i, 1], i] = -1;
            }
            //**********
            /*
            StringBuilder sb2 = new StringBuilder();
            for (i = 0; i < kilkVershyn; i++)
            {
                for (j = 0; j < kilkDug; j++)
                {
                    sb2.Append(Incydencii[i, j]);
                }
                sb2.AppendLine();
            }
            System.IO.FileStream fs = new System.IO.FileStream("3.txt", System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
            sw.Write(sb2.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
            */
        }


    }
}

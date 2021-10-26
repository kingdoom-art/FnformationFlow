using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace lab2
{
    [Serializable]
    public class Matrix
    {
        public int[][] root_mas;
        //сумма элементов в столбце
        public int[] column_mu;
        //сумма элементов в строке
        public int[] row_mu;
        [NonSerialized]
        public static string mu = "  \u03B4";
        //степень матрицы
        public int step = 1;
        public int count;


        public Matrix(List<KeyValuePair<int,int>> mas)
        {
            //стадия инициализации
            count = mas.Select(e => e.Key).Union(mas.Select(e => e.Value)).Count();
            root_mas = new int[count][];
            column_mu = new int[count];
            row_mu = new int[count];

            for(int i = 0; i < count; i++)
            {
                root_mas[i] = new int[count];
            }
            //заполняем матрицу
            foreach(var i in mas)
            {
                root_mas[i.Key - 1][i.Value - 1] = 1;
            }
            //посчитаем всякую всячину
            WriteMU();
        }

        public Matrix(int[][] mas, int _step, int _count)
        {
            root_mas = mas;
            step = _step;
            count = _count;
            row_mu = new int[count];
            column_mu = new int[count];
            WriteMU();
        }

        public Matrix(int[][] root_mas, int[] column_mu)
        {
            this.root_mas = root_mas;
        }

        private void WriteMU()
        {
            //считаем невыговариваемую букву
            for(int i = 0; i < count; i++)
            {
                row_mu[i] = root_mas[i].Sum();
            }

            for(int i = 0; i < count; i++)
            {
                for(int j = 0; j < count; j++)
                {
                    column_mu[i]+=root_mas[j][i];
                }
            }
        }

        public void ShowMatrix()
        {
            Console.WriteLine("A" + step);
            Console.Write("   ");
            for(int i = 0; i < count; i++)
            {
                Console.Write(String.Format("{0,3}", i + 1));
            }
                
            Console.WriteLine(mu);

            for(int i = 0; i < count; i++)
            {
                Console.Write(String.Format("{0,3}", i + 1));
                root_mas[i].ToList().ForEach(e => Console.Write(String.Format("{0,3}", e)));
                Console.WriteLine(String.Format("{0,3}", row_mu[i]));
            }

            Console.Write(mu);
            column_mu.ToList().ForEach(e => Console.Write(String.Format("{0,3}", e)));
            Console.WriteLine();
        }

        public Matrix Pow(Matrix m1)
        {
            int not_zero = 0;
            int[][] result = new int[count][];

            for(int i = 0; i < count; i++)
            {
                result[i] = new int[count];
                for(int j = 0; j < count; j++)
                {
                    for(int k = 0; k < count; k++)
                    {
                        result[i][j] += root_mas[i][k] * m1.root_mas[k][j];
                    }
                }

                //нужно не пропустить нулевую матрицу
                not_zero += result[i].Sum();
            }

            return not_zero != 0 ? new Matrix(result, ++step, count) : null;
        }

        public Matrix Clone()
        {
            Matrix obj = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("matrix.dat", FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
            using (FileStream fs = new FileStream("matrix.dat", FileMode.OpenOrCreate))
            {
                obj = (Matrix)formatter.Deserialize(fs);
            }

            return obj;
        }
    }
}

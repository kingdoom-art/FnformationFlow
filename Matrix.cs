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
        public bool is_sum_matrix = false;
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

        public string GetMatrixName()
        {
            return "A" + (is_sum_matrix ? Program.sum : step.ToString());
        }

        public void ShowMatrix()
        {
            Console.WriteLine(GetMatrixName());
            Console.Write("   ");
            for(int i = 0; i < count; i++)
            {
                Console.Write(String.Format("{0,3}", i + 1));
            }
                
            Console.WriteLine(Program.mu);

            for(int i = 0; i < count; i++)
            {
                Console.Write(String.Format("{0,3}", i + 1));
                root_mas[i].ToList().ForEach(e => Console.Write(String.Format("{0,3}", e)));
                Console.WriteLine(String.Format("{0,3}", row_mu[i]));
            }

            Console.Write(Program.mu);
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

        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix result = a.Clone();
            for (int i = 0; i < a.count; i++)
            {
                for (int j = 0; j < a.count; j++)
                {
                    result.root_mas[i][j] = a.root_mas[i][j] + b.root_mas[i][j];
                }
                result.column_mu[i] = a.column_mu[i] + b.column_mu[i];
                result.row_mu[i] = a.row_mu[i] + b.row_mu[i];
            }

            result.is_sum_matrix = true;
            result.step = -1;

            return result;
        }

        /// <summary>
        /// Возвращает элементы с пустыми входящими\исходящими потоками
        /// </summary>
        /// <param name="flag">По умолчанию - входящие</param>
        /// <returns></returns>
        public IEnumerable<int> GetRowSumIsZero(bool flag = true)
        {
            for(int i = 0; i < count; i++)
            {
                if((column_mu[i] == 0 & flag)||(row_mu[i] == 0 & !flag))
                {
                    yield return i+1;
                }
            }
        }

        /// <summary>
        /// Возвращает элементы с не пустыми входящими\исходящими потоками
        /// </summary>
        /// <param name="flag">По умолчанию - входящие</param>
        /// <returns></returns>
        public IEnumerable<int> GetRowSumNotZero(bool flag = true)
        {
            for (int i = 0; i < count; i++)
            {
                if ((column_mu[i] != 0 & flag) || (row_mu[i] != 0 & !flag))
                {
                    yield return i+1;
                }
            }
        }

        public bool CheckContour()
        {
            for(int i = 0; i < count; i++)
            {
                if(root_mas[i][i] != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод возвращает количество исходящих/входящих потоков из вершины
        /// </summary>
        /// <param name="ind">индекс вершины</param>
        /// <param name="flag">по умолчанию считаем входящие</param>
        /// <returns></returns>
        public int InformationPoint(int ind, bool flag = true)
        {
            return flag ? column_mu[ind] : row_mu[ind];
        }

        public int CountWay(int i, int j)
        {
            return root_mas[i][j];
        }

        public IEnumerable<int> ColumnNotZero(int col)
        {
            for(int i = 0; i < count; i++)
            {
                if(root_mas[i][col] != 0)
                {
                    yield return i;
                }
            }
        }

        public IEnumerable<int> RowNotZero(int row)
        {
            for (int i = 0; i < count; i++)
            {
                if (root_mas[row][i] != 0)
                {
                    yield return i;
                }
            }
        }
    }
}

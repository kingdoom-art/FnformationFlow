using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab2
{
    class Program
    {
        //получение графа формата (Первая_вершина Вторя_вершина)
        static List<KeyValuePair<int,int>> GetGraph()
        {
            List<KeyValuePair<int, int>> mas = new List<KeyValuePair<int, int>>();
            try
            {
                using (StreamReader str = new StreamReader("Graph.txt"))
                {
                    string tmp;
                    while ((tmp = str.ReadLine()) != null)
                    {
                        var tmpMas = tmp.Split(' ').Select(e=>Convert.ToInt32(e)).ToArray();
                        mas.Add(new KeyValuePair<int, int>(tmpMas[0], tmpMas[1]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return mas;
        }

        //Вывод матицы смежности
        static void ShowMatrix(int[,] _mas)
        {
            int row = 0;
            int column = 0;
            for(int i = 0; i < _mas.GetLength(0); i++)
            {
                if (i == 0)
                {
                    Console.Write("   ");
                    for(int k = 1; k <= _mas.GetLength(0); k++)
                    {
                        Console.Write(String.Format("{0,3}", k));
                    }
                    Console.WriteLine("");
                }

                Console.Write(String.Format("{0,3}", i+1));

                for (int j = 0; j < _mas.GetLength(0); j++)
                {
                    if (_mas[i, j] != 0) row++;
                    Console.Write(String.Format("{0,3}",_mas[i,j]));
                }
                Console.WriteLine();
            }
        }

        static int[][] MultMatrix(ref int[][] matrix)
        {
            int[][] matrixResult = new int[matrix.Count()][];
            for (int i = 0; i < matrix.Count(); ++i)
            {
                matrixResult[i] = new int[matrix.Count()];
                for (int j = 0; j < matrix.Count(); ++j)
                    for (int k = 0; k < matrix.Count(); ++k)
                        matrixResult[i][j] += matrix[i][k] * matrix[k][j];
            }
            return matrixResult;
        }

        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Matrix> all_matrix = new List<Matrix>();
            Matrix m = new Matrix(GetGraph());
            Matrix n = m.Clone();
            while(n != null)
            {
                all_matrix.Add(n.Clone());
                n = n.Pow(m);
            }

            Matrix sum_matrix = all_matrix.First().Clone();
            for(int i = 0; i < all_matrix.Count(); i++)
            {
                all_matrix[i].ShowMatrix();
                if (i != 0)
                {
                    sum_matrix += all_matrix[i];
                }
            }

            sum_matrix.ShowMatrix();
            
        }
    }
}

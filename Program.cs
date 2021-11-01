using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab2
{
    class Program
    {
        
        public static string mu = "  \u03B4";
        public static string pi = "\u03c0";
        public static string omega = "\u03c3";
        public static string lyambda = "\u03bb";
        public static string min_j = "\u1da8";
        public static string sum = "\u2a0a";

        static int size_mas = 0;
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

        static IEnumerable<int> GetSequence(int col, int sp = 1)
        {
            while(sp <= col)
            {
                yield return sp++;
            }
        }

        static List<Elements> OrderElements(List<Matrix> mas)
        {
            List<int> m1 = new List<int>();
            List<int> m2 = new List<int>();
            List<int> all_point = new List<int>();
            List<Elements> result = new List<Elements>();
            int count = size_mas;

            for(int i = -1; i < mas.Count(); i++)
            {
                Console.WriteLine(pi + "=" + (i+1));
                if (i == -1)
                {
                    m1 = GetSequence(count).ToList();
                }
                else
                {
                    m1 = mas[i].GetRowSumNotZero().ToList();
                }
                Console.Write(omega + "(" + lyambda + "=" + (i + 1) + ") > 0 для j=");

                Console.WriteLine(String.Join(',', m1));

                if (i+1 >= mas.Count())
                {
                    m2 = GetSequence(count).ToList();
                }
                else
                {
                    m2 = mas[i+1].GetRowSumIsZero().ToList();
                }

                Console.Write(omega + "(" + lyambda + "=" + (i + 2) + ") = 0 для j=");

                Console.WriteLine(String.Join(',', m2));

                Elements el = new Elements(m1, m2, i + 1);

                el.order_el = el.order_el.Except(all_point).ToList();

                all_point = all_point.Union(el.order_el).ToList();

                el.Show();

                result.Add(el);
            }
            return result;
        }

        static int[] ShowResult(List<Elements> mas)
        {
            Console.WriteLine("Результат");
            Console.Write("  j");
            (GetSequence(size_mas)).ToList().ForEach(e => Console.Write(String.Format("{0,3}", e)));
            Console.WriteLine();

            int[] m = new int[size_mas];

            foreach(var i in mas)
            {
                i.order_el.ForEach(e => m[e-1] = i.order_lvl);
            }

            Console.Write("  "+pi);
            m.ToList().ForEach(e => Console.Write(String.Format("{0,3}", e)));
            Console.WriteLine();
            return m;
        }

        static void ShowTact(int[] mas)
        {
            Console.WriteLine("2. Определение «тактности» системы.");
            Console.Write("N = max " + pi + " = ");
            int m = mas.Max();
            Console.WriteLine(m);
        }

        static void CheckContour(List<Matrix> mas)
        {
            Console.WriteLine("3. Отсутствие ненулевых элементов на главной диагонали");
            foreach (var m in mas)
            {
                if (m.CheckContour())
                {
                    Console.WriteLine("Контур есть у матрицы "+m.GetMatrixName());
                    return;
                }
            }
            Console.WriteLine("B анализируемом документообороте контуров нет");
        }

        static void InputElements(Matrix matrix)
        {
            Console.WriteLine("4. Определение входных элементов потока");
            var m1 = matrix.GetRowSumIsZero().ToList();
            Console.Write(omega + "(" + lyambda + "=" + (matrix.step) + ") = 0 для j=");

            Console.WriteLine(String.Join(',', m1));

            Random rand = new Random();
            int el = rand.Next(matrix.count);
            Console.Write("С другой стороны, "+omega+(el+1)+ "(" + lyambda + "=1)=");
            int len = matrix.InformationPoint(el);
            Console.WriteLine(len + " означает, что в X"+(el+1)+" входит элементов - "+len);
        }

        static void OutputElements(Matrix matrix)
        {
            Console.WriteLine("5. Определение выходных элементов потока");
            var m1 = matrix.GetRowSumIsZero(false).ToList();
            Console.Write(omega + "(" + lyambda + "=" + (matrix.step) + ") = 0 для j=");

            Console.WriteLine(String.Join(',', m1));
        }

        static void FlyElements(Matrix matrix)
        {
            Console.WriteLine("6. Определение висящих вершин");
            var m1 = matrix.GetRowSumIsZero(false).ToList();
            var m2 = matrix.GetRowSumIsZero().ToList();
            var m_res = m1.Except(m2);

            if (m_res.Count() == 0)
            {
                Console.Write(omega + "i(" + lyambda + "=1) = "+omega+ "j(" + lyambda + "=1)=0 ");
                Console.WriteLine("отсутствует, т.е. висящих вершин нет");
            }
            else
            {
                Console.WriteLine("Висящие вершины: "+String.Join(',', m_res));
            }
        }

        static void CountWay(Matrix matrix)
        {
            int len = matrix.step;
            Console.WriteLine("7. Определение числа путей длиной "+lyambda+"="+len);
            Random rand = new Random();
            int i = rand.Next(size_mas);
            int j;
            while ((j = rand.Next(size_mas)) == i) ;
            Console.Write("a"+(i+1)+","+(j+1) + "(" + lyambda + "=" + len + ") = ");

            int cnt = matrix.CountWay(i, j);

            Console.WriteLine(cnt);
        }

        static void AllWay(Matrix matrix)
        {
            Console.WriteLine("8. Определение всевозможных путей между двумя элементами");

            Random rand = new Random();
            int i = rand.Next(size_mas);
            int j;
            while ((j = rand.Next(size_mas)) == i) ;

            int cnt = matrix.CountWay(i, j);

            Console.WriteLine("a" + (i + 1) + "," + (j + 1) + "(" + sum + ") = " + cnt);
        }

        static void AssociatedElements(Matrix matrix)
        {
            Console.WriteLine("9. Определение всех элементов, участвующих в формировании данного");
            Random rand = new Random();
            int i = rand.Next(size_mas);

            var in_mas = matrix.ColumnNotZero(i).ToList();
            Console.WriteLine("В формировании X" + (i + 1) + " участвуют:");
            in_mas.ForEach(e => Console.WriteLine("X" + (e + 1) + " количество раз - " + matrix.CountWay(e, i)));

            i = rand.Next(size_mas);

            in_mas = matrix.RowNotZero(i).ToList();
            Console.WriteLine("Элемент Х"+(i+1)+" использоваляся для формирования элементов:");
            in_mas.ForEach(e => Console.WriteLine("для X" + (e + 1) + " количество раз - " + matrix.CountWay(i, e)));
        }

        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            List<Matrix> all_matrix = new List<Matrix>();
            Matrix m = new Matrix(GetGraph());
            size_mas = m.count;
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

            //формируем порядок элементов
            List<Elements> mas_el = OrderElements(all_matrix);

            //выводим результирующую таблицу порядков элементов
            int[] res_order = ShowResult(mas_el);

            //определяем тактность системы
            ShowTact(res_order);
            //поиск контуров во всех матрицах
            CheckContour(all_matrix);
            //определение входных элементов
            InputElements(all_matrix.First());
            //Определение выходных элементов потока
            OutputElements(all_matrix.First());
            //Определение висящих вершин
            FlyElements(all_matrix.First());
            //Определение числа путей длиной
            int len = (new Random()).Next(all_matrix.Count());
            CountWay(all_matrix[len]);
            //Определение всевозможных путей между двумя элементами
            AllWay(sum_matrix);
            //Определение всех элементов, участвующих в формировании данного
            AssociatedElements(sum_matrix);
        }
    }
}

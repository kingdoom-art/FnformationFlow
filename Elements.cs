using System;
using System.Linq;
using System.Collections.Generic;

namespace lab2
{
    public class Elements
    {
        public List<int> in_mas;
        public List<int> out_mas;
        public List<int> order_el;
        public int order_lvl;

        public Elements(List<int> in_m, List<int> out_m, int lvl)
        {
            in_mas = in_m;
            out_mas = out_m;
            order_lvl = lvl;
            order_el = GetOrderElement();
        }

        private List<int> GetOrderElement()
        {
            order_el = in_mas.Intersect(out_mas).ToList();

            return order_el;
        }

        public void Show()
        {
            if (order_el.Count() == 0)
            {
                Console.WriteLine("Xn - пустое");
                return;
            }
            Console.WriteLine(String.Join(',', order_el.Select(e => "X" + e)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            DateTime startDate = new DateTime(2018, 01, 1, 00, 00, 00);
            TestStruct element = new TestStruct(startDate);
            DateTime endDate = new DateTime(2018, 01, 1, 1, 00, 00);
            int ind1 = element.DataToIndex(startDate);
            int ind2 = element.DataToIndex(endDate);
            while (startDate != endDate)
            {
                element.AddElem(rand.Next(10));
                startDate = startDate.AddSeconds(1);
            }
            DateTime date1 = new DateTime(2018, 01, 1, 00, 10, 00);
            DateTime date2 = new DateTime(2018, 01, 1, 00, 43, 00);
            Console.Write("Список значений");
            Console.WriteLine();
            List<double> list = new List<double>(element.GetValuesRange(date1, date2));
            foreach (int i in list)
            {
                Console.Write(i + " ");
            }
            Console.Write("Текущая дата: " + startDate);
            Console.WriteLine();
            Console.Write("Размер списка: " + list.Count);
            Console.WriteLine();
            double mean = element.MeanValueForMonth();
            Console.Write("Среднее значение по всем данным  = " + mean);
            Console.WriteLine();
            double meanValue = element.MeanValue(date1, date2);
            Console.Write("Среднее значение между датами = " + meanValue);
            Console.ReadKey();
        }
    }
}

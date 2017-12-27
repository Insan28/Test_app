using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_app
{
    class TestStruct
    {
        private const int size_max = 60 * 60 * 24 * 31;//кол-во чисел в месяц
        private int index = 0;
        private double[] buffer = new double[size_max];//массив для численных значений
        private double[] sumVal = new double[11];//массив сумм значений после каждого месяца
        private double sum = 0;
        private double partialSum = 0;
        private double[] partSum = new double[2232];
        private int index_part = 1;
        DateTime nowDate = new DateTime(2018, 01, 1, 00, 00, 00);
        DateTime startDate = new DateTime(2018, 01, 1, 00, 00, 00);
        private bool bufferIsFull = false;
        public TestStruct() { }
        public TestStruct(DateTime date)
        {
            startDate = date;
        }

        public int DataToIndex(DateTime date)
        {
            if (date < startDate)
            {
                return -1;
            }
            int ind = 0;
            ind += (date.Day - 1) * 60 * 60 * 24;
            ind += date.Hour * 60 * 60;
            ind += date.Minute * 60;
            ind += date.Second;
            if (ind < size_max)
            {
                if (bufferIsFull)
                    ind = ind + index % size_max;
                return ind;
            }
            else return -1;
        }

        public void AddElem(int value)
        {
            buffer[index] = value;
            sum += value;
            partialSum += value;
            index++;
            nowDate = nowDate.AddSeconds(1);
            if (index == index_part * 60 * 20)
            {
                partSum[index_part - 1] = partialSum;
                partialSum = 0;
                index_part++;
            }
            if (index == size_max)
            {
                index = 0;
                bufferIsFull = true;
            }
            if (bufferIsFull)
            {
                sum -= buffer[index];
                startDate = startDate.AddSeconds(1);
            }
        }

        public List<double> GetValuesRange(DateTime date1, DateTime date2)
        {
            if (date1 > date2)
            {
                Console.Write("Right value must be bigger than the left value");
                return null;
            }
            if (date1 > nowDate)
            {
                Console.Write("Информации по первой дате нету!");
                return null;
            }
            if (date2 > nowDate)
            {
                Console.Write("Информации по второй дате нету!");
                return null;
            }
            int ind_1 = DataToIndex(date1);
            int ind_2 = DataToIndex(date2);
            if (ind_1 < 0)
            {
                Console.Write("Первая дата не верная!");
                return null;
            }
            if (ind_2 < 0)
            {
                Console.Write("Вторая дата не верная!");
                return null;
            }
            if (!bufferIsFull)
            {
                if (ind_1 > index)
                {
                    Console.Write("Информации по первой дате нету!");
                    return null;
                }
                if (ind_2 > index)
                {
                    Console.Write("Информации по второй дате нету!");
                    return null;
                }
            }
            List<double> list = new List<double>();
            if (ind_1 < ind_2)
            {
                for (int i = ind_1; i < ind_2; i++)
                {
                    list.Add(buffer[i]);
                }
            }
            else
            {
                for (int i = ind_1; i < size_max; i++)
                {
                    list.Add(buffer[i]);
                }
                for (int i = 0; i < ind_2; i++)
                {
                    list.Add(buffer[i]);
                }
            }
            return list;
        }

        public double MeanValue(DateTime date1, DateTime date2)
        {
            if (date1 > date2)
            {
                Console.Write("Вторая дата должна быть больше чем первая!");
                return 0;
            }
            if (date1 > nowDate)
            {
                Console.Write("Информации по первой дате нету!");
                return 0;
            }
            if (date2 > nowDate)
            {
                Console.Write("Информации по второй дате нету!");
                return 0;
            }
            int ind1 = DataToIndex(date1);
            int ind2 = DataToIndex(date2);
            if (ind1 < 0)
            {
                Console.Write("Первая дата не верная!");
                return 0;
            }
            if (ind2 < 0)
            {
                Console.Write("Вторая дата не верная!");
                return 0;
            }
            if (!bufferIsFull)
            {
                if (ind1 > index)
                {
                    Console.Write("Информации по первой дате нету!");
                    return 0;
                }
                if (ind2 > index)
                {
                    Console.Write("Информации по второй дате нету!");
                    return 0;
                }
            }
            int number_elem = 0;
            if (ind1 < ind2)
            {
                number_elem = ind2 - ind1;
            }
            else
            {
                number_elem = size_max - (ind1 - ind2);
            }
            int ind1_in_mas = 0;
            int ind2_in_mas = 0;
            for (int i = 0; i < partSum.Length; i++)
            {
                if ((ind1 >= i * 60 * 20) && (ind1 < (i + 1) * 60 * 20))
                {
                    ind1_in_mas = i;
                }
                if ((ind2 >= i * 60 * 20) && (ind2 < (i + 1) * 60 * 20))
                {
                    ind2_in_mas = i;
                }
            }
            double summa = 0;
            double mean = 0;
            if (ind2_in_mas - ind1_in_mas > 1)
            {
                for (int i = ind1_in_mas + 1; i < ind2_in_mas; i++)
                {
                    summa += partSum[i];
                }
                for (int i = ind1; i < (ind1_in_mas + 1) * 60 * 20; i++)
                {
                    summa += buffer[i];
                }
                for (int i = ind2_in_mas * 60 * 20; i < ind2; i++)
                {
                    summa += buffer[i];
                }
            }
            else if (ind2_in_mas - ind1_in_mas < 0)
            {
                for (int i = ind1_in_mas + 1; i < partSum.Length; i++)
                {
                    summa += partSum[i];
                }
                for (int i = 0; i < ind2_in_mas; i++)
                {
                    summa += partSum[i];
                }
                for (int i = ind1; i < (ind1_in_mas + 1) * 60 * 20; i++)
                {
                    summa += buffer[i];
                }
                for (int i = ind1_in_mas * 60 * 20; i < ind1; i++)
                {
                    summa += buffer[i];
                }
            }
            else
            {
                if (ind1 < ind2)
                {
                    for (int i = ind1; i < ind2; i++)
                    {
                        summa += buffer[i];
                    }
                }
                else
                {
                    for (int i = ind2; i < ind1; i++)
                    {
                        summa += buffer[i];
                    }
                }
            }
            mean = summa / number_elem;
            return mean;
        }

        public double MeanValueForMonth()
        {
            double mean = 0;
            if (!bufferIsFull)
            {
                mean = sum / (index + 1);
            }
            mean = sum / size_max;
            return mean;
        }
    }
}

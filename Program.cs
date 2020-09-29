using System;

namespace FunctionBuilder
{
    class Program
    {
        static double TheFunction(float x)
        {
            return Math.Pow(x,(Math.Pow(-1, x)) + x * x);
        }

        static double GetDouble()
        {
            string str;
            bool repeat;
            do
            {
                str = Console.ReadLine().Replace('.', ',');
                repeat = false;

                for (int i = 0; i < str.Length; i++)
                {
                    if (!(Char.IsDigit(str, i) || str[i] == ',')) 
                    { 
                        repeat = true; 
                        Console.Write("Введите корректное число: "); 
                        break; 
                    }
                }
            } while (repeat);

            return Convert.ToDouble(str);
        }

        static void Main(string[] args)
        {
            double x;
            Console.WriteLine("Ваша функция: Y=X^((-1)^X)+X^2");

            Console.Write("Введите шаг: ");
            x = GetDouble();
            Console.WriteLine(x);
            
        }
    }
}

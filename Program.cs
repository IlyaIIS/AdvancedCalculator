using System;

namespace FunctionBuilder
{
    class Program
    {
        static double GetY(double x)            //Сама функция (вычисляет F(x))
        {
            return Math.Round(Math.Sqrt(Math.Abs(x))*Math.Sin(x*(Math.Pow(-1,Math.Round(x))-1)))+x*(Math.Pow(-1,Math.Round(x))+1);
        }

        static double GetDouble()               //Получение от пользователя корректноего числа типа Double
        {
            string str;
            bool repeat;
            double output1, output2;
            do
            {
                str = Console.ReadLine();

                str = str.Replace('.', ',');
                repeat = !Double.TryParse(str, out output1);
                str = str.Replace(',', '.');
                repeat = !Double.TryParse(str, out output2) && repeat;
                if (repeat) Console.Write("Введите корректное число: ");
            } while (repeat);
            if (output1 == output2) output1 = 0;
            return output1 + output2;
        }

        static void DrawResult(double step, double xStart, double xEnd)    //Отрисовка результата
        {
            static int DigitSize(double d)            //Сколько символов в числе
            {
                return Convert.ToString(d).Length;
            }

            static void DrawMark(char mark, int n)    //Отрисовка знака mark n раз
            {
                for (int i = 1; i <= n; i++)
                {
                    Console.Write(mark);
                }
            }

            double x, y;
            int maxSize = 0;

            //Вычесление максимального размера числа из X и Y
            x = xStart;
            do
            {
                y = GetY(x);
                if (DigitSize(y) > maxSize) { maxSize = DigitSize(y); }
                if (DigitSize(x) > maxSize) { maxSize = DigitSize(x); }
                x += step;
            } while ((step > 0 && x <= xEnd) || (step < 0 && x >= xEnd));
            maxSize += 2;

            //Отрисовка
            Console.Write('┌');
            DrawMark('─', maxSize);
            Console.Write('┬');
            DrawMark('─', maxSize);
            Console.WriteLine('┐');

            Console.Write('│');
            DrawMark(' ', (maxSize - 1) / 2);
            Console.Write('X');
            DrawMark(' ', (int)Math.Ceiling((double)(maxSize - 1) / 2));
            Console.Write('│');
            DrawMark(' ', (maxSize - 1) / 2);
            Console.Write('Y');
            DrawMark(' ', (int)Math.Ceiling((double)(maxSize - 1) / 2));
            Console.WriteLine('│');

            Console.Write('├');
            DrawMark('─', maxSize);
            Console.Write('┼');
            DrawMark('─', maxSize);
            Console.WriteLine('┤');

            x = xStart;
            do
            {
                y = GetY(x);

                Console.Write('│');
                DrawMark(' ', (maxSize - DigitSize(x)) / 2);
                Console.Write(x);
                DrawMark(' ', (int)Math.Ceiling((double)(maxSize - DigitSize(x)) / 2));
                Console.Write('│');
                DrawMark(' ', (maxSize - DigitSize(y)) / 2);
                Console.Write(y);
                DrawMark(' ', (int)Math.Ceiling((double)(maxSize - DigitSize(y)) / 2));
                Console.WriteLine('│');

                x += step;
            } while ((step > 0 && x <= xEnd) || (step < 0 && x >= xEnd));

            Console.Write('└');
            DrawMark('─', maxSize);
            Console.Write('┴');
            DrawMark('─', maxSize);
            Console.WriteLine('┘');
        }

        static void Main(string[] args)
        {
            double step, xStart, xEnd;
            Console.WriteLine("Функция по умолчанию: Y = [sqrt(|X|)*sin(X*((-1)^[X]-1))] + X((-1)^[X]+1)");

            Console.Write("Введите шаг: ");
            do
            {
                step = GetDouble();
                if (step == 0) { Console.Write("Зачем ты это делаешь? Попробуй снова: "); }
            } while (step == 0);
            Console.Write("Ведите Х начальное: ");
            xStart = GetDouble();
            Console.Write("Ведите Х конечное: ");
            xEnd = GetDouble();

            if ((xStart < xEnd && step > 0) || (xStart > xEnd && step < 0)) DrawResult(step, xStart, xEnd); 
            else { DrawResult(step, xEnd, xStart);
                   Console.WriteLine("Начальное и конечное положение заменены");           }

        }
    }
}

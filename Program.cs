﻿using System;

namespace FunctionBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            double step, xStart, xEnd;
            Console.WriteLine("Функция по умолчанию: Y = [sqrt(|X|)*sin(X*((-1)^[X]-1))] + X((-1)^[X]+1)");

            step = GetDouble("Введите шаг: ");
            xStart = GetDouble("Ведите Х начальное: ");
            xEnd = GetDouble("Ведите Х конечное: ");

            if ((xStart < xEnd && step > 0) || (xStart > xEnd && step < 0)) DrawResult(step, xStart, xEnd);
            else
            {
                DrawResult(step, xEnd, xStart);
                Console.WriteLine("Начальное и конечное положение заменены");
            }
        }

        //Сама функция (вычисляет F(x))
        static double GetY(double x)            
        {
            return Math.Round(Math.Sqrt(Math.Abs(x))*Math.Sin(x*(Math.Pow(-1,Math.Round(x))-1)))+x*(Math.Pow(-1,Math.Round(x))+1);
        }

        static double GetDouble(string str)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru");
            double output;
            bool repeat;
            int position = Console.CursorTop;

            Console.Write(str);
            do
            {
                repeat = !Double.TryParse(Console.ReadLine().Replace(".", ","), out output);
                if (repeat)
                {
                    Console.SetCursorPosition(0, position);
                    Console.Write("Введите корректное число!" + new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, position + 1);
                    Console.ReadKey(true);
                    Console.SetCursorPosition(0, position);
                    Console.Write(str + new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(str.Length, position);
                }
            } while (repeat);

            return output;
        }

        //Отрисовка результата
        static void DrawResult(double step, double xStart, double xEnd)    
        {
            double x, y;
            int maxSize = GetMaxSizeXY(2, xStart, step, xEnd);

            //Отрисовка
            DrawBorder('─', '┌', '┬', '┐', maxSize);

            DrawMid(' ', '│', "X", "Y", (maxSize - 1) / 2, (maxSize - 1) / 2,
                                                            (int)Math.Ceiling((double)(maxSize - 1) / 2),
                                                            (int)Math.Ceiling((double)(maxSize - 1) / 2));

            DrawBorder('─', '├', '┼', '┤', maxSize);

            x = xStart;
            do
            {
                y = GetY(x);

                DrawMid(' ', '│', Convert.ToString(x), Convert.ToString(y), 
                 (maxSize - DigitSize(x)) / 2,
                 (maxSize - DigitSize(y)) / 2,
                 (int)Math.Ceiling((double)(maxSize - DigitSize(x)) / 2), 
                 (int)Math.Ceiling((double)(maxSize - DigitSize(y)) / 2));

                x += step;
            } while ((step > 0 && x <= xEnd) || (step < 0 && x >= xEnd));

            DrawBorder('─', '└', '┴', '┘', maxSize);
        }

        static void DrawBorder(char char0, char char1, char char2, char char3, int maxSize)
        {
            Console.Write(char1);
            Console.Write(new string(char0, maxSize));
            Console.Write(char2);
            Console.Write(new string(char0, maxSize));
            Console.WriteLine(char3);
        }
        static void DrawMid(char char0, char char1, string char2, string char3, int maxSize1, int maxSize2, int maxSize3, int maxSize4)
        {
            Console.Write(char1);
            DrawMark(char0, maxSize1);
            Console.Write(char2);
            DrawMark(char0, maxSize3);
            Console.Write(char1);
            DrawMark(char0, maxSize2);
            Console.Write(char3);
            DrawMark(char0, maxSize4);
            Console.WriteLine(char1);
        }

        static int GetMaxSizeXY(int maxSize, double x, double step, double xEnd)
        {
            double y;
            do
            {
                y = GetY(x);
                if (DigitSize(y) > maxSize) { maxSize = DigitSize(y); }
                if (DigitSize(x) > maxSize) { maxSize = DigitSize(x); }
                x += step;
            } while ((step > 0 && x <= xEnd) || (step < 0 && x >= xEnd));

            return maxSize;
        }

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
    }
}

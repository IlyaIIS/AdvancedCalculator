using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FunctionBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (IsFileCorrect())
            {
                string[] input = ReadFile();

                string formula = input[0];
                double step = Convert.ToDouble(input[1]);
                double xStart = Convert.ToDouble(input[2]);
                double xEnd = Convert.ToDouble(input[3]);

                string[] opn = ParseExpression(formula);

                //foreach (string element in ParseExpression(formula)) Console.WriteLine(element);

                File.WriteAllText("../../../output.txt", "gg");
            }
        }
        static string[] ReadFile()
        {
            string[] output = new string[4];
            int i = 0;
            foreach (string element in File.ReadLines("../../../input.txt"))
            {
                output[i] = element;
                i++;
            }

            return output;
        }

        static bool IsFileCorrect()
        {
            try
            {
                if (File.ReadLines("../../../input.txt").Count() == 4) return true;
                else
                {
                    Console.WriteLine("Введите в файле 4 строки: формула, шаг, Х начальное, Х конечное.");
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Файл не обнаружен (поместите файл input.txt в папку с программой)");
                return false;
            }
        }
        //////////////////////////////////////////////////ОБРАТНАЯ ПОЛЬСКАЯ ЗАПИСТЬ//////////////////////////////////////

        static string[] signsList = { "+", "-", "*", "/", "^", "(", ")", "sin", "log" };
        static Dictionary<string, int> ordersList = new Dictionary<string, int>
        {
            {"(", 0},
            {"+", 1},
            {"-", 1},
            {"*", 2},
            {"/", 2},
            {"^", 3},
            {"sin", 10},
            {"log", 10}
        };
        static string[] ParseExpression(string input)
        {
            string[] pInput = ParseInput(input);
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            string[] output;

            for (int i = 0; i < pInput.Length; i++)
            {
                if (Char.IsDigit(pInput[i][0])) firstList.Add(pInput[i]);
                else
                if (pInput[i][0] == '(') secondList.Add(pInput[i]);
                else
                if (pInput[i][0] == ')')
                {
                    for (int ii = secondList.Count - 1; ii >= 0; ii--)
                    {
                        if (secondList[ii] == "(")
                        {
                            secondList.RemoveAt(secondList.Count - 1);
                            break;
                        }
                        else
                        {
                            firstList.Add(secondList[secondList.Count - 1]);
                            secondList.RemoveAt(secondList.Count - 1);
                        }
                    }
                }
                else
                if (signsList.Contains(pInput[i]))
                {
                    if (secondList.Count >= 1)
                    {
                        if (ordersList[secondList[secondList.Count - 1]] >= ordersList[pInput[i]])
                        {
                            firstList.Add(secondList[secondList.Count - 1]);
                            secondList.RemoveAt(secondList.Count - 1);
                        }
                    }
                    secondList.Add(pInput[i]);
                }
            }

            for (int i = secondList.Count - 1; i >= 0; i--) firstList.Add(secondList[i]);

            output = new string[firstList.Count];
            for (int i = 0; i < output.Length; i++) output[i] = firstList[i];

            foreach (string element in output) Console.WriteLine(element);

            return output;
        }

        static string[] ParseInput(string input)
        {
            List<string> preOutput = new List<string>();
            string[] output;
            input += "  ";

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    preOutput.Add("");
                    for (; i < input.Length; i++)
                    {
                        if (Char.IsDigit(input[i]) || input[i] == '.' || (input[i] == ',' && Char.IsDigit(input[i + 1])))
                        {
                            preOutput[preOutput.Count - 1] += input[i];
                        }
                        else
                        {
                            i--;
                            break;
                        }
                    }
                }
                else
                if (signsList.Contains(Convert.ToString(input[i])))
                {
                    preOutput.Add(Convert.ToString(input[i]));
                }
                else
                if (input[i] != ' ' && input[i] != ',')
                {
                    string local = "";
                    for (; i < input.Length; i++)
                    {
                        local += input[i];
                        if (signsList.Contains(Convert.ToString(local))) break;
                    }
                    preOutput.Add(local);
                }
            }

            output = new string[preOutput.Count];
            for (int i = 0; i < output.Length; i++) output[i] = preOutput[i];

            return output;
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

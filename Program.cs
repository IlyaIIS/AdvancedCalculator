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

                WriteResult(formula, step, xStart, xEnd);
            }
        }
        /////////////////////////////////////////////////////////КОНСТАНТЫ///////////////////////////////////////////////
        static string[] signsList = { "+", "-", "*", "/", "^", "(", ")", "sin", "log", "!" };
        static Dictionary<string, int> ordersList = new Dictionary<string, int>
        {
            {"(", 0},
            {"+", 1},
            {"-", 1},
            {"*", 2},
            {"/", 2},
            {"^", 3},
            {"sin", 10},
            {"log", 10},
            {"!", 10}
        };
        /////////////////////////////////////////////////////РАБОТА С ФАЙЛАМИ/////////////////////////////////////////////
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
        static List<string> ParseExpression(string input)
        {
            string[] pInput = ParseInput(input);
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            bool isDebugEnabled = false;

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

                if (isDebugEnabled)
                {
                    for (int ii = 0; ii < pInput.Length; ii++)
                    {
                        if (ii == i) Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(pInput[ii] + " ");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                    Console.WriteLine();

                    string local = "";
                    for (int ii = 0; ii < firstList.Count; ii++) local += firstList[ii] + " ";
                    Console.WriteLine("Первая строка: " + local);
                    local = "";
                    for (int ii = 0; ii < secondList.Count; ii++) local += secondList[ii] + " ";
                    Console.WriteLine("Вторая строка: " + local);

                    Console.ReadKey(true);
                    Console.Clear();
                }
            }

            for (int i = secondList.Count - 1; i >= 0; i--) firstList.Add(secondList[i]);

            return firstList;
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
                    if (input[i] == '-')
                        if (i > 0)
                        {
                            if (!Char.IsDigit(input[i - 1]) && (input[i - 1] == '(' || input[i - 1] != ')'))
                                preOutput.Add("0");
                        }
                        else
                            preOutput.Add("0");
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
        /////////////////////////////////////////////////////КАЛЬКУЛЯТОР//////////////////////////////////////////////////
        static double Calculate(List<string> input)
        {
            for(int i = 0; i < input.Count; i++)
            {
                if (signsList.Contains(input[i]))
                {
                    switch (input[i])
                    {
                        case "+":
                            input[i] = Convert.ToString(Convert.ToDouble(input[i - 2]) + Convert.ToDouble(input[i - 1]));
                            input.RemoveAt(i - 1);
                            input.RemoveAt(i - 2);
                            i -= 2;
                            break;
                        case "-":
                            input[i] = Convert.ToString(Convert.ToDouble(input[i - 2]) - Convert.ToDouble(input[i - 1]));
                            input.RemoveAt(i - 1);
                            input.RemoveAt(i - 2);
                            i -= 2;
                            break;
                        case "*":
                            input[i] = Convert.ToString(Convert.ToDouble(input[i - 2]) * Convert.ToDouble(input[i - 1]));
                            input.RemoveAt(i - 1);
                            input.RemoveAt(i - 2);
                            i -= 2;
                            break;
                        case "/":
                            input[i] = Convert.ToString(Convert.ToDouble(input[i - 2]) / Convert.ToDouble(input[i - 1]));
                            input.RemoveAt(i - 1);
                            input.RemoveAt(i - 2);
                            i -= 2;
                            break;
                        case "^":
                            input[i] = Convert.ToString(Math.Pow(Convert.ToDouble(input[i - 2]), Convert.ToDouble(input[i - 1])));
                            input.RemoveAt(i - 1);
                            input.RemoveAt(i - 2);
                            i -= 2;
                            break;
                        case "sin":
                            input[i] = Convert.ToString(Math.Sin(Convert.ToDouble(input[i - 1])));
                            input.RemoveAt(i - 1);
                            i -= 1;
                            break;
                        case "log":
                            input[i] = Convert.ToString(Math.Log10(Convert.ToDouble(input[i - 2])) / Math.Log10((Convert.ToDouble(input[i - 1]))));
                            input.RemoveAt(i - 1);
                            input.RemoveAt(i - 2);
                            i -= 2;
                            break;
                        case "!":
                            double x = 1;
                            for (int ii = 2; ii <= Convert.ToInt32(input[i - 1]); ii++) x *= ii;
                            input[i] = Convert.ToString(x);
                            input.RemoveAt(i - 1);
                            i -= 1;
                            break;
                    }
                }
            }

            return Convert.ToDouble(input[0]);
        }

        /////////////////////////////////////////////////////ВЫВОД РЕЗУЛЬТАТА//////////////////////////////////////////////////
        static void WriteResult(string formula, double step, double xStart, double xEnd)    
        {
            double x, y;
            int maxSize = GetMaxSizeXY(formula, 2, xStart, step, xEnd);
            string text = "";

            //Отрисовка
            text = WriteBorder('─', '┌', '┬', '┐', maxSize, text);

            text = WriteMid(' ', '│', "X", "Y", (maxSize - 1) / 2, (maxSize - 1) / 2,
                                                            (int)Math.Ceiling((double)(maxSize - 1) / 2),
                                                            (int)Math.Ceiling((double)(maxSize - 1) / 2), text);

            text = WriteBorder('─', '├', '┼', '┤', maxSize, text);

            x = xStart;
            do
            {
                y = GetY(formula, x);

                text = WriteMid(' ', '│', Convert.ToString(x), Convert.ToString(y), 
                    (maxSize - DigitSize(x)) / 2,
                    (maxSize - DigitSize(y)) / 2,
                    (int)Math.Ceiling((double)(maxSize - DigitSize(x)) / 2), 
                    (int)Math.Ceiling((double)(maxSize - DigitSize(y)) / 2), text);

                x += step;
            } while ((step > 0 && x <= xEnd) || (step < 0 && x >= xEnd));

            text = WriteBorder('─', '└', '┴', '┘', maxSize, text);

            File.WriteAllText("../../../output.txt", text);
        }
        static double GetY(string formula, double x)
        {
            return Calculate(ParseExpression(formula.Replace("x", Convert.ToString(x))));
        }

        static string WriteBorder(char char0, char char1, char char2, char char3, int maxSize, string text)
        {
            return text + char1 + new string(char0, maxSize) + char2 + new string(char0, maxSize) + char3 + "\n";
        }
        static string WriteMid(char char0, char char1, string char2, string char3, int maxSize1, int maxSize2, int maxSize3, int maxSize4, string text)
        {
            return text + char1 + new string(char0, maxSize1) + char2 + new string(char0, maxSize3) + char1 + 
                new string(char0, maxSize2) + char3 + new string(char0, maxSize4) + char1 + "\n";
        }

        static int GetMaxSizeXY(string formula, int maxSize, double x, double step, double xEnd)
        {
            double y;
            do
            {
                y = GetY(formula, x);
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
    }
}

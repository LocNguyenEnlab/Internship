using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    internal class UserInterface
    {
        private char _operator;
        private List<double> _listNumbers;

        public UserInterface()
        {
        }

        internal void ShowMenuOperator()
        {
            Console.WriteLine("=====>MENU<=====");
            Console.WriteLine("+: Add numbers");
            Console.WriteLine("-: Subtract numbers");
            Console.WriteLine("*: Multiple numbers");
            Console.WriteLine("/: Divide numbers");
            Console.Write("Enter operator: ");
            while (true)
            {
                Char.TryParse(Console.ReadLine(), out _operator);
                if (_operator == '+' || _operator == '-' || _operator == '*' || _operator == '/')
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid operator, please input operator again: ");
                }
            }
        }

        internal void Inform(string v)
        {
            Console.WriteLine(v);
        }

        internal void ShowInputNumbers()
        {
            _listNumbers = new List<double>();
            Console.Write("Enter numbers you want to calculate, Enter any other characters numbers to finish: ");
            while(true)
            {
                try
                {
                    double x = Convert.ToDouble(Console.ReadLine());
                    _listNumbers.Add(x);
                } catch(Exception)
                {
                    break;
                }
                
            }
        }

        internal char GetOperator()
        {
            return _operator;
        }

        internal List<double> GetListInputNumbers()
        {
            return _listNumbers;
        }

        internal void ShowResult(double result, Operators operators, List<double> listNumbers)
        {
            for (int i = 0; i < listNumbers.Count; i++)
            {
                Console.Write("({0})", listNumbers[i]);
                if (i == listNumbers.Count - 1)
                {
                    break;
                }
                else
                {
                    Console.Write(" {0} ", (char)operators);
                }
            }
            Console.WriteLine(" = {0}", result);
        }
    }
}
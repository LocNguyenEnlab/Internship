using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var ui = new UserInterface();
            var logic = new CalculateService();
            double result;

            ui.ShowMenuOperator();
            ui.ShowInputNumbers();
            Operators operatorInput = (Operators)ui.GetOperator();
            var listNumbers = ui.GetListInputNumbers();

            if(listNumbers.Count < 2)
            {
                ui.Inform("Not enough numbers to calculate!");
            }
            else
            {
                switch (operatorInput)
                {
                    case Operators.Add:
                        result = logic.Add(listNumbers);
                        ui.ShowResult(result, Operators.Add, listNumbers);
                        break;
                    case Operators.Sub:
                        result = logic.Sub(listNumbers);
                        ui.ShowResult(result, Operators.Sub, listNumbers);
                        break;
                    case Operators.Mul:
                        result = logic.Mul(listNumbers);
                        ui.ShowResult(result, Operators.Mul, listNumbers);
                        break;
                    case Operators.Div:
                        result = logic.Div(listNumbers);
                        ui.ShowResult(result, Operators.Div, listNumbers);
                        break;
                }
            }
        }
    }
}

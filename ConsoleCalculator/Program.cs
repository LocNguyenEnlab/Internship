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
            char operators = ui.GetOperator();
            var listNumbers = ui.GetListInputNumbers();

            if(listNumbers.Count < 2)
            {
                ui.Inform("Not enough numbers to calculate!");
            }
            else
            {
                switch (operators)
                {
                    case '+':
                        result = logic.Add(listNumbers);
                        ui.ShowResult(result, operators, listNumbers);
                        break;
                    case '-':
                        result = logic.Sub(listNumbers);
                        ui.ShowResult(result, operators, listNumbers);
                        break;
                    case '*':
                        result = logic.Mul(listNumbers);
                        ui.ShowResult(result, operators, listNumbers);
                        break;
                    case '/':
                        result = logic.Div(listNumbers);
                        ui.ShowResult(result, operators, listNumbers);
                        break;
                }
            }
        }
    }
}

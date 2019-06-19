using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    internal class CalculateService
    {
        public CalculateService()
        {
        }

        internal double Add(List<double> listNumbers)
        {
            var result = listNumbers[0];

            for (int i = 1; i < listNumbers.Count; i++) {
                result += listNumbers[i];
            }
            return result;
        }

        internal double Sub(List<double> listNumbers)
        {
            var result = listNumbers[0];

            for (int i = 1; i < listNumbers.Count; i++)
            {
                result -= listNumbers[i];
            }
            return result;
        }

        internal double Mul(List<double> listNumbers)
        {
            var result = listNumbers[0];

            for (int i = 1; i < listNumbers.Count; i++)
            {
                result *= listNumbers[i];
            }
            return result;
        }

        internal double Div(List<double> listNumbers)
        {
            var result = listNumbers[0];

            for (int i = 1; i < listNumbers.Count; i++)
            {
                result /= listNumbers[i];
            }
            return result;
        }
    }
}
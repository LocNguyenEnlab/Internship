using System;
using System.Text;
using System.Windows.Forms;

namespace WinformCalculator
{
    public class CalculateService
    {

        public void Calculate(StringBuilder operators, StringBuilder numbers, StringBuilder expressions, StringBuilder result)
        {
            if (numbers.Length == 0)
            {
                MessageBox.Show("Invalid expression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                expressions.Clear();
                result.Clear();
            }
            else
            {
                if (result.Length == 0)
                {
                    result.Append(numbers);
                }
                else
                {
                    double x = Convert.ToDouble(result.ToString());
                    double y = Convert.ToDouble(numbers.ToString());
                    if (operators.ToString() == "+")
                    {
                        result.Clear();
                        result.Append(Add(x, y));
                    }
                    else if (operators.ToString() == "-")
                    {
                        result.Clear();
                        result.Append(Sub(x, y));
                    }
                    else if (operators.ToString() == "*")
                    {
                        result.Clear();
                        result.Append(Mul(x, y));
                    }
                    else if (operators.ToString() == "/")
                    {
                        result.Clear();
                        result.Append(Div(x, y));
                    }
                }
                numbers.Clear();
                operators.Clear();
            }
        }

        public string Add(double x, double y)
        {
            return String.Format("{0:0.00}", (x + y));
        }

        public string Sub(double x, double y)
        {
            return String.Format("{0:0.00}", (x - y));
        }

        public string Mul(double x, double y)
        {
            return String.Format("{0:0.00}", (x * y));
        }

        public string Div(double x, double y)
        {
            return String.Format("{0:0.00}", (x / y));
        }
    }
}

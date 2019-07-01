using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        result.Append((x + y).ToString());
                    }
                    else if (operators.ToString() == "-")
                    {
                        result.Clear();
                        result.Append((x - y).ToString());
                    }
                    else if (operators.ToString() == "*")
                    {
                        result.Clear();
                        result.Append((x * y).ToString());
                    }
                    else if (operators.ToString() == "/")
                    {
                        result.Clear();
                        result.Append((x / y).ToString());
                    }

                }
                numbers.Clear();
                operators.Clear();
            }
        }
    }
}

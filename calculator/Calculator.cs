using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace calculator
{
    public struct StartEndIndex
    {
        public int startIndex;
        public int endIndex;
    }

    internal class Calculator
    {
        private const int maxRecursion = 4000;
        public readonly char[] ops;
        private int recurseionCount;

        public Calculator(char[] ops)
        {
            this.ops = ops;
        }


        private string SimpleOps(string expression)
        {
            bool isNegative = false;


            if (expression[0] =='-')
            {
                isNegative = true;
                expression = expression.Substring(1);
            }


            if (expression.Contains('!'))
            {
                expression = expression.Remove(expression.Length-1);
                double tmp = double.Parse(expression);

                if (isNegative)
                    throw new InvalidOperationException("Nincs Negatív faktoriális.");

                return MathHelper.Factorial(tmp);
            }


            int opIndex = expression.IndexOfAny(ops);


            double a = double.Parse(expression.Substring(0, opIndex));
            double b = double.Parse(expression.Substring(opIndex + 1));

            if (isNegative)
                a = -1 * a;


            double result;
            switch (expression[opIndex])
            {
                case '+': result = a + b; break;
                case '-': result = a - b; break;
                case '*': result = a * b; break;
                case '^': result = Math.Pow(a, b); break;
                case '/':
                    if (b == 0)
                        throw new InvalidOperationException("Nullával osztás!!");

                    result = a / b; 
                    break;

                default: throw new NotSupportedException("Művelet nem támogatott!");
            }

            return result.ToString();
        }

        

        private string CalculateV2(string expression)
        {
            if(CalculatorHelper.IsComplete(expression))
                return expression;

            if (maxRecursion < recurseionCount)
                throw new Exception("Too long or looping");

            recurseionCount++;

            if (expression.Contains("√"))         //Shit design
            {
                string oldExp = CalculatorHelper.NthRootOfXHelper(expression, out string toCalc, out double N);

                string calculated = MathHelper.NthRoot( 
                    double.Parse(CalculateV2(toCalc)) , N).ToString();

                
                expression.Replace(oldExp, calculated);
            }

            else if (expression.Contains('('))
            {
                string oldExp = CalculatorHelper.Brackets(expression);

                expression = expression.Replace(
                            oldExp,
                          CalculateV2(oldExp.Substring(1,oldExp.Length-2))
                    );
            }

            else if (CalculatorHelper.DoWeNeedSimplification(expression) ) // move root thing here 
            {

                CalculatorHelper.Simplify(expression, out string oldExp, out string toCalc);

                expression = expression.Replace(oldExp, CalculateV2(toCalc)); // bruh ezt is at kell írni
            }

            else
                expression = SimpleOps(expression);



            return CalculateV2(expression);
        }

        public string Calculate(string expession)
        {
            try
            {
                return CalculateV2(expession);
            }
            
            
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Hiba!");
                return "";
            }
            catch(NotSupportedException ex)
            {
                MessageBox.Show(ex.Message, "Hiba!");
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "sad ):");

                return "";
            }

        }
    }
}

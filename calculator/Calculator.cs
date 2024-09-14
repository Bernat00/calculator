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
                BigInteger tmp = BigInteger.Parse(expression);

                if (isNegative)
                    throw new InvalidOperationException("Nincs Negatív faktoriális.");

                return MathHelper.Factorial(tmp);
            }


            int opIndex = expression.IndexOfAny(ops);


            BigInteger a = BigInteger.Parse(expression.Substring(0, opIndex));
            BigInteger b = BigInteger.Parse(expression.Substring(opIndex + 1));

            if (isNegative)
                a = -1 * a;


            BigInteger result;
            switch (expression[opIndex])
            {
                case '+': result = a + b; break;
                case '-': result = a - b; break;
                case '*': result = a * b; break;
                case '^': result = BigInteger.Pow(a, (int)b); break;
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
            if (maxRecursion < (recurseionCount++))
                throw new Exception("Too long or looping");

            if (expression.Contains("sqrt"))
            {
                throw new NotImplementedException("még nincs kész ez a része");
            }

            else if (expression.Contains('('))
            {
                string oldExp = CalculatorHelper.Brackets(expression);

                expression = expression.Replace(
                            oldExp,
                          CalculateV2(oldExp.Substring(1,oldExp.Length-2))
                    );
            }

            else if (CalculatorHelper.DoWeNeedSimplification(expression) )
            {

                CalculatorHelper.Simplify(expression, out string oldExp, out string toCalc);

                expression = expression.Replace(oldExp, CalculateV2(toCalc));
            }

            else
                expression = SimpleOps(expression);



            if (!CalculatorHelper.IsComplete(expression))
                expression = CalculateV2(expression);


            return expression;

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

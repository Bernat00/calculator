using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculator
{
    public struct StartEndIndex
    {
        public int startIndex;
        public int endIndex;
    }

    internal class Calculator
    {
        public readonly char[] ops;

        public Calculator(char[] ops)
        {
            this.ops = ops;
        }

        private bool DoWeNeedSimplification(string expression)
        {
            int negativesIndex;


            if (expression[0] == '-')
            {
                expression = expression.Substring(1);

                return expression.IndexOfAny(ops) != expression.LastIndexOfAny(ops);
            }
            

            while ((expression.Contains('*') || expression.Contains('/') || expression.Contains('^'))
                && expression[
                    negativesIndex = expression.IndexOfAny(new char[] { '/', '*', '^' }) + 1
                    ] == '-')
                {
                    expression = expression.Remove(negativesIndex,1);
                }


            return expression.IndexOfAny(ops) != expression.LastIndexOfAny(ops);
        }

        private string Brackets(string expression)
        {
            int startindex = expression.IndexOf('(');
            int endIndex = 0;

            int numOfStartBrackets = 0;
            int numOfEndBrackets = 0;


            while (numOfStartBrackets > numOfEndBrackets || numOfStartBrackets == 0)
            {
                if(expression.Length-1 < endIndex)
                    throw new NotSupportedException("Nem Jók a zárójelek!");

                if (expression[endIndex] == '(')
                    numOfStartBrackets++;

                else if (expression[endIndex] == ')')
                    numOfEndBrackets++;

                endIndex++;
            }


            return expression.Substring(startindex, endIndex-startindex);         //nem a legszebb
        }

        private string DivMult(string expression)
        {
            char[] tmp = new char[] { '/', '*' };
            int opIndex = expression.IndexOfAny(tmp);

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }

        private string SubAdd(string expression)
        {
            int opIndex = expression.LastIndexOfAny(ops);

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }

        private string Pow(string expression)
        {
            int opIndex = expression.IndexOf('^');

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }



        private string SimpleOps(string expression)
        {
            double result;
            bool isNegative = false;


            if (expression[0] =='-')
            {
                isNegative = true;
                expression = expression.Substring(1);
            }



            int opIndex = expression.IndexOfAny(ops);

            double a = double.Parse(expression.Substring(0, opIndex));
            double b = double.Parse(expression.Substring(opIndex + 1));

            if (isNegative)
                a = -1 * a;

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

        private bool IsComplete(string expression)
        {
            if ( !ops.Any(x => expression.Contains(x) )  )
                return true;

            if (expression[0] != '-')
                return false;

            else
            {
                expression = expression.Substring(1);

                return !ops.Any(x => expression.Contains(x) );
            }

        }


        private StartEndIndex StartEndIndexOf(string expression, int indexOfOperation)
        {
            int start = indexOfOperation - 1;
            int end = indexOfOperation + 2;


            for (; start >= 0 && !ops.Contains(expression[start]); start--) ;

            start++;    // ez nem szép

            for (; end <= expression.Length - 1 && !ops.Contains(expression[end]); end++) ;

            end--;

            StartEndIndex tmp;
            tmp.startIndex = start;
            tmp.endIndex = end;
            return tmp;
        }


        private string CalculateV2(string expression)
        {
            if (expression.Contains('('))
            {
                string oldExp = Brackets(expression);

                expression = expression.Replace(
                            oldExp,
                          CalculateV2(oldExp.Substring(1,oldExp.Length-2))
                    );
            }

            else if (DoWeNeedSimplification(expression) )
            {
                string oldExp;
                string toCalc;

                if (expression.Contains('^'))
                    oldExp = toCalc = Pow(expression);

                else if (expression.Contains('*') || expression.Contains('/'))
                    toCalc = oldExp = DivMult(expression);

                else if (expression.Contains("+-"))
                {
                    oldExp = expression;
                    toCalc = expression.Replace("+-", "-");
                }
                else if (expression.Contains("--"))
                {
                    oldExp = expression;
                    toCalc = expression.Replace("--", "+");
                }


                else
                    toCalc = oldExp = SubAdd(expression);


                expression = expression.Replace(oldExp, CalculateV2(toCalc));
            }

            else
                expression = SimpleOps(expression);



            if (!IsComplete(expression))
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

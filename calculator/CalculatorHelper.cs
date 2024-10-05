using System;
using System.Linq;

namespace calculator
{
    static class CalculatorHelper
    {

        public static readonly char[] ops = { '+', '-', '*', '/', '^', '!', '√' };
        public static readonly char[] nums = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        public static readonly char[] special = { ',', '(', ')' };
        public static readonly char[] brackets = {'(', ')' };



        public static bool DoWeNeedSimplification(string expression)
        {
            int negativesIndex;

            if (expression[0] == '-')
            {
                expression = expression.Substring(1);

                return expression.IndexOfAny(ops) != expression.LastIndexOfAny(ops);
            }


            while ((expression.Contains('*') || expression.Contains('/') || expression.Contains('^'))       // ÁÁÁÁ black magic
                && expression[
                    negativesIndex = expression.IndexOfAny(new char[] { '/', '*', '^' }) + 1
                    ] == '-')
            {
                expression = expression.Remove(negativesIndex, 1);
            }


            return expression.IndexOfAny(ops) != expression.LastIndexOfAny(ops);
        }


        public static void Simplify(string expression, out string oldExp, out string toCalc)
        {

            if(expression.Contains('√'))
                oldExp = toCalc = expression = GetSingleOperation(expression, new char[] { '√' });

            else if (expression.Contains('^'))
                oldExp = toCalc = GetSingleOperation(expression, new char[] { '^' });

            else if (expression.Contains('*') || expression.Contains('/'))
                toCalc = oldExp = GetSingleOperation(expression, new char[] { '*', '/' });

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
                toCalc = oldExp = GetSingleOperation(expression, new char[] { '+', '-' });
        }


        public static string Brackets(string expression)
        {
            int startindex = expression.IndexOf('(');
            int endIndex = 0;

            int numOfStartBrackets = 0;
            int numOfEndBrackets = 0;


            while (numOfStartBrackets > numOfEndBrackets || numOfStartBrackets == 0)
            {
                if (expression.Length - 1 < endIndex)
                    throw new NotSupportedException("Nem Jók a zárójelek!");

                if (expression[endIndex] == '(')
                    numOfStartBrackets++;

                else if (expression[endIndex] == ')')
                    numOfEndBrackets++;

                endIndex++;
            }


            return expression.Substring(startindex, endIndex - startindex);         //nem a legszebb
        }


        public static bool IsComplete(string expression)
        {
            if (!ops.Any(x => expression.Contains(x)))
                return true;

            if (expression[0] != '-')
                return false;

            else
            {
                expression = expression.Substring(1);

                return !ops.Any(x => expression.Contains(x));
            }

        }


        public static string GetSingleOperation(string expression, char[] OpTypes)
        {
            int opIndex = expression.IndexOfAny(OpTypes);

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }


        private static StartEndIndex StartEndIndexOf(string expression, int indexOfOperation)
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

    }
}

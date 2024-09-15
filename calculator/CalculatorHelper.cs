using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator
{
    static class CalculatorHelper
    {

        public static readonly char[] ops = { '+', '-', '*', '/', '^', '!', '√' };
        public static readonly char[] nums = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        public static readonly char[] special = { ',', '(', ')' };


        public static bool DoWeNeedSimplification(string expression)
        {
            int negativesIndex;
            // todo put Root here

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


        #region operations
        public static string Pow(string expression)
        {
            int opIndex = expression.IndexOf('^');

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }

        /// <summary>
        /// Returns the string that has to be replaced with the calculate value
        /// and gives back N.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static string NthRootOfXHelper(string expression, out string toCalc, out double N)
        {
            int rootIndex = expression.IndexOf('√');
            int startIndex = rootIndex;

            string NString = "";


            do
            {
                startIndex--;
                NString = NString.Insert(0, expression[startIndex].ToString());
            }
            while (startIndex > 0 &&
                !ops.Contains(expression[startIndex - 1]) );


            string oldExp = toCalc = Brackets(expression.Substring(startIndex));

            N = double.Parse(NString);

            return oldExp.Insert(0, NString + "√" );
        }


        public static string DivMult(string expression)
        {
            char[] tmp = new char[] { '/', '*' };
            int opIndex = expression.IndexOfAny(tmp);

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }

        public static string SubAdd(string expression)
        {
            int opIndex = expression.LastIndexOfAny(ops);

            StartEndIndex indexes = StartEndIndexOf(expression, opIndex);

            string result = expression.Substring(indexes.startIndex,
                indexes.endIndex - indexes.startIndex + 1);

            return result;
        }

        #endregion


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

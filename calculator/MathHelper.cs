using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace calculator
{
    static class MathHelper
    {
        public static string Factorial(double number)
        {
            double result = number;

            for (number--; number > 0; number--)
                result *= number;

            return result.ToString();
        }


        public static double NthRoot(double A, double N)
        {
            bool isNegative = A < 0;

            if (N % 2 != 0)
                A = Math.Abs(A);

            double result = Math.Pow(A, 1.0 / N);

            if (double.IsNaN(result))
                throw new NotSupportedException("Gyökhiba (valószínüleg páros gyök alatt negatív szám)");


            return isNegative? result*=-1:result;
        }
    }
}
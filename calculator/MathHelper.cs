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
        public static string Factorial(BigInteger number)
        {
            BigInteger result = number;

            for (number--; number > 0; number--)
                result *= number;

            return result.ToString();
        }
    }
}

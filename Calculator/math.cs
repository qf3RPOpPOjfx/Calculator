using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Math
    {
        #region elementary arithmetic

        public static decimal Addition(decimal x, decimal y)
        {
            return x + y;
        }

        public static decimal Subtraction(decimal x, decimal y)
        {
            return x - y;
        }

        public static decimal Multiplication(decimal x, decimal y)
        {
            return x * y;
        }

        public static decimal Division(decimal x, decimal y)
        {
            if (y == 0)
                throw new DivideByZeroException();
            else
                return x / y;
        }

        #endregion

        #region number editing

        public static decimal AddDigit(decimal d, decimal add, bool AddWithDecimalMark)
        {
            if (d % 1 == 0)
            //number contains no decimal
            {
                if (d > 0)
                    return (d * 10M) + d;
                else
                    return (d * 10M) - d;
            }
            else
            // number contains decimal
            {
                if (AddWithDecimalMark) throw new Exception("number already contains decimalmark");

                if (d > 0)
                    return d + (d / (10 * System.Convert.ToDecimal(System.Math.Pow(10, GetDecimalCount(d)))));
                else
                    return (d - (d / (10 * System.Convert.ToDecimal(System.Math.Pow(10, GetDecimalCount(d))))));
            }
        }

        private static int GetDecimalCount(decimal d)
        {

            int decimalCount = 0;
            if (d > 0)
            {
                while (d != System.Math.Floor(d))
                {
                    d = (d - System.Math.Floor(d)) * 10;
                    decimalCount++;
                }
            }
            else
                while (d != System.Math.Ceiling(d))
                {
                    d = (d - System.Math.Ceiling(d)) * 10;
                    decimalCount++;
                }

            return decimalCount;
        }

        #endregion
    }
}

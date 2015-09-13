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

        public static decimal AddDigit(decimal number, decimal digit, bool AddWithDecimalMark = false, int trailingDecimalZeros = 0)
        {

            int decimalCount = GetDecimalCount(number);
            decimal factor = System.Convert.ToDecimal(System.Math.Pow(10, 1 + decimalCount + trailingDecimalZeros));
            int sign = System.Math.Sign(number);
            decimal decimalToAdd = sign * (digit / factor);

            if (decimalCount == 0 && !AddWithDecimalMark)
            // number is an integer

                if (trailingDecimalZeros == 0)
                    return (number * 10m) + (sign * digit);
                else
                    throw new Exception("Invalid use of parameter trailingDecimalZeros: Number contains no decimal.");

            else
            // number contains decimal || add digit as decimal to integer

                if (!AddWithDecimalMark || decimalCount == 0)
                    if (digit != 0m)
                        return number + decimalToAdd;
                    else
                    // adding zero after the decimalmark returns the same result
                        return number;
                else
                    throw new Exception("Invalid use of parameter AddWithDecimalMark: number already contains decimalmark.");

        }

        public static decimal RemoveDigit(decimal d)
        {

            if (d % 1 == 0)

                if (d >= 0)
                    return System.Math.Floor(d / 10);
                else
                    return System.Math.Ceiling(d / 10);
            else
            {
                int decimalCount = GetDecimalCount(d);
                decimal factor = Convert.ToDecimal(System.Math.Pow(10, decimalCount - 1));

                if (d > 0)
                    return System.Math.Floor(d * factor) / factor;
                else
                    return System.Math.Ceiling(d * factor) / factor;
            }
        }

        private static int GetDecimalCount(decimal d)
        {
            if (d % 1 == 0)
                return 0;
            else
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
        }

        #endregion

        #region simple math

        public static decimal AdditiveInverse(decimal d)
        {
            return d * -1;
        }

        public static decimal OneDivideByX(decimal d)
        {
            return Division(1m, d);
        }

        public static decimal Percentage(decimal d, decimal percentage)
        {
            return d * percentage / 100;
        }

        public static decimal SquareRoot(decimal d)
        { 
            if (d >= 0)
            {
                d = Convert.ToDecimal(System.Math.Sqrt(Convert.ToDouble(d)));
                return d;
            }
            else
                throw new Exception("cannot calculate square root of negative number");
        }

        #endregion

    }
}

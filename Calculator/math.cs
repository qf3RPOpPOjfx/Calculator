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
                if (AddWithDecimalMark)
                {
                    if (d >= 0)
                        return d + (d / 10);
                    else
                        return d - (d / 10);
                }
                else
                {
                    if (d > 0)
                        return (d * 10M) + d;
                    else
                        return (d * 10M) - d;
                }
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

        public static decimal RemoveDigit(decimal d)
        {

            if (d % 1 == 0)

                if (d >= 0)
                    return System.Math.Floor(d / 10);
                else
                    return System.Math.Ceiling(d / 10);
            else
            {
                decimal tmp = (d * Convert.ToDecimal(System.Math.Pow(10, GetDecimalCount(d) - 1))) / (Convert.ToDecimal(System.Math.Pow(10, GetDecimalCount(d) - 1)));

                if (d > 0)
                    return System.Math.Floor(tmp);
                else
                    return System.Math.Ceiling(tmp);
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

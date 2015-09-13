using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class calc
    {

        private enum DisplayStatus { clear, number, operatorX, decimalMark, result, error };
        private DisplayStatus ds = DisplayStatus.clear;

        private List<object> FormulaElements = new List<object>();

        private int trailingDecimalZeros;

        //=======================================================================================
        #region error handling

        private const string ErrorCannotDivideByZero = "Cannot divide by zero.";

        private void ExceptionHandling(Exception e)
        {
            Clear();
            ds = DisplayStatus.error;
            throw e;
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Number input

        public string AddDigit(decimal d)
        {
            string s = "";
            decimal number = 0m;
            decimal result = 0m;

            //get last number if available
            if (FormulaElements.OfType<decimal>().Any())
            {
                number = Convert.ToDecimal(FormulaElements.Last());
            }

            switch (ds)
            {
                case DisplayStatus.number:
                case DisplayStatus.decimalMark:
                    
                    if (d == 0 && ((number % 1 != 0) || ds == DisplayStatus.decimalMark))
                        trailingDecimalZeros++;

                    bool AddWithDecimalMark = (ds == DisplayStatus.decimalMark);
                    result = Math.AddDigit(number, d, AddWithDecimalMark, trailingDecimalZeros);

                    FormulaElements.Remove(FormulaElements.Last());
                    FormulaElements.Add(result);
                    s = FormulaElements.Last().ToString();

                    if (d != 0)
                        trailingDecimalZeros = 0;

                    // if digit to add is 0...
                    else
                    {
                        //.. if number contains decimal add "0" for each trailing decimal zero
                        if ((number % 1 != 0))
                            
                            for (int i=0 ; i < trailingDecimalZeros; i++)
                                s = s + "0";

                        //... if last input is decimalmark add ",0" 
                        if ((ds == DisplayStatus.decimalMark))
                        { 
                            s = s + DecimalMark + "0";
                        }
                    }

                    break;

                // in all other cases add digit as new number
                case DisplayStatus.clear:
                case DisplayStatus.operatorX:
                case DisplayStatus.error:
                case DisplayStatus.result:

                    FormulaElements.Add(d);
                    s = FormulaElements.Last().ToString();
                    break;
            }

            ds = DisplayStatus.number;
            
            return s;
        }

        private int GetDecimalCount(decimal d)
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

        private const string DecimalMark = ",";

        public string AddDecimalMark()
        {
            decimal d;
            string s = "";

            switch (ds)
            {
                case DisplayStatus.clear:
                case DisplayStatus.error:
                    d = 0m;
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.number:
                    d = Convert.ToDecimal(FormulaElements.Last());
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.operatorX:
                    d = 0m;
                    FormulaElements.Add(d);
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.decimalMark:
                    d = Convert.ToDecimal(FormulaElements.Last());
                    s = d.ToString();
                    break;
                case DisplayStatus.result:
                    d = 0m;
                    FormulaElements.Add(d);
                    s = d.ToString() + DecimalMark;
                    break;
            }

            ds = DisplayStatus.decimalMark;

            return s;
        }

        public string PlusMinus()
        {
            try
            { 
                switch (ds)
                {
                    case DisplayStatus.number:
                    case DisplayStatus.decimalMark:
                    case DisplayStatus.result:

                        decimal d;
                        d = Convert.ToDecimal(FormulaElements.Last());
                        FormulaElements.Remove(FormulaElements.Last());
                        FormulaElements.Add(Math.AdditiveInverse(d));

                        if (ds == DisplayStatus.decimalMark)
                            return FormulaElements.Last().ToString() + DecimalMark;
                        else
                            return FormulaElements.Last().ToString();

                    case DisplayStatus.clear:
                    case DisplayStatus.operatorX:
                    case DisplayStatus.error:
                        return null;
                    
                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Operator input

        public enum Operators { plus, minus, multiply, divide, equals, percentage, squareRoot };

        private static string _Equals = "=";
        private static string _Plus = "+";
        private static string _Minus = "-";
        private static string _Multiply = "x";
        private static string _Divide = "/";
        private static string _Percentage = "%";
        private static string _SquareRoot = ((char)0x221A).ToString();

        public string Operator(Operators o)
        {

            switch (ds)
            {
                case DisplayStatus.clear:
                case DisplayStatus.error:
                    FormulaElements.Add(0m);
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.number:
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.operatorX:
                    FormulaElements.Remove(FormulaElements.Last());
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.decimalMark:
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.result:
                    FormulaElements.Add(o);
                    break;
            }

            ds = DisplayStatus.operatorX;

            return OperatorToString(o);
        }

        private string OperatorToString(Operators o)
        {
            try
            { 
                switch (o)
                {
                    case Operators.plus:
                        return _Plus;
                    case Operators.minus:
                        return _Minus;
                    case Operators.multiply:
                        return _Multiply;
                    case Operators.divide:
                        return _Divide;
                    case Operators.equals:
                        return _Equals;
                    case Operators.percentage:
                        return _Percentage;
                    case Operators.squareRoot:
                        return _SquareRoot;
                    default:
                        throw new NotImplementedException
                            ("Unrecognized Operator value.");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }

        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Misc math functions

        public string OneDivideByX()
        {
            decimal d = Convert.ToDecimal(FormulaElements.Last());

            if (d == 0)
            { 
                ds = DisplayStatus.error;
                return ErrorCannotDivideByZero;
            }

            switch (ds)
            {
                case DisplayStatus.decimalMark:
                case DisplayStatus.number:
                case DisplayStatus.result:
                case DisplayStatus.clear:
                    return Math.OneDivideByX(Convert.ToDecimal(FormulaElements.Last())).ToString();

                case DisplayStatus.operatorX:
                case DisplayStatus.error:
                    return null;

                default:
                    throw new NotImplementedException("Unrecognized display status");
            }

        }

        public string Percentage()
        {
            try
            { 
                switch (ds)
                {
                    case DisplayStatus.number:
                    case DisplayStatus.decimalMark:

                        decimal d = Convert.ToDecimal(FormulaElements.Last());
                        int i = FormulaElements.OfType<decimal>().Count();

                        if (i < 2) 
                            return Clear();
                        else
                        {
                            decimal d2 = Convert.ToDecimal(FormulaElements[FormulaElements.Count() - 3]);
                            d = Math.Percentage(d2, d);
                            FormulaElements.Remove(FormulaElements.Last());
                            FormulaElements.Add(d);
                            return d.ToString();
                        }

                    case DisplayStatus.clear:
                    case DisplayStatus.error:
                    case DisplayStatus.operatorX:
                    case DisplayStatus.result:
                        return null;

                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }
        }

        public string SquareRoot()
        {
            try
            {
                switch (ds)
                {
                    case DisplayStatus.number:
                    case DisplayStatus.decimalMark:
                    case DisplayStatus.result:

                        decimal d = Convert.ToDecimal(FormulaElements.Last());
                        FormulaElements.Remove(FormulaElements.Last());
                        FormulaElements.Add(Math.SquareRoot(d));
                        return d.ToString();

                    case DisplayStatus.clear:
                    case DisplayStatus.error:
                    case DisplayStatus.operatorX:
                        return null;

                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Calculate functions

        private T GetOperatorEnum<T>(object o)
        {
            T enumVal = (T)Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }

        public string GetFormula()
        {
            string s = "";

            foreach (var o in FormulaElements)
            {
                if (o.GetType() == typeof(decimal))
                    s = s + " " + o.ToString();
                else
                    s = s + " " + OperatorToString(GetOperatorEnum<Operators>(o));
            }
            return s;

        }

        public string Calculate()
        {
            decimal result = 0M;

            try
            {

                switch (ds)
                {
                    case DisplayStatus.number:

                        result = Convert.ToDecimal(FormulaElements[0]);

                        for (int i = 1; i < FormulaElements.Count(); i = i + 2)
                        {
                            decimal number = Convert.ToDecimal(FormulaElements[i + 1]);

                            switch ((Operators)FormulaElements[i])
                            {
                                case Operators.plus:
                                    result = result + number;
                                    break;
                                case Operators.minus:
                                    result = result - number;
                                    break;
                                case Operators.multiply:
                                    result = result * number;
                                    break;
                                case Operators.divide:
                                    if (number == 0)
                                        throw new DivideByZeroException();
                                    else
                                    { 
                                        result = result / number;
                                        break;
                                    }

                                default:
                                    throw new NotImplementedException("Unrecognized Operator value.");
                            }
                        }

                        FormulaElements.Clear();
                        FormulaElements.Add(result);
                        ds = DisplayStatus.result;
                        return result.ToString();

                    case DisplayStatus.operatorX:
                        return OperatorToString((Operators)FormulaElements.Last());

                    case DisplayStatus.decimalMark:
                        return FormulaElements.Last().ToString();

                    case DisplayStatus.result:
                        return FormulaElements.Last().ToString();

                    case DisplayStatus.clear:
                    case DisplayStatus.error:
                    default:
                        throw new NotImplementedException("Unrecognized DisplayStatus value.");
                }
            }
            catch (DivideByZeroException e)
            {
                ExceptionHandling(e);
                return null;
            }
            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }

        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Clear and backspace functions

        public string Clear()
        {
            FormulaElements.Clear();
            ds = DisplayStatus.clear;
            return "0";
        }

        public string ClearEntry()
        {
            try
            { 
                switch (ds)
                {
                    case DisplayStatus.number:
                    case DisplayStatus.decimalMark:
                        if (FormulaElements.OfType<decimal>().Count() > 1)
                            ds = DisplayStatus.operatorX;
                        else
                            ds = DisplayStatus.clear;

                        FormulaElements.Remove(FormulaElements.Last());
                        return "0";

                    case DisplayStatus.operatorX:
                        FormulaElements.Remove(FormulaElements.Last());
                        ds = DisplayStatus.number;
                        return FormulaElements.Last().ToString();

                    case DisplayStatus.clear:
                    case DisplayStatus.result:
                    case DisplayStatus.error:
                        return 0.ToString();

                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }
        }

        public string BackSpace()
        {

            decimal d;
            if (FormulaElements.OfType<decimal>().Any())
                d = Convert.ToDecimal(FormulaElements.Last());
            else
                d = 0;

            try
            {
                switch (ds)
                {
                    case DisplayStatus.number:
                    case DisplayStatus.result:

                        d = Math.RemoveDigit(d);
                        FormulaElements.Remove(FormulaElements.Last());
                        FormulaElements.Add(d);
                        return d.ToString();

                    case DisplayStatus.operatorX:
                        FormulaElements.Remove(FormulaElements.Last());
                        ds = DisplayStatus.number;
                        return d.ToString();

                    case DisplayStatus.decimalMark:
                        return d.ToString();

                    case DisplayStatus.clear:
                    case DisplayStatus.error:
                        return null;

                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Memory functions

        private decimal mem;

        public bool MemoryClear()
        {
            mem = 0;
            return false;
        }

        public enum memorySaveMode { save, plus, minus }
        public bool MemorySave(memorySaveMode mf)
        {
            try
            { 
                switch (ds)
                {
                    case DisplayStatus.number:
                    case DisplayStatus.decimalMark:
                    case DisplayStatus.result:

                        if (Convert.ToDecimal(FormulaElements.Last()) == 0m)
                        {
                            mem = 0;
                            return false;
                        }
                        else
                        {
                            switch (mf)
                            {
                                case memorySaveMode.save:
                                    mem = Convert.ToDecimal(FormulaElements.Last());
                                    return true;
                                case memorySaveMode.plus:
                                    mem = mem + Convert.ToDecimal(FormulaElements.Last());
                                    return true;
                                case memorySaveMode.minus:
                                    mem = mem - Convert.ToDecimal(FormulaElements.Last());
                                    return true;
                                default:
                                    throw new NotImplementedException("Unrecognized memory mode");
                            }
                        }

                    case DisplayStatus.clear:
                    case DisplayStatus.operatorX:
                    case DisplayStatus.error:
                        return false;

                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return false;
            }
        }

        public string MemoryRead()
        {
            try
            { 
                decimal d = mem;

                switch(ds)
                {
                    case DisplayStatus.clear:
                    case DisplayStatus.result:
                    case DisplayStatus.decimalMark:
                    case DisplayStatus.number:
                    case DisplayStatus.error:
                        if (FormulaElements.OfType<decimal>().Any()) FormulaElements.Remove(FormulaElements.Last());
                        FormulaElements.Add(d);
                        ds = DisplayStatus.number;
                        return d.ToString();

                    case DisplayStatus.operatorX:
                        FormulaElements.Add(d);
                        ds = DisplayStatus.number;
                        return d.ToString();

                    default:
                        throw new NotImplementedException("Unrecognized display status");
                }
            }

            catch (NotImplementedException e)
            {
                ExceptionHandling(e);
                return null;
            }

        }

        //---------------------------------------------------------------------------------------
        #endregion

    }
}

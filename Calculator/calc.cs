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

        //=======================================================================================
        #region error handling

        private const string ErrorCannotDivideByZero = "Cannot divide by zero.";

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Number input
        
        public string AddDigit(decimal d)
        {

            string s="";
            decimal d1 = 0M;

            if (FormulaElements.OfType<decimal>().Any()) d1 = Convert.ToDecimal(FormulaElements.Last());

            switch (ds)
            {
                case DisplayStatus.clear:
                case DisplayStatus.error:
                    FormulaElements.Add(d);
                    s = FormulaElements.Last().ToString();
                    break;

                case DisplayStatus.number:
                    FormulaElements.Remove(FormulaElements.Last());
                    if (d1 % 1==0)
                    {
                        if (d1 > 0)
                            FormulaElements.Add((d1 * 10M) + d);
                        else
                            FormulaElements.Add((d1 * 10M) - d);

                        s = FormulaElements.Last().ToString();
                    }
                    else
                    {
                        if (d1 > 0)
                            FormulaElements.Add(d1 + (d / (10 * System.Convert.ToDecimal(Math.Pow(10, GetDecimalCount(d1))))));
                        else
                            
                            FormulaElements.Add(d1 - (d / (10 * System.Convert.ToDecimal(Math.Pow(10, GetDecimalCount(d1))))));

                        if (d == 0)
                            s = FormulaElements.Last().ToString() + "0";
                        else
                            s = FormulaElements.Last().ToString();
                    }
                    break;

                case DisplayStatus.operatorX:
                    FormulaElements.Add(d);
                    s = FormulaElements.Last().ToString();
                    break;

                case DisplayStatus.decimalMark:
                    FormulaElements.Remove(FormulaElements.Last());
                    if (d1 >= 0)
                        FormulaElements.Add(d1 + (d / 10));
                    else
                        FormulaElements.Add(d1 - (d / 10));

                    s = FormulaElements.Last().ToString();
                    break;

                case DisplayStatus.result:
                    FormulaElements.Clear();
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
                while (d != Math.Floor(d))
                {
                    d = (d - Math.Floor(d)) * 10;
                    decimalCount++;
                }
            }
            else
                while (d != Math.Ceiling(d))
                {
                    d = (d - Math.Ceiling(d)) * 10;
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
                    d = 0M;
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.number:
                    d = Convert.ToDecimal(FormulaElements.Last());
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.operatorX:
                    d = 0M;
                    FormulaElements.Add(d);
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.decimalMark:
                    d = Convert.ToDecimal(FormulaElements.Last());
                    s = d.ToString();
                    break;
                case DisplayStatus.result:
                    d = 0M;
                    FormulaElements.Add(d);
                    s = d.ToString() + DecimalMark;
                    break;
            }

            ds = DisplayStatus.decimalMark;

            return s;
        }

        public string PlusMinus()
        {

            switch (ds)
            {
                case DisplayStatus.number:
                case DisplayStatus.decimalMark:
                case DisplayStatus.result:
                    decimal d;
                    d = Convert.ToDecimal(FormulaElements.Last());
                    FormulaElements.Remove(FormulaElements.Last());

                    if (d >= 0)
                        FormulaElements.Add(-d);
                    else
                        FormulaElements.Add(Math.Abs(d));
                    if (ds == DisplayStatus.decimalMark)
                        return FormulaElements.Last().ToString() + DecimalMark;
                    else
                        return FormulaElements.Last().ToString();

                case DisplayStatus.clear:
                case DisplayStatus.operatorX:
                case DisplayStatus.error:
                default:
                    return "";
            }
        }

        public string BackSpace()
        {
            decimal d = Convert.ToDecimal(FormulaElements.Last());

            switch (ds)
            {
                case DisplayStatus.number:
                case DisplayStatus.result:

                    if (GetDecimalCount(d) == 0)
                        if (d >= 0)
                            d = Math.Floor(Convert.ToDecimal(FormulaElements.Last()) / 10);
                        else
                            d = Math.Ceiling(Convert.ToDecimal(FormulaElements.Last()) / 10);
                    else
                    { 
                        if (d > 0)
                            d = Math.Floor(d * Convert.ToDecimal(Math.Pow(10, GetDecimalCount(d) - 1))) / (Convert.ToDecimal(Math.Pow(10, GetDecimalCount(d) - 1)));
                        else
                            d = Math.Ceiling(d * Convert.ToDecimal(Math.Pow(10, GetDecimalCount(d) - 1))) / (Convert.ToDecimal(Math.Pow(10, GetDecimalCount(d) - 1)));
                            
                    }

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
                default:
                    return "0";
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
                    FormulaElements.Add(0M);
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
                return e.Message;
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
                case DisplayStatus.clear:
                    ds = DisplayStatus.error;
                    return ErrorCannotDivideByZero;

                case DisplayStatus.operatorX:
                    return OperatorToString((Operators)FormulaElements.Last());

                case DisplayStatus.error:
                    return "";

                case DisplayStatus.decimalMark:
                case DisplayStatus.number:
                case DisplayStatus.result:
                    return (1 / Convert.ToDecimal(FormulaElements.Last())).ToString();

            }

            return "";
        }

        public string Percentage()
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
                        d = d2 * d / 100;
                        FormulaElements.Remove(FormulaElements.Last());
                        FormulaElements.Add(d);
                        return d.ToString();
                    }

                case DisplayStatus.clear:
                case DisplayStatus.error:
                case DisplayStatus.operatorX:
                case DisplayStatus.result:
                default:
                    return Clear();
            }
            
        }

        public string SquareRoot()
        {
            try
            {
                switch (ds)
                {
                    case DisplayStatus.clear:
                    case DisplayStatus.error:
                    case DisplayStatus.operatorX:
                    default:
                        return Clear();

                    case DisplayStatus.number:
                    case DisplayStatus.decimalMark:
                    case DisplayStatus.result:
                        decimal d = Convert.ToDecimal(FormulaElements.Last());
                        if (d >= 0)
                        {
                            d = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(d)));
                            FormulaElements.Remove(FormulaElements.Last());
                            FormulaElements.Add(d);
                            return d.ToString();
                        }
                        else
                            throw new Exception("cannot calculate square root of negative number");

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("x");
                throw e;
                //Clear();
                //ds = DisplayStatus.error;
                //return e.Message;
            }
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Calculate functions

        private string GetFormulaSymbol(Operators op)
        {
            switch (op)
            {
                case Operators.equals:
                    return _Equals;
                case Operators.plus:
                    return _Plus;
                case Operators.minus:
                    return _Minus;
                case Operators.multiply:
                    return _Multiply;
                case Operators.divide:
                    return _Divide;
                case Operators.percentage:
                    return _Percentage;
                case Operators.squareRoot:
                    return _SquareRoot;
                default:
                    return "";
            }
        }

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
                    s = s + " " + GetFormulaSymbol(GetOperatorEnum<Operators>(o));
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
                                    result = result / number;
                                    break;

                                default:
                                    result = 0;
                                    break;
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
                        return "0";
                }
            }
            catch (System.DivideByZeroException)
            {
                Clear();
                ds = DisplayStatus.error;
                return "Cannot divide by zero";
            }
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Clear functions

        public string Clear()
        {
            FormulaElements.Clear();
            ds = DisplayStatus.clear;
            return "0";
        }

        public string ClearEntry()
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
                default:
                    ds = DisplayStatus.clear;
                    return "0";
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
                                return true;
                        }
                    }

                case DisplayStatus.clear:
                case DisplayStatus.operatorX:
                case DisplayStatus.error:
                default:
                    mem = 0;
                    return false;
            }
        }

        public string MemoryRead()
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
                    return "error";
            }
            
        }

        //---------------------------------------------------------------------------------------
        #endregion

    }
}

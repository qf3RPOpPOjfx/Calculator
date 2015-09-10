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

        private List<decimal> num = new List<decimal>();
        
        public string AddDigit(decimal d)
        {

            string s="";
            decimal d1 = 0M;

            if (num.Any()) d1 = num.Last();

            switch (ds)
            {
                case DisplayStatus.clear:
                case DisplayStatus.error:
                    num.Add(d);
                    FormulaElements.Add(d);
                    s = num.Last().ToString();
                    break;

                case DisplayStatus.number:
                    num.Remove(num.Last());
                    FormulaElements.Remove(num.Last());
                    if (d1 % 1==0)
                    {
                        if (d1 > 0)
                        {
                            num.Add((d1 * 10M) + d);
                            FormulaElements.Add((d1 * 10M) + d);
                        }
                        else
                        { 
                            num.Add((d1 * 10M) - d);
                            FormulaElements.Add((d1 * 10M) - d);
                        }
                        DecimalCount = 0;
                        s = num.Last().ToString();
                    }
                    else
                    {
                        if (d1 > 0)
                        {
                            num.Add(d1 + (d / (10 * System.Convert.ToDecimal(Math.Pow(10, DecimalCount)))));
                            FormulaElements.Add(d1 + (d / (10 * System.Convert.ToDecimal(Math.Pow(10, DecimalCount)))));
                        }
                        else
                        { 
                            num.Add(d1 - (d / (10 * System.Convert.ToDecimal(Math.Pow(10, DecimalCount)))));
                            FormulaElements.Add(d1 + (d / (10 * System.Convert.ToDecimal(Math.Pow(10, DecimalCount)))));
                        }
                        DecimalCount++;
                        if (d == 0)
                            s = num.Last().ToString() + "0";
                        else
                            s = num.Last().ToString();
                    }
                    break;

                case DisplayStatus.operatorX:
                    num.Add(d);
                    FormulaElements.Add(d);
                    s = num.Last().ToString();
                    break;

                case DisplayStatus.decimalMark:
                    num.Remove(num.Last());
                    num.Add(d1 + (d / 10));
                    FormulaElements.Remove(num.Last());
                    FormulaElements.Add(d);
                    DecimalCount++;
                    s = num.Last().ToString();
                    break;

                case DisplayStatus.result:
                    num.Clear();
                    num.Add(d);
                    FormulaElements.Clear();
                    FormulaElements.Add(d);
                    s = num.Last().ToString();
                    break;
            }

            ds = DisplayStatus.number;

            return s;
        }

        private int DecimalCount;

        private int GetDecimalCount(decimal d)
        {

            int decimalCount = 0;
            while (d != Math.Floor(d))
            {
                d = (d - Math.Floor(d)) * 10;
                decimalCount++;
            }
            return decimalCount;
        }

        private const string DecimalMark = ",";

        public string AddDecimalMark()
        {
            decimal d;
            string s = "";
            DecimalCount = 0;

            switch (ds)
            {
                case DisplayStatus.clear:
                case DisplayStatus.error:
                    d = 0M;
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.number:
                    d = num.Last();
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.operatorX:
                    d = 0M;
                    num.Add(d);
                    s = d.ToString() + DecimalMark;
                    break;
                case DisplayStatus.decimalMark:
                    d = num.Last();
                    s = d.ToString();
                    break;
                case DisplayStatus.result:
                    d = 0M;
                    num.Add(d);
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
                    d = num.Last();
                    num.Remove(num.Last());
                    FormulaElements.Remove(num.Last());

                    if (d >= 0)
                    {
                        num.Add(-d);
                        FormulaElements.Add(-d);
                    }
                    else
                    { 
                        num.Add(Math.Abs(d));
                        FormulaElements.Add(Math.Abs(d));
                    }
                    if (ds == DisplayStatus.decimalMark)
                        return num.Last().ToString() + DecimalMark;
                    else
                        return num.Last().ToString();

                case DisplayStatus.clear:
                case DisplayStatus.operatorX:
                case DisplayStatus.error:
                default:
                    return "";
            }
        }

        public string BackSpace()
        {
            decimal d = num.Last();

            switch (ds)
            {
                case DisplayStatus.number:

                    if (DecimalCount == 0)
                        if (d >= 0)
                            d = Math.Floor(num.Last() / 10);
                        else
                            d = Math.Ceiling(num.Last() / 10);
                    else
                    {
                        if (d > 0)
                            d = d - (d - (Math.Floor(d * Convert.ToDecimal(Math.Pow(10, 4 - 1))) / (Convert.ToDecimal(Math.Pow(10, 4 - 1)))));
                        else
                            d = d - (d - (Math.Floor(d * Convert.ToDecimal(Math.Pow(-10, 4 - 1))) / (Convert.ToDecimal(Math.Pow(-10, 4 - 1)))));
                    }

                    num.Remove(num.Last());
                    num.Add(d);
                    FormulaElements.Remove(num.Last());
                    FormulaElements.Add(d);
                    return d.ToString();

                case DisplayStatus.operatorX:
                    op.Remove(op.Last());
                    FormulaElements.Remove(num.Last());
                    ds = DisplayStatus.number;
                    return d.ToString();

                case DisplayStatus.result:
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

        private List<Operators> op = new List<Operators>();

        public string Operator(Operators o)
        {

            switch (ds)
            {
                case DisplayStatus.clear:
                case DisplayStatus.error:
                    num.Add(0M);
                    op.Add(o);
                    FormulaElements.Add(0M);
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.number:
                    op.Add(o);
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.operatorX:
                    op.Remove(op.Last());
                    op.Add(o);
                    FormulaElements.Remove(op.Last());
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.decimalMark:
                    DecimalCount = 0;
                    op.Add(o);
                    FormulaElements.Add(o);
                    break;
                case DisplayStatus.result:
                    op.Add(o);
                    FormulaElements.Add(o);
                    break;
            }

            ds = DisplayStatus.operatorX;

            return OperatorToString(o);
        }

        private string OperatorToString(Operators o)
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

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Misc math functions

        public string OneDivideByX()
        {
            decimal d = num.Last();

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
                    return OperatorToString(op.Last());

                case DisplayStatus.error:
                    return "";

                case DisplayStatus.decimalMark:
                case DisplayStatus.number:
                case DisplayStatus.result:
                    return (1 / num.Last()).ToString();

            }

            return "";
        }

        public string Percentage()
        {
            switch (ds)
            {
                case DisplayStatus.number:
                case DisplayStatus.decimalMark:
                    decimal d = num.Last();
                    int i = num.Count();

                    if (i < 2) 
                        return Clear();
                    else
                    {
                        d = num[i - 2] * (d / 100);
                        num.Remove(num.Last());
                        num.Add(d);
                        FormulaElements.Remove(num.Last());
                        FormulaElements.Add(d);
                        return Calculate();
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
                    decimal d = num.Last();
                    d = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(d)));
                    num.Remove(num.Last());
                    num.Add(d);
                    FormulaElements.Remove(num.Last());
                    FormulaElements.Add(d);
                    return d.ToString();

            }
        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Calculate functions

        public string GetFormula()
        {
            string s = "";
            int l = op.Count();

            switch (ds)
            {

                case DisplayStatus.number:
                case DisplayStatus.operatorX:
                case DisplayStatus.decimalMark:
                    if (num.Any())
                    {
                        for (int i = 0; i < num.Count(); i++)
                        {

                            if (s == "")
                                s = s + " " + num[i].ToString();
                            else
                                s = s + " " + num[i].ToString();

                            if (i < l)
                            {
                                s = s + " " + OperatorToString(op[i]);
                            }
                        }
                        return s;
                    }
                    else
                        return "";

                case DisplayStatus.clear:
                case DisplayStatus.result:
                case DisplayStatus.error:
                default:
                    return "";

            }
        }

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

        public string GetFormula2()
        {
            string s = "";

            foreach (var pair in FormulaElements)
            {
                if (pair.GetType() == typeof(decimal))
                    s = s + " " + pair.ToString();
                else
                    s = s + " " + GetFormulaSymbol(GetOperatorEnum<Operators>(pair));
            }
            return s;

            //{
            //switch (pair.Key)
            //{
            //    case FormulaElementType.number:
            //        s = s + numbers[pair.Value].ToString();
            //        break;
            //    case FormulaElementType.symbol:
            //        s = s + symbols[pair.Value].ToString();
            //        break;
            //}
            //}


        }

        public string Calculate()
        {
            decimal result = 0M;
            int l = 0;

            switch (ds)
            {
                case DisplayStatus.number:
                    for (int i = 0; i < num.Count(); i++)
                    {
                        if (i == 0)
                        {
                            result = num[i];
                            i++;
                        }
                        switch (op[l])
                        {
                            case Operators.plus:
                                result = result + num[i];
                                break;
                            case Operators.minus:
                                result = result - num[i];
                                break;
                            case Operators.multiply:
                                result = result * num[i];
                                break;
                            case Operators.divide:
                                result = result / num[i];
                                break;
                            default:
                                result = 0;
                                break;
                        }
                        l++;
                    }
                    num.Clear();
                    op.Clear();
                    num.Add(result);
                    ds = DisplayStatus.result;
                    return result.ToString();

                case DisplayStatus.operatorX:
                    return OperatorToString(op.Last());

                case DisplayStatus.decimalMark:
                    return num.Last().ToString();

                case DisplayStatus.result:
                    return num.Last().ToString();

                case DisplayStatus.clear:
                case DisplayStatus.error:
                default:
                    return "0";
            }

        }

        //---------------------------------------------------------------------------------------
        #endregion

        //=======================================================================================
        #region Clear functions

        public string Clear()
        {
            num.Clear();
            op.Clear();
            FormulaElements.Clear();
            ds = DisplayStatus.clear;
            DecimalCount = 0;
            return "0";
        }

        public string ClearEntry()
        {
            switch (ds)
            {
                case DisplayStatus.number:
                case DisplayStatus.decimalMark:
                    if (num.Count() > 1)
                        ds = DisplayStatus.operatorX;
                    else
                        ds = DisplayStatus.clear;

                    num.Remove(num.Last());
                    FormulaElements.Remove(num.Last());
                    return "0";

                case DisplayStatus.operatorX:
                    op.Remove(op.Last());
                    FormulaElements.Remove(op.Last());
                    ds = DisplayStatus.number;
                    return num.Last().ToString();

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

                    if (num.Last() == 0m)
                    {
                        mem = 0;
                        return false;
                    }
                    else
                    {
                        switch (mf)
                        {
                            case memorySaveMode.save:
                                mem = num.Last();
                                return true;
                            case memorySaveMode.plus:
                                mem = mem + num.Last();
                                return true;
                            case memorySaveMode.minus:
                                mem = mem - num.Last();
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

                    if (num.Any()) num.Remove(num.Last());
                    if (FormulaElements.Any()) FormulaElements.Remove(num.Last());
                    num.Add(d);
                    FormulaElements.Add(d);
                    return d.ToString();

                case DisplayStatus.operatorX:
                    num.Add(d);
                    FormulaElements.Add(d);
                    return d.ToString();
                default:
                    return "error";
            }
            
        }

        //---------------------------------------------------------------------------------------
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class formula
    {
        private List<object> FormulaElements = new List<object>();

        private enum FormulaElementType {number, _operator, numberPostfix, empty };

        private const string DecimalMark = ",";

        public enum Operators { plus, minus, multiply, divide, equals, percentage, squareRoot };

        private static string _Equals = "=";
        private static string _Plus = "+";
        private static string _Minus = "-";
        private static string _Multiply = "x";
        private static string _Divide = "/";
        private static string _Percentage = "%";
        private static string _SquareRoot = ((char)0x221A).ToString();


        private bool FormulaContainsNumber()
        {
            if (FormulaElements.OfType<decimal>().Any())
                return true;
            else
                return false;
        }

        private bool FormulaElementsEmpty
        {
            get 
            { 
            if (FormulaElements.Any())
                return false;
            else
                return true;
            }
        }

        private FormulaElementType LastFormulaElementType
        {
            get
            { 
                if (FormulaElementsEmpty)
                    return FormulaElementType.empty;
                if (FormulaElements.Last().GetType() == typeof(decimal))
                    return FormulaElementType.number;
                else if (FormulaElements.Last().GetType() == typeof(Operators))
                    return FormulaElementType._operator;
                else if (FormulaElements.Last().GetType() == typeof(string))
                    return FormulaElementType.numberPostfix;
                else
                    throw new NotImplementedException();
            }
        }

        private void RemoveLastElement()
        {
            if (!FormulaElementsEmpty)
                FormulaElements.Remove(FormulaElements.Last());
        }

        private void AddZero()
        {
            FormulaElements.Add(0m);
        }

        public void AddOperator(Operators o)
        {
            switch (LastFormulaElementType)
            {
                case FormulaElementType.empty:
                    AddZero();
                    break;
                case FormulaElementType._operator:
                    RemoveLastElement();
                    break;
            }

            FormulaElements.Add(o);
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

        public void AddDecimalMark()
        {
            if (LastFormulaElementType == FormulaElementType.number)
            { 
                switch (LastFormulaElementType)
                {
                    case FormulaElementType.empty:
                    case FormulaElementType._operator:
                        AddZero();
                        break;
                    case FormulaElementType.numberPostfix:
                        throw new Exception("number already contains decimalmark.");
                }

                if (Math.GetDecimalCount(Convert.ToDecimal(FormulaElements.Last())) != 0)
                    FormulaElements.Add(",");
                else
                    throw new Exception("number already contains decimalmark.");

            }
            else
                throw new Exception("cannot add decimalmark to non-number.");

        }

    }
}

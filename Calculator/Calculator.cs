using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {

        private calc c = new calc();

        public Calculator()
        {
            InitializeComponent();
            UpdateResult("0");
        }


        private void UpdateResult(string result)
        {
            txtResult.Text = result;
            txtFormula.Text = c.GetFormula();
        }

        //==================================
        #region buttonclick events Digits
        //==================================
        private void btn1_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(1M));
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(2M));
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(3M));
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(4M));
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(5M));
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(6M));
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(7M));
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(8M));
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(9M));
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDigit(0M));
        }
        //==================================
        #endregion
        //==================================

        //==================================
        #region buttonclick events Operators
        //==================================
        private void btnPlus_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Operator(global::Calculator.calc.Operators.plus));
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Operator(global::Calculator.calc.Operators.minus));
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Operator(global::Calculator.calc.Operators.multiply));
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Operator(global::Calculator.calc.Operators.divide));
        }
        //==================================
        #endregion
        //==================================

        //==================================
        #region buttonclick events Misc.
        //==================================
        private void btnDot_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDecimalMark());
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Calculate());
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            UpdateResult(c.PlusMinus());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Clear());
        }

        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            UpdateResult(c.ClearEntry());
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            UpdateResult(c.BackSpace());
        }

        private void btnOneDivideByX_Click(object sender, EventArgs e)
        {
            UpdateResult(c.OneDivideByX());
        }

        private void btnPercentage_Click(object sender, EventArgs e)
        {
            UpdateResult(c.Percentage());
        }

        private void btnSquareRoot_Click(object sender, EventArgs e)
        {
            UpdateResult(c.SquareRoot());
        }

        //==================================
        #endregion
        //==================================

        //==================================
        #region Memory
        //==================================

        private void MemoryStatusUpdate(bool on)
        {
            if (on)
                txtMemory.Text = "M";
            else
                txtMemory.Text = "";
        }

        private void btnMemoryClear_Click(object sender, EventArgs e)
        {
            // only update Memory textbox
            MemoryStatusUpdate(c.MemoryClear());
        }

        private void btnMemorySave_Click(object sender, EventArgs e)
        {
            // only update Memory textbox
            MemoryStatusUpdate(c.MemorySave(calc.memorySaveMode.save));
        }

        private void btnMemoryPlus_Click(object sender, EventArgs e)
        {
            // only update Memory textbox
            MemoryStatusUpdate(c.MemorySave(calc.memorySaveMode.plus));
        }

        private void btnMemoryMinus_Click(object sender, EventArgs e)
        {
            // only update Memory textbox
            MemoryStatusUpdate(c.MemorySave(calc.memorySaveMode.minus));
        }

        private void btnMemoryRead_Click(object sender, EventArgs e)
        {
            UpdateResult(c.MemoryRead());
        }

        //==================================
        #endregion
        //==================================
    }
}

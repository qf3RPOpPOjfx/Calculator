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

        //==================================
        #region main
        private calc c = new calc();

        public Calculator()
        {
            InitializeComponent();
        }

        private void ExceptionHandling(Exception e)
        {
            txtResult.Text = e.Message;
            txtFormula.Text = "";
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                string result = c.Calculate();
                UpdateResult(result);
            }
            catch (Exception exp)
            {
                ExceptionHandling(exp);
            }
        }

        private void UpdateResult(string result)
        {
            try
            {
                if (result!= null)
                { 
                    txtResult.Text = result;
                    txtFormula.Text = c.GetFormula();
                }
            }
            catch (Exception e)
            {
                ExceptionHandling(e);
            }
        }
        #endregion
        

        //==================================
        #region digits
        //==================================
        private void AddDigit(decimal d)
        {
            try
            { 
                string result = c.AddDigit(d);
                UpdateResult(result);
            }
            catch (Exception e)
            {
                ExceptionHandling(e);
            }

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            AddDigit(1m);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            AddDigit(2m);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            AddDigit(3m);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            AddDigit(4m);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            AddDigit(5m);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            AddDigit(6m);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            AddDigit(7m);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            AddDigit(8m);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            AddDigit(9m);
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            AddDigit(0m);
        }
        //==================================
        #endregion
        

        //==================================
        #region operators
        //==================================
        private void Operator(calc.Operators o)
        {
            try
            { 
                string result = c.Operator(o);
                UpdateResult(result);
            }
            catch (Exception e)
            {
                ExceptionHandling(e);
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            Operator(calc.Operators.plus);
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            Operator(calc.Operators.minus);
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            Operator(calc.Operators.multiply);
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            Operator(calc.Operators.divide);
        }
        //==================================
        #endregion
        

        //==================================
        #region decimalmark and plus/minus

        private void btnDecimalMark_Click(object sender, EventArgs e)
        {
            UpdateResult(c.AddDecimalMark());
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            UpdateResult(c.PlusMinus());
        }

        #endregion
        

        //==================================
        #region clear(entry) and backspace

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

        #endregion
        

        //==================================
        #region math misc.
        //==================================

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
            try
            {
                string result = c.SquareRoot();
                UpdateResult(result);
            }
            catch (Exception exp)
            {
                ExceptionHandling(exp);
            }
        }

        //==================================
        #endregion
        

        //==================================
        #region memory
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
        

    }
}

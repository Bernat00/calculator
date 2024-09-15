using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculator
{
    public partial class Form1 : Form
    {
        Calculator calculator;

        readonly char[] ops = CalculatorHelper.ops;
        readonly char[] nums = CalculatorHelper.nums;
        readonly char[] special = CalculatorHelper.special;
        string result = null;

        private bool nrootPressed = false;

        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("hu-HU");
            
            calculator = new Calculator(ops);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonPress(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button b = (Button)sender;

                if (b.Text == "x√y")
                {
                    if (nrootPressed)
                    {
                        Screen.Text += ")";
                        nrootPressed = false;
                    }
                    else
                    {
                        Screen.Text += "√(";
                        nrootPressed = true;
                    }
                    return;
                }

                Screen.Text += b.Text;
            }
        }

        private void Clear(object sender, EventArgs e)
        {
            InternalClear();
        }

        private void InternalClear()
        {
            Screen.Clear();
            result = null;
        }  

        private void AllKeyPress(object sender, KeyPressEventArgs e)
        {
                if (ops.Contains(e.KeyChar) && result != null)
                {
                    if(e.KeyChar == ')')
                        nrootPressed = false;

                    e.Handled = true;

                    Screen.Clear();

                    Screen.Text = result + e.KeyChar;

                    result = null;
                }

                else if (nums.Contains(e.KeyChar) || ops.Contains(e.KeyChar) || special.Contains(e.KeyChar))
                {
                    e.Handled = true;

                    if (result == null)
                        Screen.Text += e.KeyChar;

                    else
                    {
                        InternalClear();
                        Screen.Text += e.KeyChar;
                    }

                }
            
        }

        private void AllPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch(e.KeyData)
            {
                case Keys.Enter: ButtonEqual.Focus(); break;
                case Keys.Delete: InternalClear(); break;
                case Keys.Back:
                    if(Screen.Text.Length > 0)
                        Screen.Text = 
                        Screen.Text.Remove(Screen.Text.Length-1);
                    break;
            }
        }

        private void CalculateV2(object sender, EventArgs e)
        {
            Screen.Text = calculator.Calculate(Screen.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Calculator
{
    public partial class Form1 : Form
    {
        int inputSize = 0;
        Label input;
        Label expression;

        // New opperation
        bool newOP = false;

        // Empty Expression
        const string EMPTY = "";

        // Special input
        const int SIGN = -1;
        const int PERCENT = 20;
        const int POINT = 98;
        const int CLEAR = 99;
        const int EQUAL = 100;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Calculator";
            this.ClientSize = new Size(400, 610);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            Button[] buttonArray = new Button[19];
            CreatingButton(buttonArray);
            
            input = new Label();
            input.Size = new Size(400, 60);
            input.Location = new Point(0, 40);
            input.AutoSize = false;
            input.ForeColor = Color.Black;
            input.Font = new Font(input.Font.FontFamily, 40);
            input.Text = "0";
            input.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(input);
            
            expression = new Label();
            expression.Size = new Size(400, 30);
            expression.Location = new Point(0, 10);
            expression.AutoSize = false;
            expression.Padding = new Padding(0, 0, 20, 0);
            expression.ForeColor = Color.Gray;
            expression.Font = new Font(expression.Font.FontFamily, 20);
            expression.Text = EMPTY;
            expression.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(expression);
            
        }

        private void CreatingButton(Button[] buttonArray)
        {
            // Declare button identities
            string[] buttonTexts = {"C", "±", "%", "÷",
                                    "7", "8", "9", "×",
                                    "4", "5", "6", "-",
                                    "1", "2", "3", "+",
                                    "0", ".", "="};
            /* -1 => sign
             * 0-9 => number
             * 10-13 => binary operator
             * 20 => percent
             * 98 => point
             * 99 => clear
             * 100 => equal
             * */
            int[] buttonTags = { CLEAR, SIGN, PERCENT, 13,
                                7, 8, 9, 12,
                                4, 5, 6, 11,
                                1, 2, 3, 10,
                                0, POINT, EQUAL};

            const int BUTTON_SIZE = 100;
            int horizontal = 0;
            int vertical = 110;
            for (int i = 0; i < 19; i++)
            {                               
                buttonArray[i] = new Button();
                // Configure button properties
                buttonArray[i].Size = new Size(BUTTON_SIZE, BUTTON_SIZE);
                if (i == 16)
                    buttonArray[i].Size = new Size(BUTTON_SIZE * 2, BUTTON_SIZE);
                buttonArray[i].Location = new Point(horizontal, vertical);
                buttonArray[i].Text = buttonTexts[i];
                buttonArray[i].Font = new Font(buttonArray[i].Font.FontFamily, 20);


                // Button's Control
                buttonArray[i].Tag = buttonTags[i];
                buttonArray[i].Click += new EventHandler(ButtonClick);
                this.Controls.Add(buttonArray[i]);

                // Set up for next button
                if ((i + 1) % 4 == 0)
                {
                    vertical += BUTTON_SIZE;
                    horizontal = 0;
                }
                else
                {
                    horizontal += BUTTON_SIZE;
                    if (i == 16)
                        horizontal += BUTTON_SIZE;
                }
                
            }
        }
        
        private void ButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if ((int)btn.Tag == CLEAR)
            {
                input.Text = "0";
            }
            else if (0 <= (int)btn.Tag && (int)btn.Tag <= 9 || (int)btn.Tag == POINT)
            {
                if (newOP)
                    input.Text = "0";

                if (input.Text == "0")
                    input.Text = "";
                if (inputSize < 9)
                    if (input.Text == "-0")
                        input.Text = "-";
                    input.Text += btn.Text;
                newOP = false;
            }
            else if ((int)btn.Tag == SIGN)
            {
                if (input.Text[0] == '-')
                    input.Text = input.Text.Substring(1);
                else
                    input.Text = "-" + input.Text;
            }
            else if (10 <= (int)btn.Tag && (int)btn.Tag <= 13)
            {
                if (expression.Text != EMPTY)
                {
                    string[] token = expression.Text.Split(' ');
                    double a = double.Parse(token[0]);
                    string op = token[1];
                    double b = double.Parse(input.Text);

                    input.Text = BinaryOperation(a, b, op).ToString();
                    expression.Text = EMPTY;
                }
                expression.Text = input.Text + " " + btn.Text;
                input.Text = "0";
                newOP = true;
            }
            else if ((int)btn.Tag == EQUAL)
            {
                if (expression.Text != EMPTY)
                {
                    string[] token = expression.Text.Split(' ');
                    double a = double.Parse(token[0]);
                    string op = token[1];
                    double b = double.Parse(input.Text);

                    if (op == "÷" && input.Text == "0")
                        input.Text = "Ma Error";
                    else
                        input.Text = BinaryOperation(a, b, op).ToString();
                    expression.Text = EMPTY;
                    newOP = true;
                }
            }
            else if ((int)btn.Tag == PERCENT)
            {
                input.Text = BinaryOperation(double.Parse(input.Text), 100, "×").ToString();
                newOP = true;
            }

        }

        private double BinaryOperation(double a, double b, string op)
        {
            if (op == "+")
                return a + b;
            else if (op == "-")
                return a - b;
            else if (op == "×")
                return a * b;
            else
                return a / b;
        }


    }
}

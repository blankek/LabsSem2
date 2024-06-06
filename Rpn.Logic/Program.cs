using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;

namespace Rpn.Logic
{
    class Token
    {

    }

    class Number : Token
    {
        public double Symbol { get; }

        public Number(double num)
        {
            Symbol = num;
        }
    }

    class Variable : Token
    {
        public char Symbol { get; }

        public Variable(char symbol)
        {
            Symbol = symbol;
        }
    }
    class Paranthesis : Token
    {
        public char Symbol { get; }
        public bool isClosing { get; }

        public Paranthesis(char symbol)
        {
            Symbol = symbol;
            isClosing = symbol == ')';
        }
    }

    public class RpnCalculator
    {
        private List<Token> RPN;
        public double Result;

        public RpnCalculator(string expression, double xValue = 0)
        {
            RPN = toRPN(Tokenize(expression));
            Result = Calculate(RPN, xValue);
        }

        private List<Token> Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();
            string number = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                var c= input[i];
                if (char.IsDigit(c) || c== '.')
                {
                    number += c;
                }                
                else if (c == '+' || c == '-' || c == '*' || c == '/' || c=='^')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number, CultureInfo.InvariantCulture)));
                        number = string.Empty;
                    }
                    switch (c)
                    {
                        case '*':
                            tokens.Add(new Multiply());
                            continue;
                        case '/':
                            tokens.Add(new Divide());
                            continue;
                        case '+':
                            tokens.Add(new Plus());
                            continue;
                        case '-':
                            tokens.Add(new Minus());
                            continue;
                        case '^':
                            tokens.Add(new Power());
                            continue;
                    }
                }
                else if (c == '(' || c == ')')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number, CultureInfo.InvariantCulture)));
                        number = string.Empty;
                    }
                    tokens.Add(new Paranthesis(c));
                }
                else if (c == 'x')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number, CultureInfo.InvariantCulture)));
                        number = string.Empty;
                    }
                    tokens.Add(new Variable('x'));
                }                
                else if (c == 'l' && input.Substring(i, 3).ToLower() == "log")
                {
                    tokens.Add(new Log());
                    i += 2;
                }              
                else if (c == 's' && input.Substring(i, 3).ToLower() == "sin")
                {
                    tokens.Add(new Sin());
                    i += 2;
                }
                else if (c == 'c' && input.Substring(i, 3).ToLower() == "cos")
                {
                    tokens.Add(new Cos());
                    i += 2;
                }
                else if (c == 't' && input.Substring(i, 2).ToLower() == "tg")
                {
                    tokens.Add(new Tg());
                    i += 1;
                }
                else if (c == 'c' && input.Substring(i, 3).ToLower() == "ctg")
                {
                    tokens.Add(new Ctg());
                    i += 2;
                }
                else if (c == 's' && input.Substring(i, 4).ToLower() == "sqrt")
                {
                    tokens.Add(new Sqrt());
                    i += 3;
                }
                else if (c == 'r' && input.Substring(i, 2).ToLower() == "rt")
                {
                    tokens.Add(new Rt());
                    i += 1;
                }
            }

            if (number != string.Empty)
            {
                tokens.Add(new Number(double.Parse(number, CultureInfo.InvariantCulture)));
            }

            return tokens;
        }

        private List<Token> toRPN(List<Token> tokens)
        {
            List<Token> rpnOutput = new List<Token>();
            Stack<Token> operators = new Stack<Token>();
            string number = string.Empty;

            foreach (Token token in tokens)
            {
                if (operators.Count == 0 && !(token is Number) && !(token is Variable))
                {
                    operators.Push(token);
                    continue;
                }

                if (token is Operation)
                {
                    if (operators.Peek() is Paranthesis)
                    {
                        operators.Push(token);
                        continue;
                    }

                    Operation first = (Operation)token;
                    Operation second = (Operation)operators.Peek();

                    if (first.Priority > second.Priority)
                    {
                        operators.Push(token);
                    }
                    else if (first.Priority <= second.Priority)
                    {
                        while (operators.Count > 0 && !(token is Paranthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }
                        operators.Push(token);
                    }
                }
                else if (token is Paranthesis)
                {
                    if (((Paranthesis)token).isClosing)
                    {
                        while (!(operators.Peek() is Paranthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }

                        operators.Pop();
                    }
                    else
                    {
                        operators.Push(token);
                    }
                }
                else if (token is Number || token is Variable)
                {
                    rpnOutput.Add(token);
                }
            }

            while (operators.Count > 0)
            {
                rpnOutput.Add(operators.Pop());
            }
            return rpnOutput;
        }
        public double Calculate(double xValue)
        {
            return Calculate(RPN, xValue);
        }


        private double Calculate(List<Token> rpnCalc, double xValue)
        {
            Stack<double> tempCalc = new Stack<double>();

            foreach (Token token in rpnCalc)
            {
                if (token is Number num)
                {
                    tempCalc.Push(num.Symbol);
                }
                else if (token is Variable)
                {
                    tempCalc.Push(xValue);
                }
                else if (token is Operation op)
                {
                    int requiredOperands = ((Operation)token).ArgsCount;

                    double[] operands = new double[requiredOperands];
                    for (int i = 0; i < requiredOperands; i++)
                    {
                        operands[i] = tempCalc.Pop();
                    }

                    double result = ((Operation)token).Execute(operands);
                    tempCalc.Push(result);               
                }
            }

            if (tempCalc.Count != 1)
            {
                throw new InvalidOperationException("Stack should contain only one result.");
            }

            return tempCalc.Pop();
        }
    }
}
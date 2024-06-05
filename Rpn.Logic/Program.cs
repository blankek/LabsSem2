using System.ComponentModel;

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

    class Operation : Token
    {
        public char Symbol { get; }
        public int Priority { get; }

        public Operation(char symbol)
        {
            Symbol = symbol;
            Priority = GetPriority(symbol);
        }

        private static int GetPriority(char symbol)
        {
            switch (symbol)
            {
                case '(': return 0;
                case ')': return 0;
                case '+': return 1;
                case '-': return 1;
                case '*': return 2;
                case '/': return 2;
                default: return 3;
            }
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
            foreach (var c in input)
            {
                if (char.IsDigit(c))
                {
                    number += c;
                }
                else if (c == ',' || c == '.')
                {
                    number += ",";
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Operation(c));
                }
                else if (c == '(' || c == ')')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Paranthesis(c));
                }
                else if (c == 'x')
                {
                    if (number != string.Empty)
                    {
                        tokens.Add(new Number(double.Parse(number)));
                        number = string.Empty;
                    }
                    tokens.Add(new Variable('x'));
                }
            }

            if (number != string.Empty)
            {
                tokens.Add(new Number(double.Parse(number)));
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
                    if (tempCalc.Count < 2)
                    {
                        throw new InvalidOperationException("Stack empty.");
                    }

                    double first = tempCalc.Pop();
                    double second = tempCalc.Pop();

                    double result = 0;
                    switch (op.Symbol)
                    {
                        case '+': result = second + first; break;
                        case '-': result = second - first; break;
                        case '*': result = first * second; break;
                        case '/': result = second / first; break;
                    }
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

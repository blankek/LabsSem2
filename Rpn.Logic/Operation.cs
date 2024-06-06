using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpn.Logic
{
    abstract class Operation : Token
    {
        public abstract string Name { get; }
        public abstract int Priority { get; }
        public abstract int ArgsCount { get; }
        public abstract bool IsFunction { get; }

        public abstract double Execute(params double[] numbers);

        public override string ToString()
        {
            return Name;
        }
    }

    class Plus : Operation
    {
        public override string Name => "+";
        public override int Priority => 1;
        public override int ArgsCount => 2;
        public override bool IsFunction => false;

        public override double Execute(params double[] numbers)
        {
            return (numbers[1] + numbers[0]);
        }
    }

    class Minus : Operation
    {
        public override string Name => "-";
        public override int Priority => 1;
        public override int ArgsCount => 2;
        public override bool IsFunction => false;

        public override double Execute(params double[] numbers)
        {
            return (numbers[1] - numbers[0]);
        }
    }

    class Multiply : Operation
    {
        public override string Name => "*";
        public override int Priority => 2;
        public override int ArgsCount => 2;
        public override bool IsFunction => false;

        public override double Execute(params double[] numbers)
        {
            return (numbers[1] * numbers[0]);
        }
    }

    class Divide : Operation
    {
        public override string Name => "/";
        public override int Priority => 2;
        public override int ArgsCount => 2;
        public override bool IsFunction => false;

        public override double Execute(params double[] numbers)
        {
            return (numbers[1] / numbers[0]);
        }
    }

    class Power : Operation
    {
        public override string Name => "^";
        public override int Priority => 3;
        public override int ArgsCount => 2;
        public override bool IsFunction => false;

        public override double Execute(params double[] numbers)
        {
            return (Math.Pow(numbers[1], numbers[0]));
        }
    }

    class Sin : Operation
    {
        public override string Name => "sin";
        public override int Priority => 3;
        public override int ArgsCount => 1;
        public override bool IsFunction => true;

        public override double Execute(params double[] numbers)
        {
            return (Math.Sin(numbers[0]));
        }
    }

    class Cos : Operation
    {
        public override string Name => "cos";
        public override int Priority => 3;
        public override int ArgsCount => 1;
        public override bool IsFunction => true;

        public override double Execute(params double[] numbers)
        {
            return (Math.Cos(numbers[0]));
        }
    }

    class Tg : Operation
    {
        public override string Name => "tg";
        public override int Priority => 3;
        public override int ArgsCount => 1;
        public override bool IsFunction => true;

        public override double Execute(params double[] numbers)
        {
            return (Math.Tan(numbers[0]));
        }
    }

    class Ctg : Operation
    {
        public override string Name => "ctg";
        public override int Priority => 3;
        public override int ArgsCount => 1;
        public override bool IsFunction => true;

        public override double Execute(params double[] numbers)
        {
            return (1.0 / Math.Tan(numbers[0]));
        }
    }

    class Log : Operation
    {
        public override string Name => "log";
        public override int Priority => 3;
        public override int ArgsCount => 2;
        public override bool IsFunction => true;

        public override double Execute(params double[] numbers)
        {
            return (Math.Log(numbers[1], numbers[0]));
        }
    }

    class Sqrt : Operation
    {
        public override string Name => "sqrt";
        public override int Priority => 3;
        public override int ArgsCount => 1;
        public override bool IsFunction => true;

        public override double Execute(params double[] numbers)
        {
            return (Math.Sqrt(numbers[0]));
        }
    }
    class Rt : Operation
    {
        public override string Name => "rt";
        public override int Priority => 3;
        public override int ArgsCount => 2;
        public override bool IsFunction => true;
        public override double Execute(params double[] numbers)
        {
            return (Math.Pow(numbers[1], 1 / numbers[0]));
        }
    }
}

using System.Numerics;

namespace Calculator
{
    public class Operation<T> where T : INumber<T>
    {
        private Func<T[], T> _func;

        public Operation(Func<T[], T> func)
        {
            _func = func;
        }

        public T Execute(T[] args)
        {
            return _func(args);
        }
    }


    public class Calculator<T> where T : INumber<T>
    {
        public Operation<T> SumOperation { get; set; } = new Operation<T>(args =>
        {
            T result = T.Zero;
            foreach (var arg in args)
                result += arg;
            return result;
        });

        public Operation<T> SubtractOperation { get; set; } = new Operation<T>(args =>
        {
            if (args.Length < 2)
            {
                Console.WriteLine("needed at least 2 arguments");
                return T.Zero;
            }

            T result = args[0];
            for (int i = 1; i < args.Length; i++)
                result -= args[i];
            return result;
        });

        public T Custom(Operation<T> operation, params T[] args)
        {
            return operation.Execute(args);
        }
    }

    public class Program2
    {
        class Program
        {
            public static void Main()
            {
                Calculator<double> calc = new Calculator<double>();
                var sumRes = calc.SumOperation.Execute([1, 2, 3, 75]);
                var subRes = calc.SubtractOperation.Execute([1, 2, 3, 75]);
                var custom = new Operation<double>(args => args[0] * args[1] * args[2]);
                var customRes = calc.Custom(custom, 4, 5, 2);
                Console.WriteLine(sumRes);
                Console.WriteLine(subRes);
                Console.WriteLine(customRes);
                Console.WriteLine("\n\nIt just works!\n\n");
            }
        }
    }
}
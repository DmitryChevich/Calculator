using System.Numerics;

namespace Calculator
{
    public interface ICalculator<T> where T : INumber<T>
    {
        public Operation<T> SumOperation { get; }
        public Operation<T> SubtractOperation { get; }
        public void ShowHistory();
    }

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


    public class Calculator<T> : ICalculator<T> where T : INumber<T>
    {
        public record History
        {
            public string Func = "";
            public T[] HistoryArgs = [];
            public T Result = T.Zero;
        }

        static List<History> _history = new List<History>();

        private void WriteHistory(History history)
        {
            Console.WriteLine($"Name:\n " + history.Func + $"\nArgs");
            foreach (var arg in history.HistoryArgs)
            {
                Console.WriteLine($" " + arg);
            }

            Console.WriteLine($"Result:\n {history.Result}");
        }

        public Operation<T> SumOperation { get; set; } = new Operation<T>(args =>
        {
            T result = T.Zero;
            foreach (var arg in args)
                result += arg;

            var historySum = new History
            {
                Func = "Sum", HistoryArgs = args, Result = result
            };

            _history.Add(historySum);

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

            var historySub = new History
            {
                Func = "Subtract", HistoryArgs = args, Result = result
            };

            _history.Add(historySub);

            return result;
        });

        public T Custom(Operation<T> operation, params T[] args)
        {
            var result = operation.Execute(args);


            var historyCustom = new History
            {
                Func = $"Custom {operation}", HistoryArgs = args, Result = result
            };
            _history.Add(historyCustom);

            return result;
        }

        public void ShowHistory()
        {
            for (int i = 0; i < _history.Count; i++)
            {
                Console.WriteLine();
                WriteHistory(_history[i]);
            }
        }
    }

    public class Program2
    {
        public static void Main()
        {
            Calculator<double> calc = new Calculator<double>();
            var sumRes = calc.SumOperation.Execute([1, 2, 3, 75]);
            var subRes = calc.SubtractOperation.Execute([1, 2, 3, 75]);
            var custom = new Operation<double>(args => args[0] * args[1] * args[2]);
            var customRes = calc.Custom(custom, 4, 5, 2);
            calc.ShowHistory();
            Console.WriteLine("\nIt just works!\n");
        }
    }
}
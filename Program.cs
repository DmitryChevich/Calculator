using System.Text.Json;
using System.Text.Json.Serialization;

namespace Calculator
{
    internal interface ICalculator
    {
        public long Sum();
        public long Sub();
        public long Mult();
        public long Div();
    }

    internal record History
    {
        [JsonInclude] public long Number1;
        [JsonInclude] public long Number2;
        [JsonInclude] public char Sign;
        [JsonInclude] public long Result;
    }

    internal class Calculator(long number1, long number2) : ICalculator
    {
        List<History> history = new List<History>();

        public long Sum()
        {
            var historySum = new History
                { Number1 = number1, Number2 = number2, Sign = '+', Result = number1 + number2 };
            history.Add(historySum);
            return number1 + number2;
        }

        public long Sub()
        {
            var historySub = new History
                { Number1 = number1, Number2 = number2, Sign = '-', Result = number1 - number2 };
            history.Add(historySub);
            return number1 - number2;
        }

        public long Mult()
        {
            var historyMult = new History
                { Number1 = number1, Number2 = number2, Sign = '*', Result = number1 * number2 };
            history.Add(historyMult);
            return number1 * number2;
        }

        public long Div()
        {
            var historyDiv = new History
                { Number1 = number1, Number2 = number2, Sign = '/', Result = number1 / number2 };
            history.Add(historyDiv);
            return number1 / number2;
        }

        public async void ShowHistory()
        {
            await using var fs = new FileStream("history.json", FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, history);
            fs.Flush();
            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine(history[i]);
            }
        }

        public double Average()
        {
            var av = (from p in history where p.Result != 0 select p).Average(f => f.Result);
            return av;
        }
    }


    internal class Program
    {
        public static void Main()
        {
            var calculator = new Calculator(8, 2);
            Console.WriteLine(calculator.Sum());
            Console.WriteLine(calculator.Sub());
            Console.WriteLine(calculator.Mult());
            Console.WriteLine(calculator.Div());
            Console.WriteLine(calculator.Average());
            calculator.ShowHistory();
        }
    }
}
using System.Text.Json;

namespace Calculator
{
    internal interface ICalculator
    {
        public long Sum();
        public long Sub();
        public long Mult();
        public long Div();
    }

    internal ref struct History
    {
        public long Number1;
        public long Number2;
        public char Sign;
        public long Result;
    }

    internal class Calculator(long number1, long number2) : ICalculator
    {
        public long Sum()
        {
            var history = new History
                { Number1 = number1, Number2 = number2, Sign = '+', Result = number1 + number2 };
            var historyString =
                $"{history.Number1.ToString()} {history.Sign.ToString()} {history.Number2.ToString()} = {history.Result.ToString()}";
            _ = ToFile(historyString);
            return number1 + number2;
        }

        public long Sub()
        {
            var history = new History
                { Number1 = number1, Number2 = number2, Sign = '-', Result = number1 - number2 };
            var historyString =
                $"{history.Number1.ToString()} {history.Sign.ToString()} {history.Number2.ToString()} = {history.Result.ToString()}";
            _ = ToFile(historyString);
            return number1 - number2;
        }

        public long Mult()
        {
            var history = new History
                { Number1 = number1, Number2 = number2, Sign = '*', Result = number1 * number2 };
            var historyString =
                $"{history.Number1.ToString()} {history.Sign.ToString()} {history.Number2.ToString()} = {history.Result.ToString()}";
            _ = ToFile(historyString);
            return number1 * number2;
        }

        public long Div()
        {
            var history = new History
                { Number1 = number1, Number2 = number2, Sign = '/', Result = number1 / number2 };
            var historyString =
                $"{history.Number1.ToString()} {history.Sign.ToString()} {history.Number2.ToString()} = {history.Result.ToString()}";

            _ = ToFile(historyString);
            return number1 / number2;
        }

        private static async Task ToFile(string historyString)
        {
            var historyJson = JsonSerializer.Serialize(historyString);
            await using var fs = new FileStream("history.json", FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, historyJson);
        }


        public static async Task ShowHistory()
        {
            await using var fs = new FileStream("history.json", FileMode.OpenOrCreate);
            var historyJsonDes = await JsonSerializer.DeserializeAsync<string>(fs);
            Console.WriteLine(historyJsonDes);
        }
    }


    internal abstract class Program
    {
        public static void Main()
        {
            var calculator = new Calculator(8, 2);
            Console.WriteLine(calculator.Sum());
            Console.WriteLine(calculator.Sub());
            Console.WriteLine(calculator.Mult());
            Console.WriteLine(calculator.Div());
            _ = Calculator.ShowHistory();
        }
    }
}
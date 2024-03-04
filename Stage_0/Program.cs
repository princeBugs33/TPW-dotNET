using System;

namespace Calculator
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Calculator calculator = new Calculator();
            int result = calculator.Add(1, 2);
            Console.WriteLine("Result of Add(1, 2) method: " + result);
        }
    }
}

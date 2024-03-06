using System;

namespace Stage_0
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Calc calculator = new Calc();
            int result = calculator.Add(1, 2);
            Console.WriteLine("Result of Add(1, 2) method: " + result);
        }
    }
}

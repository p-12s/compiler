using System;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            CalcParser parser = new CalcParser();

            string input = string.Empty;
            while (true)
            {
                input = Console.ReadLine();
                Console.WriteLine(parser.Calculate(input));
            }

        }
    }
}

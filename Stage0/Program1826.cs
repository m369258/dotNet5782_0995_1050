// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;

namespace Stage0
{
  partial class Program
    {
        static void Main()
        {
            Welcome1826();
            Welcome3114();
            Console.ReadKey();
        }

        static partial void Welcome3114();
        private static void Welcome1826()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }

    }
}
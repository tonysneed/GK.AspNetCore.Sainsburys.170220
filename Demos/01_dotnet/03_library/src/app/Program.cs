using System;
using ClassLibrary;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var echo = Hello.Echo("Hello World!");
            Console.WriteLine(echo);
            var echo2 = ClassLibrary2.Hello.Echo("Hello World!");
            Console.WriteLine(echo2);
        }
    }
}

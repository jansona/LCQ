using System;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Test t = (Test)Enum.Parse(typeof(Test), "2");
            Console.WriteLine((int)t);
        }

        public enum Test
        {
            a = 1,
            b ,
            c,
            d,
            e
        }
    }
}

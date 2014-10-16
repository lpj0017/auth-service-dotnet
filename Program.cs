using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RongAuthClient.Instance.Auth("xxxxxxx", "xxxxxxx", "1", "demon", ""));
            Console.Read();
        }
    }
}

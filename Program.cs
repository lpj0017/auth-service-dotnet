using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  com.rongcloud.demo.auth
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(RongAuthClient.Instance.Auth("yourappkey", "youappSecret", "youruserid", "youusername", "youuserportrait"));
            }
            Console.Read();
        }
    }
}

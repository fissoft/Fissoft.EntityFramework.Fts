using System;

namespace Fissoft.EntityFramework.Fts.Tests
{
    public class ConsoleUtil
    {
        public static void Write() { }

        public static void Write(string str)
        {
            if (str.StartsWith("SELECT") || str.StartsWith("-- p__linq__"))
                Console.WriteLine(str);
        }
    }
}
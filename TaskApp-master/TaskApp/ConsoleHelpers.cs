using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp
{
    static class ConsoleHelpers
    {
        public static string ReadPasswordLine()
        {
            var passwordBuffer = new List<char>();
            while (true)
            {
                ConsoleKeyInfo ch = Console.ReadKey(true);
                switch (ch.Key)
                {
                    case ConsoleKey.Enter:
                        return new string(passwordBuffer.ToArray());
                    case ConsoleKey.Backspace:
                        if (passwordBuffer.Count > 0) passwordBuffer.RemoveAt(passwordBuffer.Count - 1);
                        break;
                    default:
                        passwordBuffer.Add(ch.KeyChar);
                        break;
                }
            }
        }
    }
}

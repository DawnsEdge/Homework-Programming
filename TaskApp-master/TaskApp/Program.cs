using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TaskApp!");
            App app = new App();
            app.Run();
        }
    }
}

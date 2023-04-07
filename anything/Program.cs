using System;
using System.Threading.Tasks;
using anything.Components;

namespace anything
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            while (true)
            {
                Console.WriteLine(Settings.Logo);

                Console.Write("\n\nEnter Token > ");
                string token = Console.ReadLine();

                Console.Write("Enter Message > ");
                string msg = Console.ReadLine();

                Console.Write("Enter Amount > ");
                string amt = Console.ReadLine();
                int amount = Convert.ToInt32(amt);

                Console.Write("Enter Channel ID > ");
                string gid = Console.ReadLine();

                await Events.SpamMessage(token, msg, gid, amount);

                Console.Write("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
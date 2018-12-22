using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using briskbot.access;
using System.Threading;
using briskbot.game;

namespace briskbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(NuclearOption);

            Uri endpoint = new Uri("http://www.briskchallenge.com");

            using(var client = new HttpClient())
            {
                client.BaseAddress = endpoint;

                run(client).GetAwaiter().GetResult();  
            }
        }

        public async static Task run(HttpClient client)
        {
            IApiClient apiClient = new ApiClient(client);
            GameAccess newAccess = new GameAccess(apiClient);  
            WarRoom board = await WarRoom.CreateWarRoom(newAccess);
            int waitCount = 0;

            while(waitCount < 10)
            {
                TurnStatus state = await board.TakeTurn();

                switch(state)
                {
                    case TurnStatus.TookTurn:
                        Console.WriteLine("And so my watch has ended (for now)");
                        waitCount = 0;
                        break;
                    case TurnStatus.Waiting:
                        Console.WriteLine("waits patiently...");
                        waitCount++;
                        break;
                    case TurnStatus.LostGame:
                        Console.WriteLine($"Valar Morghulis");
                        return;
                    case TurnStatus.WonGame:
                        Console.WriteLine($"Hail to the King!");
                        return;
                    default:
                        Console.WriteLine("Status Unavailable");
                    break;
                }
            }
        }

        protected static void NuclearOption(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Nuclear Launch Detected.");
            Environment.Exit(Environment.ExitCode);
        }
    }
}

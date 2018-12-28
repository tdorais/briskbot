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
            IGameAccess newAccess = await GameAccess.Create(apiClient, "Pinky and the Brain");  
            WarRoom board = new WarRoom(newAccess);

            Console.WriteLine($"Game: {newAccess.GameId}");

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
                        Console.WriteLine("Waits patiently...");
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
                
                Thread.Sleep(1000); //optimize
            }
        }

        protected static void NuclearOption(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Nuclear Launch Detected.");
            Environment.Exit(Environment.ExitCode);
        }
    }
}

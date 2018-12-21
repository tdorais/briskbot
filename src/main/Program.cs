using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using briskbot.access;
using System.Threading;

namespace briskbot
{
    class Program
    {
        static bool killSwitch = false;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(NuclearOption);

            Uri endpoint = new Uri("http://www.briskchallenge.com");

            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = endpoint;
                ApiClient client = new ApiClient(httpClient);

                run(client).GetAwaiter().GetResult();  
            }
        }

        public async static Task run(ApiClient client)
        {           
            GameAccess access = new GameAccess(client);  
            GameResult result = await access.CreateGame();

            Console.WriteLine($"Valar Morghulis: {result.game}");

            Turn gamestate = await access.CheckTurn(result.game, result.player);
            int waitCount = 0;

            while((gamestate.winner ?? 0) == 0 || waitCount > 10)
            {
                if(killSwitch)
                {
                    Environment.Exit(Environment.ExitCode);
                }

                if(gamestate.current_turn ?? false)
                {
                    waitCount = 0;
                    Console.WriteLine("And so my watch has ended (for now)");
                    await access.EndTurn(result.game, result.player, result.token);
                    gamestate.current_turn = false;
                }
                else
                {
                    Console.WriteLine("waits patiently...");
                    Thread.Sleep(1000); //optimize
                    waitCount++;
                    gamestate = await access.CheckTurn(result.game, result.player);
                }
            }

            Console.WriteLine($"Hail to the King, {gamestate.winner}");
        }

        protected static void NuclearOption(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Nuclear Launch Detected.");
            killSwitch = true;
            args.Cancel = false;
        }
    }
}

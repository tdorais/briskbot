using System;
using System.Net.Http;
using briskbot.access;
using System.Threading.Tasks;
using System.Threading;

namespace briskbot.game
{
    public enum TurnStatus
    {
        NA,
        TookTurn,
        Waiting,
        LostGame,
        WonGame
    }
    // Gentlemen, you can't fight here! This is the war room!
    public class WarRoom
    {
        private readonly IGameAccess access;
        private readonly GameResult gameInfo;

        public async static Task<WarRoom> CreateWarRoom(IGameAccess newAccess)
        {
            GameResult newInfo = await newAccess.CreateGame("Pinky and the Brain");
            WarRoom newRoom = new WarRoom(newAccess, newInfo);

            Console.WriteLine($"Game: {newInfo.game}");

            return newRoom;
        }
        private WarRoom(IGameAccess newAccess, GameResult info)
        {
            access = newAccess;
            gameInfo = info;
        }

        public async Task<TurnStatus> TakeTurn()
        {
            Turn gameState = await access.CheckTurn(gameInfo.game, gameInfo.player);

            int winner = gameState.winner ?? 0;
            if(winner == gameInfo.player)
                return TurnStatus.WonGame;
            else if (winner > 0)
                return TurnStatus.LostGame;
            

            var turn = TurnStatus.Waiting;
            if(gameState.current_turn ?? false)
            {
                await access.EndTurn(gameInfo.game, gameInfo.player, gameInfo.token);
                turn = TurnStatus.TookTurn;
            }

            Thread.Sleep(1000); //optimize
            return turn;
        }
    }
}

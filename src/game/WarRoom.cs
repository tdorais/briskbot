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
        
        public WarRoom(IGameAccess newAccess)
        {
            access = newAccess;
        }

        public async Task<TurnStatus> TakeTurn()
        {
            PlayerState state = await access.GetPlayerState();

            //Bouncer methods to shortcut out of taking the turn
            {
                int winner = state.winner ?? 0;
                if(winner == access.CurrentPlayer)
                    return TurnStatus.WonGame;
                else if (winner > 0)
                    return TurnStatus.LostGame;

                if(!(state.current_turn ?? false))  
                    return TurnStatus.Waiting;
            }

            Territory HQ = state.territories[0];

            //place reinforcements
            {
                bool placed = await access.PlaceArmies(HQ.territory, state.num_reserves);
                if(placed)
                    Console.WriteLine($"Reinforced {HQ.territory_name} with {state.num_reserves} battalions");
            }
            
            await access.EndTurn();

            return TurnStatus.TookTurn;
        }
    }
}

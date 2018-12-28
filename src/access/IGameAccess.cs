using System.Threading.Tasks;
using System.Net;

namespace briskbot.access
{
    public interface IGameAccess
    {
        int GameId { get; }
        string Token { get; }
        int CurrentPlayer { get; }

        Task<GameResult> CreateGame(string teamName);

        Task<PlayerState> GetPlayerState();
        
        Task<bool> PlaceArmies(int territory, int num);

        Task<bool> EndTurn();
    }
}
using System.Threading.Tasks;
using System.Net;

namespace briskbot.access
{
    public interface IGameAccess
    {
        Task<GameResult> CreateGame(string teamName);

        Task<PlayerState> GetPlayerState(int gameId, int playerId);

        Task<Turn> CheckTurn(int gameId, int playerId);

        Task<HttpStatusCode> EndTurn(int gameId, int playerId, string token);
    }
}
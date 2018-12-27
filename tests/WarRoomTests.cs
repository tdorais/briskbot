using System;
using Xunit;
using briskbot.access;
using Moq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using briskbot.game;

namespace briskbot.tests
{
    public class WarRoomTests
    {
        private readonly Mock<IApiClient> mockClient;
        private readonly Mock<IGameAccess> mockAccess;

        public WarRoomTests()
        {
            mockClient = new Mock<IApiClient>();
            mockAccess = new Mock<IGameAccess>();
        }

        [Theory]
        [InlineData(true, null, TurnStatus.TookTurn)]
        [InlineData(true, 0, TurnStatus.TookTurn)]
        [InlineData(false, null, TurnStatus.Waiting)]
        [InlineData(false, 0, TurnStatus.Waiting)]
        [InlineData(true, 1, TurnStatus.WonGame)]
        [InlineData(false, 1, TurnStatus.WonGame)]
        [InlineData(true, 2, TurnStatus.LostGame)]
        [InlineData(false, 2, TurnStatus.LostGame)]
        public async Task TakeTurnReturnsStatus(bool currentTurn, int? winner, TurnStatus status)
        {
            GameResult mockedResult = new GameResult(){player = 1};
            Turn mockedTurn = new Turn() {winner = winner, current_turn = currentTurn};
            mockAccess.Setup(a => a.CreateGame(It.IsAny<string>())).ReturnsAsync(mockedResult);
            mockAccess.Setup(a => a.CheckTurn(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mockedTurn);
            WarRoom board = await WarRoom.CreateWarRoom(mockAccess.Object);

            TurnStatus actual = await board.TakeTurn();

            Assert.Equal(status, actual);
        }
    }
}

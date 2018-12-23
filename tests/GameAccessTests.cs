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
    public class GameAccessTests
    {
        private readonly Mock<IApiClient> mockClient;
        private readonly GameAccess access;

        public GameAccessTests()
        {
            var mockAccess = new Mock<IGameAccess>();
            mockClient = new Mock<IApiClient>();
            
            access = new GameAccess(mockClient.Object);
        }

        [Fact]
        public async Task PostCallsThatReturnNonSuccessfullyThrowExceptions()
        {
            HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(badResponse);  

            await Assert.ThrowsAsync<Exception>(() => access.CreateGame("Test Tanks"));
        }

        [Fact]
        public async Task GetCallsThatReturnNonSuccessfullyThrowExceptions()
        {
            HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockClient.Setup(c => c.Get("/v1/brisk/game/1/player/1?check_turn=true")).ReturnsAsync(badResponse);

            await Assert.ThrowsAsync<Exception>(() => access.CheckTurn(1,1));
        }

        [Fact]
        public async Task CreateGameReturnsGameId()
        {   
            HttpResponseMessage createdResponse = new HttpResponseMessage(HttpStatusCode.OK);
            createdResponse.Content = new StringContent("{'game':1}");    
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(createdResponse);  

            GameResult actual = await access.CreateGame("Test Tanks");

            Assert.Equal<int>(1, actual.game);
        }
        
        [Fact]
        public async Task CreateGameReturnsPlayerId()
        {   
            HttpResponseMessage createdResponse = new HttpResponseMessage(HttpStatusCode.OK);
            createdResponse.Content = new StringContent("{'player':1}");    
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(createdResponse);  

            GameResult actual = await access.CreateGame("Test Tanks");

            Assert.Equal<int>(1, actual.player);
        }

        [Fact]
        public async Task GetPlayerStateReturnsCurrentTurn()
        {
            var playerStateResponse = new HttpResponseMessage(HttpStatusCode.OK);
            playerStateResponse.Content = new StringContent("{'current_turn':true}");
            mockClient.Setup(c => c.Get("/v1/brisk/game/1/player/1")).ReturnsAsync(playerStateResponse);

            PlayerState actual = await access.GetPlayerState(1,1);

            Assert.True(actual.current_turn);
        }

        [Fact]
        public async Task CheckTurnReturnsCurrentTurn()
        {
            var playerStateResponse = new HttpResponseMessage(HttpStatusCode.OK);
            playerStateResponse.Content = new StringContent("{'current_turn':true}");
            mockClient.Setup(c => c.Get("/v1/brisk/game/1/player/1?check_turn=true")).ReturnsAsync(playerStateResponse);

            Turn actual = await access.CheckTurn(1,1);

            Assert.True(actual.current_turn);
        }

        [Theory]
        [InlineData("{'winner':null}",null)]
        [InlineData("{'winner':1}",1)]
        [InlineData("{'winner':2}",2)]
        public async Task CheckTurnReturnsWinner(string body, int? result)
        {
            var playerStateResponse = new HttpResponseMessage(HttpStatusCode.OK);
            playerStateResponse.Content = new StringContent(body);
            mockClient.Setup(c => c.Get("/v1/brisk/game/1/player/1?check_turn=true")).ReturnsAsync(playerStateResponse);

            Turn actual = await access.CheckTurn(1,1);

            Assert.Equal<int?>(result, actual.winner);
        }

        [Fact]
        public async Task EndTurnSubmitsSuccessfully()
        {
            HttpResponseMessage okResponse = new HttpResponseMessage(HttpStatusCode.OK);
            mockClient.Setup(c => c.Post("/v1/brisk/game/1/player/1", It.IsAny<HttpContent>())).ReturnsAsync(okResponse);

            HttpStatusCode actual = await access.EndTurn(1,1,"");

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, actual);
        }
    }
}

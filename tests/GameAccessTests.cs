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
        public async Task PostCallsThatReturnsNonSuccessfullyThrowExceptions()
        {
            HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(badResponse);  

            await Assert.ThrowsAsync<Exception>(() => access.CreateGame("Test Tanks"));
        }

        [Fact]
        public async Task GetCallsThatReturnsNonSuccessfullyThrowExceptions()
        {
            HttpResponseMessage badResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mockClient.Setup(c => c.Get("/v1/brisk/game/0/player/0")).ReturnsAsync(badResponse);

            await Assert.ThrowsAsync<Exception>(() => access.GetPlayerState());
        }

        [Fact]
        public async Task CreateGameReturnsGameId()
        {   
            HttpResponseMessage createdResponse = new HttpResponseMessage(HttpStatusCode.OK);
            createdResponse.Content = new StringContent("{\"game\":1}");    
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(createdResponse);  

            GameResult actual = await access.CreateGame("Test Tanks");

            Assert.Equal<int>(1, actual.game);
        }
        
        [Fact]
        public async Task CreateGameReturnsPlayerId()
        {   
            HttpResponseMessage createdResponse = new HttpResponseMessage(HttpStatusCode.OK);
            createdResponse.Content = new StringContent("{\"player\":1}");    
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(createdResponse);  

            GameResult actual = await access.CreateGame("Test Tanks");

            Assert.Equal<int>(1, actual.player);
        }
        
        [Fact]
        public async Task CreateGameReturnsToken()
        {   
            HttpResponseMessage createdResponse = new HttpResponseMessage(HttpStatusCode.OK);
            createdResponse.Content = new StringContent("{\"token\":\"sometokenvalue\"}");    
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(createdResponse);  

            GameResult actual = await access.CreateGame("Test Tanks");

            Assert.Equal("sometokenvalue", actual.token);
        }

        [Fact]
        public async Task StaticCreateSavesGameResultsInternally()
        {
            HttpResponseMessage createdResponse = new HttpResponseMessage(HttpStatusCode.OK);
            createdResponse.Content = new StringContent("{\"game\":1, \"player\":2, \"token\":\"sometokenvalue\"}");    
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(createdResponse);  

            IGameAccess actual = await GameAccess.Create(mockClient.Object, "Test Tanks");

            Assert.Equal<int>(1, actual.GameId);
            Assert.Equal("sometokenvalue", actual.Token);
            Assert.Equal<int>(2, actual.CurrentPlayer);
        }

        [Fact]
        public async Task GetPlayerStateReturnsCurrentTurn()
        {
            var playerStateResponse = new HttpResponseMessage(HttpStatusCode.OK);
            playerStateResponse.Content = new StringContent("{\"current_turn\":true}");
            mockClient.Setup(c => c.Get("/v1/brisk/game/0/player/0")).ReturnsAsync(playerStateResponse);

            PlayerState actual = await access.GetPlayerState();

            Assert.True(actual.current_turn);
        }

        [Theory]
        [InlineData("{\"winner\":null}",null)]
        [InlineData("{\"winner\":1}",1)]
        [InlineData("{\"winner\":2}",2)]
        public async Task GetPlayerStateReturnsWinner(string body, int? result)
        {
            var playerStateResponse = new HttpResponseMessage(HttpStatusCode.OK);
            playerStateResponse.Content = new StringContent(body);
            mockClient.Setup(c => c.Get("/v1/brisk/game/0/player/0")).ReturnsAsync(playerStateResponse);

            PlayerState actual = await access.GetPlayerState();

            Assert.Equal<int?>(result, actual.winner);
        }

        [Fact]
        public async Task GetPlayerStateReturnsTerritories()
        {
            string addTerritory(int id, string name, int armies)
            {
                return $"{{\"territory\":{id}, \"territory_name\":\"{name}\", \"num_armies\":{armies}}}";
            }

            var playerStateResponse = new HttpResponseMessage(HttpStatusCode.OK);
            playerStateResponse.Content = new StringContent($"{{\"territories\":[{addTerritory(1,"alpha centauri", 10)},{addTerritory(2,"rigel", 5)}]}}");

            mockClient.Setup(c => c.Get("/v1/brisk/game/0/player/0")).ReturnsAsync(playerStateResponse);

            PlayerState actual = await access.GetPlayerState();

            Assert.Equal<int>(2, actual.territories.Length);
            Assert.Equal<int>(1, actual.territories[0].territory);
            Assert.Equal("alpha centauri", actual.territories[0].territory_name);
            Assert.Equal<int>(10, actual.territories[0].num_armies);
            Assert.Equal<int>(2, actual.territories[1].territory);
            Assert.Equal("rigel", actual.territories[1].territory_name);
            Assert.Equal<int>(5, actual.territories[1].num_armies);
        }

        [Fact]
        public async Task PlaceArmiesSubmitsSuccessfully()
        {
            HttpResponseMessage okResponse = new HttpResponseMessage(HttpStatusCode.OK);
            mockClient.Setup(c => c.Post("/v1/brisk/game/0/player/0/territory/1", It.IsAny<HttpContent>())).ReturnsAsync(okResponse);

            bool actual = await access.PlaceArmies(1,2);

            Assert.True(actual);
        }

        [Fact]
        public async Task EndTurnSubmitsSuccessfully()
        {
            HttpResponseMessage okResponse = new HttpResponseMessage(HttpStatusCode.OK);
            mockClient.Setup(c => c.Post("/v1/brisk/game/0/player/0", It.IsAny<HttpContent>())).ReturnsAsync(okResponse);

            bool actual = await access.EndTurn();

            Assert.True(actual);
        }
    }
}

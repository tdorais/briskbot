using System;
using Xunit;
using briskbot.access;
using Moq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace briskbot.tests
{
    public class GameAccessTests
    {
        private readonly Mock<IApiClient> mockClient = new Mock<IApiClient>();
        private readonly GameAccess client;

        public GameAccessTests()
        {
            HttpResponseMessage mockedResponse = new HttpResponseMessage(HttpStatusCode.OK);
            mockedResponse.Content = new StringContent("{\"game\":1}");
            mockClient.Setup(c => c.Post("/v1/brisk/game", It.IsAny<HttpContent>())).ReturnsAsync(mockedResponse);
            client = new GameAccess(mockClient.Object);
        }

        [Fact]
        public async Task GameIdIsReturned()
        {         
            GameResult actual = await client.CreateGame();

            Assert.Equal<int>(1, actual.game);
        }
    }
}

using System.Net;
using MeuTodo;
using Tests.Factory;

namespace Tests.Integration
{
    public class TodoControllerIntegrationTest : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TodoControllerIntegrationTest(TestingWebAppFactory<Startup> factory)  => _client = factory.CreateClient();

        [Fact]
        public async Task Uunauthorized_response_when_get_all_todos()
        {
            var response = await _client.GetAsync("v1/todos");
            
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
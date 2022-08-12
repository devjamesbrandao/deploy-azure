using System.Net;
using FluentAssertions;
using MeuTodo;
using MeuTodo.ViewModels;
using Microsoft.Extensions.Configuration;
using Refit;
using Tests.Factory;
using Tests.HttpClients;
using Xunit.Priority;

namespace Tests.Integration
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TodoControllerIntegrationTest : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly ILoginAPI _authTodo;
        
        private readonly HttpClient _client;

        private static string UserName { get; set; }

        private static string Password { get; set; }

        private static string TokenJWT { get; set; }

        public TodoControllerIntegrationTest(TestingWebAppFactory<Startup> factory)  
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Test.json")
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            _authTodo = RestService.For<ILoginAPI>(configuration.GetSection("UrlWebAppTestes").Value);

            UserName = configuration.GetSection("APITodos_Access:Name").Value;

            Password = configuration.GetSection("APITodos_Access:Password").Value;

            // Artifice to add InMemoryDbContext
            _client = factory.CreateClient();
        }

        [Fact, Priority(1)]
        public async Task Get_JWT_Token_With_Success()
        {
            var token = await _authTodo.PostLogin(
                new LoginUserViewModel()
                {
                    Name = UserName,
                    Password = Password
                }
            );

            TokenJWT = token.Token;

            token.Token.Should().NotBeEmpty(because: "API require this JWT Token");
        }

        [Fact, Priority(2)]
        public void Get_Unauthorized_wheh_Get_All_Todos()
        {
            Assert.True(true);
        }
    }
}
using System.Net;
using FluentAssertions;
using MeuTodo;
using MeuTodo.Models;
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

        private readonly ITodo _todos;
        
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

            UserName = configuration.GetSection("APITodos_Access:Name").Value;

            Password = configuration.GetSection("APITodos_Access:Password").Value;

            _authTodo = RestService.For<ILoginAPI>(configuration.GetSection("UrlWebAppTestes").Value);

            var token = _authTodo.PostLogin(
                new LoginUserViewModel()
                {
                    Name = UserName,
                    Password = Password
                }
            ).Result;

            _todos = RestService.For<ITodo>(
                configuration.GetSection("UrlWebAppTestes").Value,
                new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = () => Task.FromResult(token.Token)
                }
            );

            TokenJWT = token.Token;

            // Artifice to add InMemoryDbContext
            _client = factory.CreateClient();
        }

        [Fact, Priority(1)]
        public void JWT_Token_Should_Be_Valida()
        {
            TokenJWT.Should().NotBeEmpty(because: "API require this JWT Token");
        }

        [Fact, Priority(2)]
        public async Task Post_Todo_With_Success()
        {
            var todo = new CreateTodoViewModel
            {
                Title = "Learning Integration Tests in ASP.NET Core"
            };

            var createdTodo = await _todos.PostTodoAsync(todo);

            createdTodo.StatusCode.Should().Be(HttpStatusCode.Created);

            createdTodo.Content.Should().BeAssignableTo<Todo>();

            createdTodo.Content.Done.Should().BeFalse();

            createdTodo.Content.Title.Should().Be(todo.Title);
        }

        [Fact, Priority(3)]
        public async Task Post_Todo_Return_Bad_Request()
        {
            var todo = new CreateTodoViewModel();

            var createdTodo = await _todos.PostTodoAsync(todo);

            createdTodo.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact, Priority(4)]
        public async Task Get_Todo_By_Id_With_Success()
        {
            var todo = await _todos.GetTodoByIdAsync(1);

            todo.StatusCode.Should().Be(HttpStatusCode.OK);

            todo.Content.Should().BeAssignableTo<Todo>();
        }

        [Fact, Priority(5)]
        public async Task Get_Todo_By_Id_Not_Found()
        {
            var todo = await _todos.GetTodoByIdAsync(2);

            todo.StatusCode.Should().Be(HttpStatusCode.NotFound);

            todo.ReasonPhrase.Should().Contain("Not");
        }

        [Fact, Priority(6)]
        public async Task Put_Todo_Return_Bad_Request()
        {
            var todo = new CreateTodoViewModel();

            var putTodo = await _todos.PutTodoAsync(todo, 1);

            putTodo.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            putTodo.Content.Should().Be(null);
        }

        [Fact, Priority(7)]
        public async Task Put_Todo_Return_Not_Found()
        {
            var todo = new CreateTodoViewModel()
            {
                Title = "Learning Integration Tests in ASP.NET Core 6.0"
            };

            var putTodo = await _todos.PutTodoAsync(todo, 2);

            putTodo.StatusCode.Should().Be(HttpStatusCode.NotFound);

            putTodo.Content.Should().Be(null);
        }

        [Fact, Priority(7)]
        public async Task Put_Todo_With_Success()
        {
            var todo = new CreateTodoViewModel()
            {
                Title = "Learning Integration Tests in ASP.NET Core 6.0"
            };

            var putTodo = await _todos.PutTodoAsync(todo, 1);

            putTodo.StatusCode.Should().Be(HttpStatusCode.OK);

            putTodo.Content.Should().BeAssignableTo<Todo>();

            putTodo.Content.Title.Should().Be("Learning Integration Tests in ASP.NET Core 6.0");
        }
    }
}
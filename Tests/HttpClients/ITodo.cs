using MeuTodo.Models;
using MeuTodo.ViewModels;
using Refit;

namespace Tests.HttpClients
{
    public interface ITodo
    {
        [Get("/api/todo/todos")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<List<Todo>>> GetAllTodosAsync();

        [Get("/api/todo/todos/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<Todo>> GetTodoByIdAsync(int id);

        [Post("/api/todo/todos")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<Todo>> PostTodoAsync([Body] CreateTodoViewModel todo);

        [Put("/api/todo/todos/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<Todo>> PutTodoAsync([Body] CreateTodoViewModel todo, int id);

        [Delete("/api/todo/todos/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteTodoAsync([Body] CreateTodoViewModel todo, int id);
    }
}
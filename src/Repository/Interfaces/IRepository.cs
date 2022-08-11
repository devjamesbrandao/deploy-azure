using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Models;

namespace MeuTodo.Repositorio.Interfaces
{
    public interface IRepository
    {
        Task<User> GetUserAsync(string name, string password);

        Task<List<Todo>> GetAllTodosAsync();

        Task<Todo> GetTodoByIdAsync(int id);

        Task PostTodoAsync(Todo todo);

        Task UpdateTodoAsync(Todo todo);

        Task DeleteTodoAsync(Todo todo);
    }
}
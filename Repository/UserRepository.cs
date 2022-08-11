using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Repository
{
    public class UserRepository : IRepository
    {
        private readonly AppDataContext _context;

        public UserRepository(AppDataContext context) => _context = context;

        public async Task<User> GetUserAsync(string name, string password) 
            => await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name && x.Password == password);

        public async Task<List<Todo>> GetAllTodosAsync() => await _context.Todos.AsNoTracking().ToListAsync();

        public async Task<Todo> GetTodoByIdAsync(int id) => await _context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task PostTodoAsync(Todo todo)
        {
            await _context.Todos.AddAsync(todo);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            _context.Todos.Update(todo);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodoAsync(Todo todo)
        {
            _context.Todos.Remove(todo);

            await _context.SaveChangesAsync();
        }
    }
}
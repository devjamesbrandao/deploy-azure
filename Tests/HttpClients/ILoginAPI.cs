using MeuTodo.ViewModels;
using Refit;
using src.Models;

namespace Tests.HttpClients
{
    public interface ILoginAPI
    {
        [Post("/api/Auth/login")]
        Task<TokenUser> PostLogin([Body] LoginUserViewModel login);
    }
}
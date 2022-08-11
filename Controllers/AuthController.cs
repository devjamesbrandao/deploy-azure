using System.Threading.Tasks;
using MeuTodo.Configuration;
using MeuTodo.Repositorio.Interfaces;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository _repository;

        public AuthController(IRepository repository) => _repository = repository;


        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserViewModel login)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _repository.GetUserAsync(login.Name, login.Password);

            if(user is null) return NotFound("User not found");

            return Ok(new { UserName = user.Name, Token = JwtToken.GenerateToken(user) });
        }
    }
}
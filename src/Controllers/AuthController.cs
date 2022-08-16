using System.Threading.Tasks;
using MeuTodo.Configuration;
using MeuTodo.Repositorio.Interfaces;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository _repository;

        public AuthController(IRepository repository) => _repository = repository;

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="login"></param>
        /// <remarks>
        /// Requisition example:
        /// 
        ///     [POST] api/auth/login
        ///     {        
        ///       "name": "Naruto",
        ///       "Password": "Lamem"        
        ///     }
        /// or
        ///     [POST] api/auth/login
        ///     {
        ///         "name": "Sakura",
        ///         "Password": "Sasuke" 
        ///     }    
        /// </remarks>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TokenUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenUser>> LoginAsync([FromBody] LoginUserViewModel login)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _repository.GetUserAsync(login.Name, login.Password);

            if(user is null) return NotFound("User not found");

            return Ok(new TokenUser(){ UserName = user.Name, Token = JwtToken.GenerateToken(user) });
        }
    }
}
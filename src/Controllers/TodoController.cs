using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Models;
using MeuTodo.Repositorio.Interfaces;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly IRepository _repository;

        public TodoController(IRepository repository) => _repository = repository;

        /// <summary>
        /// Get all todos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("todos")]
        public async Task<ActionResult<List<Todo>>> GetAllTodosAsync() => await _repository.GetAllTodosAsync();


        /// <summary>
        /// Get todo by id
        /// </summary>
        /// <param name="id">Todo id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("todos/{id}")]
        public async Task<ActionResult<Todo>> GetByIdAsync([FromRoute] int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            return todo == null? NotFound("Todo not found"): Ok(todo);
        }
        

        /// <summary>
        /// Create todo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("todos")]
        public async Task<IActionResult> PostAsync([FromBody] CreateTodoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var todo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };

            await _repository.PostTodoAsync(todo);

            return Created($"v1/todos/{todo.Id}", todo);
        }
        
        
        /// <summary>
        /// Update todo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id">Todo id</param>
        /// <returns></returns>
        [HttpPut("todos/{id}")]
        public async Task<ActionResult<Todo>> PutAsync([FromBody] CreateTodoViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var todo = await _repository.GetTodoByIdAsync(id);

            if(todo == null) return NotFound("Todo not found");

            todo.UpdateTitle(model.Title);
            
            await _repository.UpdateTodoAsync(todo);

            return Ok(todo);
        }


        /// <summary>
        /// Delete todo
        /// </summary>
        /// <param name="id">Todo id</param>
        /// <returns></returns>
        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            if(todo == null) return NotFound("Todo not found");

            await _repository.DeleteTodoAsync(todo);

            return Ok();
        }  
    }
}
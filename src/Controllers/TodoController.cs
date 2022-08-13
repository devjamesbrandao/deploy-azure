using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Models;
using MeuTodo.Repositorio.Interfaces;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// <remarks>
        /// Requisition example:
        /// 
        ///     [GET] api/todo/todos
        /// </remarks>
        [HttpGet("todos")]
        [ProducesResponseType(typeof(List<Todo>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Todo>>> GetAllTodosAsync() => await _repository.GetAllTodosAsync();


        /// <summary>
        /// Get todo by id
        /// </summary>
        /// <param name="id">Todo id</param>
        /// <remarks>
        /// Requisition example:
        /// 
        ///     [GET] api/todo/todos/1
        /// </remarks>
        [HttpGet("todos/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Todo>> GetByIdAsync([FromRoute] int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            return todo == null? NotFound("Todo not found"): Ok(todo);
        }
        

        /// <summary>
        /// Create todo
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Requisition example:
        /// 
        ///     [POST] api/todo/todos
        ///     {        
        ///       "title": "Learning Azure DevOps"     
        ///     }
        /// </remarks>
        [HttpPost("todos")]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <remarks>
        /// Requisition example:
        /// 
        ///     [PUT] api/todo/todos/1
        ///     {        
        ///       "title": "Learning Azure DevOps and Github Actions"     
        ///     }
        /// </remarks>
        [HttpPut("todos/{id}")]
        [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <remarks>
        /// Requisition example:
        /// 
        ///     [DELETE] api/todo/todos/1
        /// </remarks>
        [HttpDelete("todos/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            if(todo == null) return NotFound("Todo not found");

            await _repository.DeleteTodoAsync(todo);

            return Ok();
        }  
    }
}
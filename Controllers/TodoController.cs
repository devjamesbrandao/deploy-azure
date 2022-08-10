using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class TodoController : ControllerBase
    {
        private readonly AppDataContext _context;

        public TodoController(AppDataContext context) => _context = context;

        /// <summary>
        /// Obter todas as tarefas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("todos")]
        public async Task<ActionResult<List<Todo>>> GetAllTodosAsync() => await _context.Todos.AsNoTracking().ToListAsync();


        /// <summary>
        /// Obter tarefa por id
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <returns></returns>
        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var todo = await _context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return todo == null? NotFound(): Ok(todo);
        }
        

        /// <summary>
        /// Rotina para criar tarefa
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

            try
            {
                await _context.Todos.AddAsync(todo);

                await _context.SaveChangesAsync();

                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (Exception) { return BadRequest(); }
        }
        
        /// <summary>
        /// Rotina para atualizar tarefa
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id">Id da tarefa</param>
        /// <returns></returns>
        [HttpPut("todos/{id}")]
        public async Task<IActionResult> PutAsync([FromBody] CreateTodoViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null) return NotFound();

            try
            {
                todo.Title = model.Title;
                
                _context.Todos.Update(todo);

                await _context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception){ return BadRequest(); }
        }

        /// <summary>
        /// Rotina para deletar tarefa
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <returns></returns>
        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.Todos.Remove(todo);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception){ return BadRequest(); }
        }
        
    }
}
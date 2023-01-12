using database.Models;
using database.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Immutable;

namespace database.Controllers
{
    [ApiController]
    [Route("/")]
    public class TodoController : Controller
    {
        private readonly TodosService todosService;

        public TodoController(TodosService todosService) { this.todosService = todosService; }

        [HttpGet]
        public async Task<List<Todo>> Get() => await todosService.GetAsync();

        [HttpPost]
        public async Task<IActionResult> Post(Todo newTodo)
        {
            //should probably bound amount of todos

            
            newTodo.Id = ObjectId.GenerateNewId().ToString();
            await todosService.CreateAsync(newTodo);
            return CreatedAtAction(nameof(Get), new {id = newTodo.Id}, newTodo);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Todo updatedTodo)
        {
            //check if the todo actually exists
            var todo = await todosService.GetAsync(id);

            if(todo is null)
            {
                return NotFound();
            }

            updatedTodo.Id = todo.Id;

            await todosService.UpdateAsync(id, updatedTodo);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = await todosService.GetAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            await todosService.RemoveAsync(id);

            return NoContent();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly TodoContext _context;

        public ToDoController (TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new Todoitem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        
        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todoitem>>>  GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todoitem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
    
        [HttpPost]
        public async Task<ActionResult<Todoitem>> PostTodoItem(Todoitem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }


        [HttpPut ("{id}")]
        public async Task<ActionResult<Todoitem>> PutTodoItem(long id, Todoitem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
       // POST api/<controller>
       [HttpPost]
       public void Post([FromBody]string value)
       {
       }


       // PUT api/<controller>/5
       [HttpPut("{id}")]
       public void Put(int id, [FromBody]string value)
       {
       }

       // DELETE api/<controller>/5
       [HttpDelete("{id}")]
       public void Delete(int id)
       {
       } */
    }
}

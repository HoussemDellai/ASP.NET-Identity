using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AspNetIdentity.WebApi.Models;
using AspNetIdentity.WebApi.Models.Entities;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Controllers
{
    [RoutePrefix("api/todoes")]
    public class TodoesController : BaseApiController //ApiController
    {
        private ToDoContext db = new ToDoContext();

        // GET: api/Todoes
        [Route("all")]
        [Authorize(Roles = "User")]
        public IQueryable<Todo> GetTodoes()
        {
            return db.Todoes;
        }

        // GET: api/Todoes/5
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> GetTodo(int id)
        {
            Todo todo = await db.Todoes.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // PUT: api/Todoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTodo(int id, Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todo.Id)
            {
                return BadRequest();
            }

            db.Entry(todo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Todoes
        [ResponseType(typeof(Todo))]
        [Authorize(Roles = "User")]
        [Route("Create")]
        [HttpPost]
        public async Task<IHttpActionResult> PostTodo(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            todo.CreatedAt = DateTime.UtcNow;
            todo.UpdatedAt = DateTime.UtcNow;

            var userId = User.Identity.GetUserId();

            var user = AppUserManager.Users.Include(u => u.Todos).FirstOrDefault(u => u.Id == userId);
            
            if (user.Todos == null)
                user.Todos = new List<Todo>();

            user.Todos.Add(todo);
            //todo.User = user;

            await AppUserManager.UpdateAsync(user);
            //db.Entry(todo).State = EntityState.Modified;
            //db.Todoes.Add(todo);
            //await db.SaveChangesAsync();

            return Created("api/todoes/" + todo.Id, todo.Id);
            //return CreatedAtRoute("DefaultApi", new { id = todo.Id }, todo);
        }

        // DELETE: api/Todoes/5
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> DeleteTodo(int id)
        {
            Todo todo = await db.Todoes.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            db.Todoes.Remove(todo);
            await db.SaveChangesAsync();

            return Ok(todo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoExists(int id)
        {
            return db.Todoes.Count(e => e.Id == id) > 0;
        }
    }
}
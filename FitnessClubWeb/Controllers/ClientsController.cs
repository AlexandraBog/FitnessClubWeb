using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessClubWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly FitnessClubContext context;

        public ClientsController(FitnessClubContext context)
        {
            this.context = context;
            context.Visitings.Load();
        }


        // GET: api/Clients
        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return this.context.Clients.Include(c => c.Subscription).Include(c => c.Visitings);
        }

        // GET: api/Clients/5
        [HttpGet("{id}", Name = "GetClient")]
        public async Task<IActionResult> GetClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = await context.Clients.Include(c => c.Subscription).Include(c => c.Visitings).SingleOrDefaultAsync(c => c.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        // POST: api/Clients
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return CreatedAtAction("GetClient", new { id = client.ID }, client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = context.Clients.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            item.FIO = client.FIO;
            item.BirthDay = client.BirthDay;
            context.Clients.Update(item);
            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = context.Clients.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            context.Clients.Remove(item);
            await context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/clients/5/mark
        [HttpPut("{id}/mark")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MarkVisiting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            var lastVisiting = client.Visitings.LastOrDefault();
            if (lastVisiting != null && lastVisiting.FinishTime == null)
            {
                client.Visitings.LastOrDefault().FinishTime = client.Visitings.LastOrDefault().StartTime.Date.AddHours(23);
            }

            Visiting visiting = new Visiting
            {
                StartTime = DateTime.Now,
                FinishTime = null
            };


            client.Visitings.Add(visiting);

            context.Update(client);
            try
            {
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return Ok();
        }

        // PUT: api/clients/5/unmark
        [HttpPut("{id}/unmark")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> MarkExit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            var lastVisiting = client.Visitings.LastOrDefault();
            if (lastVisiting == null)
            {
                return NotFound();
            }
            client.Visitings.LastOrDefault().FinishTime = DateTime.Now;

            context.Update(client);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}

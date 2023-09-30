using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank_API.Context;
using MiniBank_API.Models;

namespace MiniBank_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly MiniBankContext _context;

        public ClientController(MiniBankContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return _context.Clients.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetById(int id)
        {
            var client = _context.Clients.FirstOrDefault(i => i.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return client;
        }

        [HttpPost]
        public IActionResult Post(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var existingClient = _context.Clients.FirstOrDefault(i=>i.Id== id);
            if (existingClient == null)
            {
                return NotFound();
            }
            existingClient.PhoneNumber = client.PhoneNumber  ;
            existingClient.Email = client.Email;
            existingClient.Name = client.Name;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var clientToDelete = _context.Clients.FirstOrDefault(i => i.Id == id);
            if (clientToDelete == null)
            {
                return NotFound();
            }
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}

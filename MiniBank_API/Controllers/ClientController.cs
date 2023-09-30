using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank_API.Models;
using MiniBank_API.Service;

namespace MiniBank_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _service;

        public ClientController(ClientService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetById(int id)
        {
            var client = _service.GetById(id);
            if (client == null)
            {
                return NotFound();
            }
            return client;
        }

        [HttpPost]
        public IActionResult Post(Client client)
        {
            var newClient = _service.Create(client);

            return CreatedAtAction(nameof(GetById), new { id = newClient.Id }, newClient );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var existingClient = _service.GetById(id);
            if (existingClient == null)
            {
                return NotFound();
            }
            _service.Update(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var clientToDelete = _service.GetById(id);
            if (clientToDelete == null)
            {
                return NotFound();
            }
            _service.Delete(id);

            return Ok();
        }
    }
}

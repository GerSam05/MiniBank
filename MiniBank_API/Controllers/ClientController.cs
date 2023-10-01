using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank_API.Models;
using MiniBank_API.Service;
using System.Net;

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _service.GetAll();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _service.GetById(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> PostClient(Client client)
        {
            var newClient = await _service.Create(client);

            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient );
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var existingClient = await _service.GetById(id);
            if (existingClient == null)
            {
                return NotFound();
            }
            await _service.Update(client);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var clientToDelete = await _service.GetById(id);
            if (clientToDelete == null)
            {
                return NotFound();
            }
            await _service.Delete(id);

            return Ok();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PartialPutClient(int id, JsonPatchDocument<Client> jsonPatch)
        {
            var client = await _service.GetById(id);
            if (client == null)
            {
                return NotFound();
            }
            if (jsonPatch == null || jsonPatch.Operations.Count == 0 || jsonPatch.Operations.Any(o => o.op != "replace"))
            {
                return BadRequest();
            }

            jsonPatch.ApplyTo(client);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.Update(client);

            return NoContent();
        }
    }
}

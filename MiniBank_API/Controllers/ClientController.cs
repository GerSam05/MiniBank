using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank_API.Models;
using MiniBank_API.Models.Dtos;
using MiniBank_API.Service;
using System.Net;

namespace MiniBank_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _service;
        private readonly IMapper _mapper;

        public ClientController(ClientService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IEnumerable<ClientDto>> GetClients()
        {
            return await _service.GetAll();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            if (id == 0) return BadRequest(new {message = "Campo Id debe ser mayor que cero (0)"});

            var client = await _service.GetClientDtoById(id);
            if (client == null)
            {
                return NotFound(new { message = $"El Client con Id={id} no existe en la base de datos"});
            }
            return Ok(client);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> PostClient(ClientCreateDto newClientDto)
        {
            if (!ModelState.IsValid)
            {
                //_logger.LogError("ModelState Inválido");
                return BadRequest(ModelState);
            }
            var newClient = await _service.Create(newClientDto);

            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient );
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientUpdateDto clientUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                //_logger.LogError("ModelState Inválido");
                return BadRequest(ModelState);
            }
            if (id != clientUpdateDto.Id)
            {
                return BadRequest(new { message = $"El Id={id} de la URL no coincide con el Id={clientUpdateDto.Id} del cuerpo de la solicitud"});
            }

            var existingClient = await _service.GetById(id);
            if (existingClient != null)
            {
                await _service.Update(clientUpdateDto);
                return NoContent();
            }
            else return NotFound(new { message = $"El Client con Id={id} no existe en la base de datos" });

        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (id == 0) return BadRequest(new { message = "Campo Id debe ser mayor que cero (0)" });

            var clientToDelete = await _service.GetById(id);
            if (clientToDelete != null)
            {
                await _service.Delete(id);

                return NoContent();
            }
            else return NotFound(new { message = $"El Client con Id={id} no existe en la base de datos" });

        }
        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialPutClient(int id, JsonPatchDocument<ClientUpdateDto> jsonPatch)
        {
            if (id == 0) return BadRequest(new { message = "Campo Id debe ser mayor que cero (0)" });

            var client = await _service.GetById(id);
            if (client == null)
            {
                return NotFound(new { message = $"El Client con Id={id} no existe en la base de datos" });
            }

            if (jsonPatch == null || jsonPatch.Operations.Count == 0 ||
                jsonPatch.Operations.Any(o => o.op != "replace"))
            {
                return BadRequest(new { message = $"Json nulo o inválido" });
            }

            ClientUpdateDto clientToUpdateDto = _mapper.Map<ClientUpdateDto>(client);

            jsonPatch.ApplyTo(clientToUpdateDto);

            if (TryValidateModel(clientToUpdateDto))
            {
                await _service.Update(clientToUpdateDto);
                return NoContent();
            }
            else return BadRequest(ModelState);
        }
    }
}

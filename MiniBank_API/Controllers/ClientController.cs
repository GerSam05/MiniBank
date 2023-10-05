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
        private readonly ILogger<ClientController> _logger;
        private readonly ClientService _service;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ClientController(ILogger<ClientController> logger, ClientService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetClients()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de Clientes");
                List<ClientDto> listClients = await _service.GetAll();
                _response.Resultado = listClients;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetClient(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error en peticion de Cliente con Id=0");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = _response.ErrorIdCero();
                    return BadRequest(_response);
                }

                var client = await _service.GetClientDtoById(id);
                if (client == null)
                {
                    _logger.LogError("Id de Client no encontrado");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorNotFound(id);
                    return NotFound(_response);
                }

                _response.Resultado = client;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.Message.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> PostClient(ClientCreateDto newClientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(ModelState);
                }
                var newClient = await _service.Create(newClientDto);
                _response.Resultado = newClient;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.Message.ToString() };
            }
            return _response;

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientUpdateDto clientUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(ModelState);
                }

                if (id != clientUpdateDto.Id)
                {
                    _logger.LogError("Error en peticion de Id");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorDifId(id, clientUpdateDto.Id);
                    return BadRequest(_response);
                }

                var existingClient = await _service.GetById(id);
                if (existingClient == null)
                {
                    _logger.LogError("Id de Client No encontrado");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorNotFound(id);
                    return NotFound(_response);
                }
                await _service.Update(clientUpdateDto);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Editado();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.Message.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error en peticion de Client con Id=0");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorIdCero();
                    return BadRequest(_response);
                }

                var clientToDelete = await _service.GetById(id);
                if (clientToDelete == null)
                {
                    _logger.LogError("Id de Client No encontrado");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorNotFound(id);
                    return NotFound(_response);
                }

                await _service.Delete(id);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Eliminado();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.Message.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PartialPutClient(int id, JsonPatchDocument<ClientUpdateDto> jsonPatch)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error en peticion de Client con Id=0");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorIdCero();
                    return BadRequest(_response);
                }

                var client = await _service.GetById(id);
                if (client == null)
                {
                    _logger.LogError("Id de Cliente No encontrado");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorNotFound(id);
                    return NotFound(_response);
                }

                if (jsonPatch == null || jsonPatch.Operations.Count == 0 ||
                    jsonPatch.Operations.Any(o => o.op != "replace"))
                {
                    _logger.LogError("Error al Actualizar, Json Nulo");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorJson();
                    return BadRequest(_response);
                }

                ClientUpdateDto clientToUpdateDto = _mapper.Map<ClientUpdateDto>(client);

                jsonPatch.ApplyTo(clientToUpdateDto);

                if (TryValidateModel(clientToUpdateDto))
                {
                    await _service.Update(clientToUpdateDto);
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.Editado();
                    return Ok(_response);
                }
                else
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.Message.ToString() };
            }
            return BadRequest(_response);
        }
    }
}

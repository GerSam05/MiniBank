using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MiniBank_API.Models;
using MiniBank_API.Models.Dtos;
using MiniBank_API.Service;
using System.Net;
using System.Security.Principal;

namespace MiniBank_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AccountService _service;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public AccountController(AccountService service, IMapper mapper, ILogger<AccountController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAccounts()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de Cuentas");
                List<AccountDto> accountList = await _service.GetAll();
                _response.Resultado = accountList;
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
        public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error en peticion de Cuenta con Id=0");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorIdCero();
                    return BadRequest(_response);
                }

                var account = await _service.GetAccountDtoById(id);
                if (account == null)
                {
                    _logger.LogError("Id de Cuenta no encontrado");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorNotFound(id);
                    return NotFound(_response);
                }
                _response.Resultado = account;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> PostAccount(AccountCreateDto accountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(ModelState);
                }

                string validationResult = await _service.ValidationAccount(accountDto);
                if (!validationResult.Equals("valid"))
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(new { message = validationResult });
                }

                var newAccount = await _service.Create(accountDto);
                _response.Resultado = newAccount;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ExceptionMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountUpdateDto accountoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(ModelState);
                }

                if (id != accountoUpdateDto.Id)
                {
                    _logger.LogError("Error en peticion de Id");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorDifId(id, accountoUpdateDto.Id);
                    return BadRequest(_response);
                }

                var existingAccount = await _service.GetById(id);
                if (existingAccount == null)
                {
                    _logger.LogError("Id de Cuenta No encontrado");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorNotFound(id);
                    return NotFound(_response);
                }

                string validationResult = await _service.ValidationAccount(accountoUpdateDto);
                if (!validationResult.Equals("valid"))
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(new { message = validationResult });
                }

                await _service.Update(accountoUpdateDto);
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
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error en peticion de Cuenta con Id=0");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorIdCero();
                    return BadRequest(_response);
                }

                var clientToDelete = await _service.GetById(id);
                if (clientToDelete == null)
                {
                    _logger.LogError("Id de Cuenta No encontrado");
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PartialUpdateAccount(int id, JsonPatchDocument<AccountUpdateDto> jsonPatch)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error en peticion de Cuenta con Id=0");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorIdCero();
                    return BadRequest(_response);
                }

                var account = await _service.GetById(id);
                if (account == null)
                {
                    _logger.LogError("Id de Cuenta No encontrado");
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

                AccountUpdateDto accountToUpdateDto = _mapper.Map<AccountUpdateDto>(account);

                jsonPatch.ApplyTo(accountToUpdateDto);

                if (!TryValidateModel(accountToUpdateDto))
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(ModelState);
                }

                string validationResult = await _service.ValidationAccount(accountToUpdateDto);

                if (!validationResult.Equals("valid"))
                {
                    _logger.LogError("ModelState Inválido");
                    return BadRequest(new { message = validationResult });
                }

                await _service.Update(accountToUpdateDto);
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
    }
}

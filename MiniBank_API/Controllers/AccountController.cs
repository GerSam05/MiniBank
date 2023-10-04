using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MiniBank_API.Models;
using MiniBank_API.Models.Dtos;
using MiniBank_API.Service;
using System.Security.Principal;

namespace MiniBank_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;
        private readonly IMapper _mapper;

        public AccountController(AccountService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            return await _service.GetAll();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            if (id == 0) return BadRequest(new { message = "Campo Id debe ser mayor que cero (0)" });

            var account = await _service.GetAccountDtoById(id);
            if (account == null)
            {
                return NotFound(new { message = $"La Cuenta con Id={id} no existe en la base de datos" });
            }
            return Ok(account);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostAccount(AccountCreateDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                //_logger.LogError("ModelState Inválido");
                return BadRequest(ModelState);
            }

            string validationResult = await _service.ValidationAccount(accountDto);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new {message = validationResult});
            }
            var newAccount = await _service.Create(accountDto);
            return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody]AccountUpdateDto accountoUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                //_logger.LogError("ModelState Inválido");
                return BadRequest(ModelState);
            }

            if (id != accountoUpdateDto.Id)
            {
                return BadRequest(new { message = $"El Id={id} de la URL no coincide con el Id={accountoUpdateDto.Id} del cuerpo de la solicitud" });
            }

            var existingAccount = await _service.GetById(id);
            if (existingAccount == null)
            {
                return NotFound(new { message = $"La Cuenta con Id={id} no existe en la base de datos" });
            }

            string validationResult = await _service.ValidationAccount(accountoUpdateDto);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new { message = validationResult });
            }

            await _service.Update(accountoUpdateDto);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (id == 0) return BadRequest(new { message = "Campo Id debe ser mayor que cero (0)" });

            var accountToDelete = await _service.GetById(id);
            if (accountToDelete == null)
            {
                return NotFound(new { message = $"La Cuenta con Id={id} no existe en la base de datos" });
            }
            await _service.Delete(id);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateAccount(int id, JsonPatchDocument<AccountUpdateDto> jsonPatch)
        {
            if (id == 0) return BadRequest(new { message = "Campo Id debe ser mayor que cero (0)" });

            var account = await _service.GetById(id);
            if (account == null)
            {
                return NotFound(new { message = $"La Cuenta con Id={id} no existe en la base de datos" });
            }

            if (jsonPatch == null || jsonPatch.Operations.Count == 0 || jsonPatch.Operations.Any(o => o.op != "replace"))
            {
                return BadRequest(new { message = $"Json nulo o inválido" });
            }

            AccountUpdateDto accountToUpdateDto = _mapper.Map<AccountUpdateDto>(account);

            jsonPatch.ApplyTo(accountToUpdateDto);

            if (!TryValidateModel(accountToUpdateDto))
            {
                return BadRequest(ModelState);
            }

            string validationResult = await _service.ValidationAccount(accountToUpdateDto);

            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new { message = validationResult });
            }

            await _service.Update(accountToUpdateDto);
            return NoContent();
        }
    }
}

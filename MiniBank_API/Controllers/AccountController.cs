using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MiniBank_API.Models;
using MiniBank_API.Service;
using System.Security.Principal;

namespace MiniBank_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ICollection<Account>> GetAccounts()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            var account = await _service.GetById(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult> PostAccount(Account account)
        {
            string validationResult = await _service.ValidationAccount(account);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new {message = validationResult});
            }
            await _service.Create(account);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody]Account account)
        {
            var accountToUpdate = _service.GetById(id);
            if (accountToUpdate == null)
            {
                return NotFound();
            }
            string validationResult = await _service.ValidationAccount(account);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new { message = validationResult });
            }
            await _service.Update(account);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var accountToDelete = await _service.GetById(id);
            if (accountToDelete == null)
            {
                return NotFound();
            }
            await _service.Update(accountToDelete);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateAccount(int id, JsonPatchDocument<Account> jsonPatch)
        {
            var account = await _service.GetById(id);
            if (account == null)
            {
                return NotFound();
            }
            if (jsonPatch == null || jsonPatch.Operations.Count == 0 || jsonPatch.Operations.Any(o => o.op != "replace"))
            {
                return BadRequest();
            }

            jsonPatch.ApplyTo(account);

            string validationResult = await _service.ValidationAccount(account);
            if (!validationResult.Equals("valid"))
            {
                return BadRequest(new { message = validationResult });
            }

            await _service.Update(account);
            return NoContent();
        }
    }
}

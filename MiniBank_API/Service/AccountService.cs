using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank_API.Context;
using MiniBank_API.Models;
using MiniBank_API.Models.Dtos;

namespace MiniBank_API.Service
{
    public class AccountService
    {
        private readonly MiniBankContext _context;
        private readonly IMapper _mapper;

        public AccountService (MiniBankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAll()
        {
            var listAccount = await _context.Accounts.ToListAsync();
            List<AccountDto> listAccountDto = _mapper.Map<List<AccountDto>>(listAccount);
            return listAccountDto;
        }

        public async Task<AccountDto?> GetAccountDtoById(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            AccountDto accountDto = _mapper.Map<AccountDto>(account);
            return accountDto;
        }

        public async Task<Account?> GetById(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a=>a.Id == id);
        }

        public async Task<Account> Create(AccountCreateDto newAccountDto)
        {
            Account newAccount = _mapper.Map<Account>(newAccountDto);
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return newAccount;
        }

        public async Task Update(AccountUpdateDto accountDto)
        {
            var existingAccount = await GetById(accountDto.Id);
            if (existingAccount != null)
            {
                Account account = _mapper.Map<Account>(accountDto);
                account.RegDate = existingAccount.RegDate;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var accountToDelete = await GetById(id);
            if (accountToDelete != null)
            {
                _context.Accounts.Remove(accountToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> ValidationAccount(AccountCreateDto accountDto)
        {
            string result = "valid";

            var accountType = await _context.AccountTypes.FirstOrDefaultAsync(i => i.Id == accountDto.AccountType);
            if (accountType == null)
            {
                result = $"El tipo de cuente {accountDto.AccountType} no existe!";
            }
            var client = await _context.Clients.FirstOrDefaultAsync(i => i.Id == accountDto.ClientId);
            if (client == null)
            {
                result = $"El cliente con Id={accountDto.ClientId} no existe!";
            }
            return result;
        }

        public async Task<string> ValidationAccount(AccountUpdateDto accountDto)
        {
            string result = "valid";

            var accountType = await _context.AccountTypes.FirstOrDefaultAsync(i=>i.Id == accountDto.AccountType);
            if (accountType == null)
            {
                result = $"El tipo de cuente {accountDto.AccountType} no existe!";
            }
            var client = await _context.Clients.FirstOrDefaultAsync(i=>i.Id==accountDto.ClientId);
            if (client == null)
            {
                result = $"El cliente con Id={accountDto.ClientId} no existe!";
            }
            return result;
        }
    }
}

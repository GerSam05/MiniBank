using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBank_API.Context;
using MiniBank_API.Models;

namespace MiniBank_API.Service
{
    public class AccountService
    {
        private readonly MiniBankContext _context;

        public AccountService (MiniBankContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Account>> GetAll()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account?> GetById(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a=>a.Id == id);
        }

        public async Task<Account> Create(Account newAccount)
        {
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return newAccount;
        }

        public async Task Update(Account account)
        {
            var accauntToUpdate = await _context.Accounts.FirstOrDefaultAsync(i => i.Id == account.Id);
            if (accauntToUpdate != null)
            {
                accauntToUpdate.ClientId = account.ClientId;
                accauntToUpdate.AccountType = account.AccountType;
                accauntToUpdate.Balance = account.Balance;
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task Delete(int id)
        {
            var accountToDelete = await _context.Accounts.FirstOrDefaultAsync(i => i.Id == id);
            if (accountToDelete != null)
            {
                _context.Accounts.Remove(accountToDelete);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<string> ValidationAccount(Account account)
        {
            string result = "valid";

            var accountType = await _context.AccountTypes.FirstOrDefaultAsync(i=>i.Id == account.AccountType);
            if (accountType == null)
            {
                result = $"El tipo de cuente {account.AccountType} no existe!";
            }
            var client = await _context.Clients.FirstOrDefaultAsync(i=>i.Id==account.ClientId);
            if (client == null)
            {
                result = $"El cliente {account.ClientId} no existe!";
            }
            return result;
        }
    }
}

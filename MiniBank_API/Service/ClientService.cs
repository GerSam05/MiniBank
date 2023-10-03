using MiniBank_API.Context;
using MiniBank_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MiniBank_API.Service
{
    public class ClientService
    {
        private readonly MiniBankContext _context;

        public ClientService(MiniBankContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _context.Clients.ToListAsync();
        }
        
        public async Task<Client?> GetById(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Client> Create(Client newClient)
        {
            await _context.Clients.AddAsync(newClient);
            await _context.SaveChangesAsync();
            return newClient;
        }

        public async Task Update(Client client)
        {
            var existingClient = await GetById(client.Id);
            if (existingClient != null)
            {
                existingClient.Name= client.Name;
                existingClient.PhoneNumber= client.PhoneNumber;
                existingClient.Email= client.Email;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var clientToDelete = await GetById(id);
            if (clientToDelete != null)
            {
                _context.Clients.Remove(clientToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using MiniBank_API.Context;
using MiniBank_API.Models;

namespace MiniBank_API.Service
{
    public class ClientService
    {
        private readonly MiniBankContext _context;

        public ClientService(MiniBankContext context)
        {
            _context = context;
        }

        public IEnumerable<Client> GetAll()
        {
            return _context.Clients.ToList();
        }

        public Client? GetById(int id)
        {
            return _context.Clients.FirstOrDefault(c => c.Id == id);
        }

        public Client Create(Client newClient)
        {
            _context.Clients.Add(newClient);
            _context.SaveChanges();
            return newClient;
        }

        public void Update(Client client)
        {
            var existingClient = GetById(client.Id);
            if (existingClient != null)
            {
                existingClient.Name= client.Name;
                existingClient.PhoneNumber= client.PhoneNumber;
                existingClient.Email= client.Email;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var clientToDelete = GetById(id);
            if (clientToDelete != null)
            {
                _context.Clients.Remove(clientToDelete);
                _context.SaveChanges();
            }
        }


    }
}

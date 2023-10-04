using MiniBank_API.Context;
using MiniBank_API.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MiniBank_API.Models.Dtos;

namespace MiniBank_API.Service
{
    public class ClientService
    {
        private readonly MiniBankContext _context;
        private readonly IMapper _mapper;

        public ClientService(MiniBankContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAll()
        {
            var listaClient = await _context.Clients.ToListAsync();
            List<ClientDto> listaClientDto = _mapper.Map<List<ClientDto>>(listaClient);
            return listaClientDto;
        }

        public async Task<ClientDto?> GetClientDtoById(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            ClientDto clientDto = _mapper.Map<ClientDto>(client);
            return clientDto;
        }

        public async Task<Client?> GetById(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Client> Create(ClientCreateDto newClientDto)
        {
            Client newClient = _mapper.Map<Client>(newClientDto);
            await _context.Clients.AddAsync(newClient);
            await _context.SaveChangesAsync();
            return newClient;
        }

        public async Task Update(ClientUpdateDto clientUpdateDto)
        {
            var existingClient = await GetById(clientUpdateDto.Id);
            if (existingClient != null)
            {
                Client client = _mapper.Map<Client>(clientUpdateDto);
                client.RegDate = existingClient.RegDate;
                _context.Clients.Update(client);
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

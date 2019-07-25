using ClientsTests.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientsTests.Repositories
{
	public class ClientRepository : IClientRepository
	{
		ApplicationContext Context { get; }

		public ClientRepository(ApplicationContext context) => Context = context;

		public async Task AddClient(Client client)
		{
			await Context.Clients.AddAsync(client);
			await Context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Client>> GetClients() => await Context.Clients.ToListAsync();

		public async Task<Client> GetClient(int id)
		{
			var client = await Context.Clients.FirstOrDefaultAsync(n => n.Id == id);
			return client;
		}

		public async Task UpdateClient(Client oldClient, Client newClient)
		{
			oldClient.Name = newClient.Name;
			await Context.SaveChangesAsync();
		}

		public async Task DeleteClient(Client client)
		{
			Context.Clients.Remove(client);
			await Context.SaveChangesAsync();
		}
	}
}
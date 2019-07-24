using ClientsIntegrationTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientsIntegrationTest.Repositories
{
	public interface IClientRepository
	{
		Task AddClient(Client client);

		Task<IEnumerable<Client>> GetClients();

		Task<Client> GetClient(int id);

		Task UpdateClient(Client oldClient, Client newClient);

		Task DeleteClient(Client client);
	}
}
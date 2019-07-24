using ClientsIntegrationTest.Repositories;
using ClientsIntegrationTest.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClientsIntegrationTest.Controllers
{
	[Route("api/clients")]
	public class ClientsController : ControllerBase
	{
		IClientRepository Repository { get; }

		public ClientsController(IClientRepository repository) => Repository = repository;

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Client client)
		{
			if (client == null || client.Name == null)
			{
				return NotFound();
			}

			await Repository.AddClient(client);
			return Ok(client);
		}

		[HttpGet]
		public async Task<IEnumerable<Client>> Get() => await Repository.GetClients();

		[HttpGet("{id}")]
		public async Task<IActionResult> GetClient(int id)
		{
			Client client = await Repository.GetClient(id);

			if (client == null)
			{
				return NotFound();
			}

			return Ok(client);
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody]Client newClient)
		{
			if (newClient == null || newClient.Name == null)
			{
				return NotFound();
			}

			Client oldClient = await Repository.GetClient(newClient.Id);

			if (oldClient == null)
			{
				return NotFound();
			}

			await Repository.UpdateClient(oldClient, newClient);
			return Ok(newClient);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			Client client = await Repository.GetClient(id);

			if (client == null)
			{
				return NotFound();
			}

			await Repository.DeleteClient(client);
			return Ok(client);
		}
	}
}
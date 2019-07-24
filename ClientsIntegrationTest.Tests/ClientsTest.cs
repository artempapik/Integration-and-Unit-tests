using ClientsIntegrationTest.Repositories;
using ClientsIntegrationTest.Controllers;
using Microsoft.Extensions.Configuration;
using ClientsIntegrationTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System;
using Moq;

//AAA - Arrange, Act, Assert

//changed namespace
//URL stores in json
//naming convention
//fluent validation
//FirstOrDefault()
//get client from id
//test for invalid client
//wrote unit tests

namespace Tests
{
	public class ClientsTest
	{
		[SetUp]
		public void Init()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			URL = configuration["IntegrationTest:URL"];
		}

		static string URL { get; set; }

		static HttpClient Client { get; } = new HttpClient();

		static string Alphabet { get; } = "abcdefgijklmnopqrstuvwxyz";

		async Task<List<Client>> GetClientsAsync()
		{
			var response = await Client.GetAsync(URL);
			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Client>>(content);
		}

		/* Integration Tests */

		[Test]
		public async Task ClientGetAsync_GettingClients_ClientCreated()
		{
			var oldClients = await GetClientsAsync();

			//generate post message
			var jsonClient = JsonConvert.SerializeObject(new Client { Name = "Test" });
			var stringContent = new StringContent(jsonClient, Encoding.UTF8, "application/json");
			await Client.PostAsync(URL, stringContent);

			var newClients = await GetClientsAsync();

			newClients.Count
				.Should()
				.Be(oldClients.Count + 1);
		}

		[Test]
		public async Task ClientChangeAsync_ChangingClients_ClientChanged()
		{
			var clients = await GetClientsAsync();
			Client oldClient = clients.FirstOrDefault();

			if (oldClient == default)
			{
				throw new ArgumentNullException("clients database is empty");
			}

			int oldClientId = oldClient.Id;

			//name must be unique
			var random = new Random();
			string newName = default;

			for (int i = 0; i < 10; i++)
			{
				newName += $"{Alphabet[random.Next(0, 24)]}";
			}

			var jsonClient = JsonConvert.SerializeObject(new Client { Id = oldClientId, Name = newName });
			var stringContent = new StringContent(jsonClient, Encoding.UTF8, "application/json");
			await Client.PutAsync(URL, stringContent);

			//get client from id
			var response = await Client.GetAsync($"{URL}/{oldClientId}");
			var content = await response.Content.ReadAsStringAsync();
			Client newClient = JsonConvert.DeserializeObject<Client>(content);

			oldClient.Name
				.Should()
				.NotBe(newClient.Name);
		}

		[Test]
		public async Task ClientDeleteAsync_DeletingClients_ClientDeleted()
		{
			var oldClients = await GetClientsAsync();
			Client client = oldClients.FirstOrDefault();

			if (client == default)
			{
				throw new ArgumentNullException("clients database is empty");
			}

			await Client.DeleteAsync($"{URL}/{client.Id}");
			var newClients = await GetClientsAsync();

			newClients.Count
				.Should()
				.Be(oldClients.Count - 1);
		}

		/* Unit Tests */

		[Test]
		public async Task CreateValidAndInvalidClientAsync_Creating_OnlyOneIsCreated()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			Client emptyClient = null;
			var nameNullClient = new Client { Name = null };
			var notEmptyClient = new Client { Id = 1, Name = "Iggor" };

			var notFoundResult = await controller.Post(emptyClient);
			var nameNullResult = await controller.Put(nameNullClient);
			var okResult = await controller.Post(notEmptyClient);

			int i = 1;
		}

		[Test]
		public async Task PutValidAndInvalidClientAsync_Putting_OnlyOneIsPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = new List<Client>
			{
				new Client
				{
					Id = 1,
					Name = "sweet pupsik"
				},
				new Client
				{
					Id = 2,
					Name = "Igor"
				},
				new Client
				{
					Id = 3,
					Name = "Kolya"
				}
			};

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(1)).Returns(Task.FromResult(clients[0]));

			Client emptyClient = null;
			var clientEmptyName = new Client { Id = 1, Name = null };
			var clientWithBigId = new Client { Id = int.MaxValue, Name = "test" };
			var okClient = new Client { Id = 1, Name = "Artem" };

			var notFoundResult = await controller.Put(emptyClient);
			var notFoundName = await controller.Put(clientEmptyName);
			var notFoundId = await controller.Put(clientWithBigId);
			var okResult = await controller.Put(okClient);

			int i = 1;
		}

		[Test]
		public async Task DeleteValidAndInvalidClientAsync_Deletting_OnlyOneIsDeleted()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = new List<Client>
			{
				new Client
				{
					Id = 1,
					Name = "sweet pupsik"
				},
				new Client
				{
					Id = 2,
					Name = "Igor"
				},
				new Client
				{
					Id = 3,
					Name = "Kolya"
				}
			};

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(1)).Returns(Task.FromResult(clients[0]));

			int idNotExist = int.MaxValue;
			int idExist = 1;

			var notFoundResult = await controller.Delete(idNotExist);
			var okResult = await controller.Delete(idExist);

			int i = 1;
		}
	}
}
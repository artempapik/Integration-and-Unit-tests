using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientsTests.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Net.Http;
using Newtonsoft.Json;
using ClientsTests;
using System.Text;
using System.Linq;
using System;

//AAA - Arrange, Act, Assert

//changed namespace
//URL stores in json
//naming convention
//fluent validation
//FirstOrDefault()
//get client from id
//test for invalid client
//wrote unit tests

//integration tests - fill database before testing
//fix naming convention in testing methods
//on deleting, check if user with following id doesn't exist rather than check size of database
//move unit tests into new project
//move logic for new name & filing mock database into separate methods
//in unit tests, make separate methods for each test object
//there should be some assert in the end of the test method
//change string concatenation to StringBuilder in new client name creating
//remove any hardcoded values in tests
//check that all tests is successfully passed
//changed first project name to avoid nested namespaces
//read http response status codes & change some responses in controller
//check if class field is private by default
//install ReSharper
//code refactoring using ReSharper
//read about React

namespace ClientsIntegrationTest.Tests
{
	public class IntegrationTests
	{
		private static string ErrorMessage { get; } = "clients database is empty";

		[SetUp]
		public async Task Init()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			Url = configuration["IntegrationTest:URL"];

			//clearing the database
			var clients = await GetClientsAsync();

			while (clients.Count != 0)
			{
				Client client = clients.FirstOrDefault();

				if (client != null)
				{
					await Client.DeleteAsync($"{Url}/{client.Id}");
				}

				clients.RemoveFirst();
			}

			//post some clients
			for (int i = 0; i < 10; i++)
			{
				var jsonClient = JsonConvert.SerializeObject(new Client { Name = Utils.CreateNewClientName() });
				var stringContent = new StringContent(jsonClient, Encoding.UTF8, "application/json");
				await Client.PostAsync(Url, stringContent);
			}
		}

		private static string Url { get; set; }

		private static HttpClient Client { get; } = new HttpClient();

		private static async Task<LinkedList<Client>> GetClientsAsync()
		{
			var response = await Client.GetAsync(Url);
			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<LinkedList<Client>>(content);
		}

		[Test]
		public async Task ClientGetAsync_GettingClients_ClientCreated()
		{
			string newName = Utils.CreateNewClientName();

			//generate post message
			var jsonClient = JsonConvert.SerializeObject(new Client { Name = newName });
			var stringContent = new StringContent(jsonClient, Encoding.UTF8, "application/json");
			await Client.PostAsync(Url, stringContent);

			var newClients = await GetClientsAsync();

			newClients
				.Last()
				.Name
				.Should()
				.Be(newName);
		}

		[Test]
		public async Task ClientChangeAsync_ChangingClients_ClientNameChanged()
		{
			var clients = await GetClientsAsync();
			Client oldClient = clients.FirstOrDefault();

			if (oldClient == null)
			{
				throw new ArgumentNullException(ErrorMessage);
			}

			int oldClientId = oldClient.Id;
			string newName = Utils.CreateNewClientName();

			var jsonClient = JsonConvert.SerializeObject(new Client { Id = oldClientId, Name = newName });
			var stringContent = new StringContent(jsonClient, Encoding.UTF8, "application/json");
			await Client.PutAsync(Url, stringContent);

			//get client from id
			var response = await Client.GetAsync($"{Url}/{oldClientId}");
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

			if (client == null)
			{
				throw new ArgumentNullException(ErrorMessage);
			}

			await Client.DeleteAsync($"{Url}/{client.Id}");
			var newClients = await GetClientsAsync();

			//check if database contains client after deleting
			bool clientExist = newClients.Contains(client);

			clientExist
				.Should()
				.BeFalse();
		}
	}
}
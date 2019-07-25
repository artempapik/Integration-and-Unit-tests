using System.Collections.Generic;
using ClientsTests.Repositories;
using ClientsTests.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClientsTests.Models;
using FluentAssertions;
using NUnit.Framework;
using ClientsTests;
using System.Linq;
using Moq;

namespace ClientUnitTest.Tests
{
	public class UnitTests
	{
		[Test]
		public async Task CreateNullClientAsync_CreatingClient_ClientIsNotCreated()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			Client emptyClient = null;
			var result = await controller.Post(emptyClient);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task CreateClientWithNullNameAsync_CreatingClient_ClientIsNotCreated()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var nameNullClient = new Client { Name = null };
			var result = await controller.Put(nameNullClient);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task CreateValidClientAsync_CreatingClient_ClientIsCreated()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var validCLient = new Client { Id = 1, Name = Utils.CreateNewClientName() };
			var result = await controller.Post(validCLient);

			result
				.GetType()
				.Should()
				.Be(typeof(OkObjectResult));
		}

		List<Client> CreateClientsMock() => new List<Client>
		{
			new Client
			{
				Id = 1,
				Name = "Sweet Pupsik"
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

		[Test]
		public async Task PutNullClientAsync_PuttingClient_ClientIsNotPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = CreateClientsMock();

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(clients[0].Id)).Returns(Task.FromResult(clients[0]));

			Client emptyClient = null;
			var result = await controller.Put(emptyClient);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task PutClientWithNullNameAsync_PuttingClient_ClientIsNotPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = CreateClientsMock();

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(clients[0].Id)).Returns(Task.FromResult(clients[0]));

			var clientEmptyName = new Client { Id = clients[0].Id, Name = null };
			var result = await controller.Put(clientEmptyName);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task PutClientWithUnexistingIdAsync_PuttingClient_ClientIsNotPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = CreateClientsMock();

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(clients[0].Id)).Returns(Task.FromResult(clients[0]));

			var clientWithBigId = new Client { Id = int.MaxValue, Name = Utils.CreateNewClientName() };
			var result = await controller.Put(clientWithBigId);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task PutValidClientAsync_PuttingClient_ClientIsPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = CreateClientsMock();

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(clients[0].Id)).Returns(Task.FromResult(clients[0]));

			var okClient = new Client { Id = clients[0].Id, Name = Utils.CreateNewClientName() };
			var result = await controller.Put(okClient);

			result
				.GetType()
				.Should()
				.Be(typeof(OkObjectResult));
		}

		[Test]
		public async Task DeleteClientWithUnexistingIdAsync_DelettingClient_ClientIsNotDeleted()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = CreateClientsMock();

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(clients[0].Id)).Returns(Task.FromResult(clients[0]));

			int idNotExist = int.MaxValue;
			var result = await controller.Delete(idNotExist);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task DeleteValidClientAsync_DelettingClient_ClientIsDeleted()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			var clients = CreateClientsMock();

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(clients.AsEnumerable()));
			mock.Setup(n => n.GetClient(clients[0].Id)).Returns(Task.FromResult(clients[0]));

			int idExist = clients[0].Id;
			var result = await controller.Delete(idExist);

			result
				.GetType()
				.Should()
				.Be(typeof(OkObjectResult));
		}
	}
}
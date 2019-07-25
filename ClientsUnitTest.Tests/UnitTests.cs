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
		private Client Client { get; } = new Client
		{
			Id = 1,
			Name = "Igor"
		};

		private IEnumerable<Client> CreateClientsMock() => new List<Client>
		{
			Client,
			new Client
			{
				Id = 2,
				Name = "Igor"
			},
			new Client
			{
				Id = 3,
				Name = "Nick"
			}
		};

		[Test]
		public async Task CreateNullClientAsync_CreatingClient_ClientIsNotCreated()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);
			var result = await controller.Post(null);

			result
				.GetType()
				.Should()
				.Be(typeof(NoContentResult));
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
				.Be(typeof(NoContentResult));
		}

		[Test]
		public async Task CreateValidClientAsync_CreatingClient_ClientIsCreated()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(CreateClientsMock().AsEnumerable()));

			var validClient = new Client { Id = 1, Name = Utils.CreateNewClientName() };
			var result = await controller.Post(validClient);

			result
				.GetType()
				.Should()
				.Be(typeof(OkObjectResult));
		}

		[Test]
		public async Task PutNullClientAsync_PuttingClient_ClientIsNotPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			mock.Setup(n => n.GetClient(Client.Id)).Returns(Task.FromResult(Client));
			var result = await controller.Put(null);

			result
				.GetType()
				.Should()
				.Be(typeof(NoContentResult));
		}

		[Test]
		public async Task PutClientWithNullNameAsync_PuttingClient_ClientIsNotPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			mock.Setup(n => n.GetClient(Client.Id)).Returns(Task.FromResult(Client));

			var clientEmptyName = new Client { Id = Client.Id, Name = null };
			var result = await controller.Put(clientEmptyName);

			result
				.GetType()
				.Should()
				.Be(typeof(NoContentResult));
		}

		[Test]
		public async Task PutClientWithNonExistingIdAsync_PuttingClient_ClientIsNotPut()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			mock.Setup(n => n.GetClient(Client.Id)).Returns(Task.FromResult(Client));

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

			mock.Setup(n => n.GetClient(Client.Id)).Returns(Task.FromResult(Client));

			string newName = Utils.CreateNewClientName();
			var okClient = new Client { Id = Client.Id, Name = newName };
			var result = await controller.Put(okClient);

			result
				.GetType()
				.Should()
				.Be(typeof(OkObjectResult));
		}

		[Test]
		public async Task DeleteClientWithNonExistingIdAsync_DeletingClient_ClientIsNotDeleted()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);

			mock.Setup(n => n.GetClient(Client.Id)).Returns(Task.FromResult(Client));

			int idNotExist = int.MaxValue;
			var result = await controller.Delete(idNotExist);

			result
				.GetType()
				.Should()
				.Be(typeof(NotFoundResult));
		}

		[Test]
		public async Task DeleteValidClientAsync_DeletingClient_ClientIsDeleted()
		{
			var mock = new Mock<IClientRepository>();
			var controller = new ClientsController(mock.Object);
		
			mock.Setup(n => n.GetClients()).Returns(Task.FromResult(CreateClientsMock().AsEnumerable()));
			mock.Setup(n => n.GetClient(Client.Id)).Returns(Task.FromResult(Client));

			int idExist = Client.Id;
			var result = await controller.Delete(idExist);

			result
				.GetType()
				.Should()
				.Be(typeof(OkObjectResult));
		}
	}
}
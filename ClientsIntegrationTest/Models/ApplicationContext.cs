using Microsoft.EntityFrameworkCore;

namespace ClientsTests.Models
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) => Database.EnsureCreated();

		public DbSet<Client> Clients { get; set; }
	}
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace ClientsIntegrationTest
{
	public class Program
	{
		public static void Main(string[] args) => WebHost
			.CreateDefaultBuilder(args)
			.UseStartup<Startup>()
			.Build()
			.Run();
	}
}
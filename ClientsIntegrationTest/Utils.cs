using System.Text;
using System.Linq;
using System;

namespace ClientsTests
{
	public static class Utils
	{
		private static string Alphabet { get; } = $@"{Enumerable
			.Range('A', 'Z' - 'A' + 1)
			.Select((c => (char) c))}";

		public static string CreateNewClientName()
		{
			var random = new Random();
			var newName = new StringBuilder();

			for (int i = 0; i < 10; i++)
			{
				newName.Append($"{Alphabet[random.Next(0, 24)]}");
			}

			return $"{newName}";
		}
	}
}
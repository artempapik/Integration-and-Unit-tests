using System.Text;
using System;

namespace ClientsTests
{
	public static class Utils
	{
		static string Alphabet { get; } = "abcdefgijklmnopqrstuvwxyz";

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
using System.Text;
using System.Linq;
using System;

namespace ClientsTests
{
	public static class Utils
	{
		private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static string CreateNewClientName()
		{
			var random = new Random();
			var newName = new StringBuilder();

			for (int i = 0; i < 10; i++)
			{
				newName.Append($"{Alphabet[random.Next(0, Alphabet.Length - 1)]}");
			}

			return $"{newName}";
		}
	}
}
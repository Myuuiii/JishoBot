using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace JishoBot
{
	public class Bot
	{
		public static void Main(string[] args)
		{
			if (!File.Exists("token.txt"))
			{
				File.Create("token.txt").Close();
				Console.Write("Please enter your bot token:");
				string token = Console.ReadLine();
				File.WriteAllText("token.txt", token);
			}
			MainAsync().GetAwaiter().GetResult();
		}

		public static async Task MainAsync()
		{
			DiscordClient client = new DiscordClient(new DiscordConfiguration()
			{
				Token = File.ReadAllText("token.txt"),
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.All
			});



			await client.ConnectAsync();
			client.Ready += async (sender, e) =>
			{
				await client.UpdateStatusAsync(new DiscordActivity("j!help", ActivityType.ListeningTo), UserStatus.Online);
			};
			client.MessageCreated += HandleMessage;

			await Task.Delay(-1);
		}

		private static async Task HandleMessage(DiscordClient sender, MessageCreateEventArgs e)
		{
			if (e.Message.Author.IsBot) return;
			if (e.Message.Content.ToLower().StartsWith("j! "))
				await Commands.JishoCommand.ExecuteAsync(sender, e, false);
			else if (e.Message.Content.ToLower().StartsWith("j!about"))
				await Commands.AboutCommand.ExecuteAsync(sender, e);
			else if (e.Message.Content.ToLower().StartsWith("j!help"))
				await Commands.HelpCommand.ExecuteAsync(sender, e);

			if (e.Message.Channel.Name == "jisho-bot")
				await Commands.JishoCommand.ExecuteAsync(sender, e, true);
		}
	}
}
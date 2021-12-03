using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace JishoBot.Commands
{
	public class AboutCommand
	{
		public static async Task ExecuteAsync(DiscordClient sender, MessageCreateEventArgs e)
		{
			DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
			embedBuilder.Title = "About JishoBot";

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("**Used libraries**");
			sb.AppendLine("[JishoNET](https://github.com/Myuuiii/JishoNET) for retrieving definitions");
			sb.AppendLine("[KanjiNET](https://github.com/Myuuiii/KanjiNET) for retrieving kanji information");
			sb.AppendLine();
			sb.AppendLine("**Original Websites**");
			sb.AppendLine("[Jisho.org](https://jisho.org) as a source for all definitions (used by JishoNET)");
			sb.AppendLine("[Kanjiapi.dev](https://kanjiapi.dev) as a source for all kanji definitions (used by KanjiNET)");
			sb.AppendLine();
			sb.AppendLine("**Support this project**");
			sb.AppendLine("You can support this project by sharing it to others and leaving a star on [JishoBot](https://github.com/Myuuiii/JishoBot)'s GitHub page");
			sb.AppendLine();
			sb.AppendLine("Thank you for using JishoBot! ❤");

			embedBuilder.Description = sb.ToString();
			embedBuilder.WithColor(new DiscordColor(0, 255, 0));

			await e.Message.RespondAsync(null, embedBuilder.Build());
		}
	}
}
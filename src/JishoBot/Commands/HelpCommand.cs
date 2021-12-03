using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace JishoBot.Commands
{
	public class HelpCommand
	{
		public static async Task ExecuteAsync(DiscordClient sender, MessageCreateEventArgs e)
		{
			DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();

			embedBuilder.Title = "JishoBot - Help";

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("`j! <term>`");
			sb.AppendLine("*Used to look up a kanji or a word on Jisho.org and Kanjiapi.dev*");
			sb.AppendLine();
			sb.AppendLine("`j!about`");
			sb.AppendLine("*Displays some information about JishoBot, as well as how you can support us*");
			sb.AppendLine();
			sb.AppendLine("`j!help`");
			sb.AppendLine("*Displays this message*");

			embedBuilder.Description = sb.ToString();
			embedBuilder.WithColor(new DiscordColor(0, 255, 0));

			await e.Message.RespondAsync(null, embedBuilder.Build());
		}
	}
}
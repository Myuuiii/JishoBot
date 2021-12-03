using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Humanizer;
using JishoNET.Models;
using KanjiNET;
using KanjiNET.Models;

namespace JishoBot.Commands
{
	public class JishoCommand
	{
		public static async Task ExecuteAsync(DiscordClient sender, MessageCreateEventArgs e)
		{
			string searchTerm = string.Join(' ', e.Message.Content.Split(' ').Skip(1).ToArray());

			JishoClient jishoClient = new JishoClient();
			JishoResult result = jishoClient.GetDefinition(searchTerm);

			if (result.Success && result.Data.Length > 0)
			{
				DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
				embedBuilder.Title = "Definition of: " + searchTerm;

				StringBuilder sb = new StringBuilder();

				if (Regex.Matches(searchTerm, "[一-龯]").Count == 1 && searchTerm.Length == 1)
				{
					KanjiClient kanjiClient = new KanjiClient();
					KanjiResult<KanjiDefinition> kanjiResult = kanjiClient.GetKanjiDefinition(Regex.Matches(searchTerm, "[一-龯]").First().Value);

					KanjiDefinition kanjiInfo = kanjiResult.Data;

					if (kanjiResult.Success)
					{
						sb.AppendLine($"***Kanji Information***");
						sb.AppendLine($"Stroke count: {kanjiInfo.StrokeCount}");
						if (kanjiInfo.KunReadings.Length != 0)
						{
							sb.AppendLine($"Kun-Readings : {string.Join(", ", kanjiInfo.KunReadings)}");
						}
						if (kanjiInfo.OnReadings.Length != 0)
						{
							sb.AppendLine($"On-Readings: {string.Join(", ", kanjiInfo.OnReadings)}");
						}
						sb.AppendLine($"JLPT Level: {kanjiInfo.JlptLevel}");
						sb.AppendLine();
					}
					else
					{
						sb.AppendLine($"***Kanji Information could not be found***");
						sb.AppendLine();
					}
				}

				// take the first 3 results from Jisho.org and format them into the embed
				int currentItem = 1;
				foreach (var dataResult in result.Data.Take(3))
				{
					if (String.IsNullOrWhiteSpace(dataResult.Japanese[0].Reading)) continue;
					sb.AppendLine($"***{currentItem++.ToOrdinalWords().Humanize()} definition ***");
					sb.AppendLine($"**English Definitions**: ");

					foreach (var sense in dataResult.Senses.Take(4))
					{
						sb.AppendLine($"- [{sense.PartsOfSpeech[0]}] *{string.Join(", ", sense.EnglishDefinitions)}*");
					}

					if (!string.IsNullOrWhiteSpace(dataResult.Japanese[0].Reading))
						sb.AppendLine($"**Japanese Reading**: {dataResult.Japanese[0].Reading}");

					if (!string.IsNullOrWhiteSpace(dataResult.Japanese[0].Word))
						sb.AppendLine($"**Japanese Word**: {dataResult.Japanese[0].Word}");

					sb.AppendLine("");

				}

				embedBuilder.Description = sb.ToString();
				embedBuilder.WithFooter($"Definitions provided by Jisho.org using Myuuiii's JishoNET wrapper {Environment.NewLine}Kanji Information provided by kanjiapi.dev using Myuuiii's KanjiNET wrapper");
				embedBuilder.WithColor(new DiscordColor(0, 255, 0));

				await e.Message.RespondAsync(null, embedBuilder.Build());
			}
			else
			{
				await e.Message.RespondAsync("No results found for your query.");
			}
		}
	}
}
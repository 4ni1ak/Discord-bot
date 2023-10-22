using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatAndSet.Presentation.SlashCommands
{
	internal class FunSL : ApplicationCommandModule

	{
		[SlashCommand("Test", "This is first Slash Command")]
		public async Task TestSlashCommand(InteractionContext ctx)

		{
			var embedMessage = new DiscordEmbedBuilder()
			{
				Title = "Test"
			};
			await ctx.Channel.SendMessageAsync(embed: embedMessage);
		}

	}
}

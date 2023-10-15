using CreadAndSet.CardSystem;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace CreatAndSet.Commands
{
	public class TestCommands : BaseCommandModule
	{
		[Command("Test")]
		public async Task MyFirstCommand(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync($"Are u lost {ctx.User.Username}" +
				$"{ctx.User.Email}");



		}
		[Command("Add")]
		public async Task Add(CommandContext ctx, int number1, int number2)
		{
			await ctx.Channel.SendMessageAsync($"{(number1 + number2).ToString()}");


		}
		[Command("Embeded")]
		public async Task Embeded(CommandContext ctx)
		{
			var message = new DiscordMessageBuilder()
				.AddEmbed(new DiscordEmbedBuilder()
					.WithTitle("This is a title")
					.WithDescription("This is a description"));

			await ctx.Channel.SendMessageAsync(message);
		}

		[Command("Embeded2")]
		public async Task EmbededMessage(CommandContext ctx)
		{
			var message = new DiscordEmbedBuilder()
			{
				Title = "This is a title",
				Description = $"This command was executed by {ctx.User.Username}",
				Color = DiscordColor.Blue

			};

			await ctx.Channel.SendMessageAsync(embed: message);
		}
		[Command("cardgame")]
		public async Task CardGame(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("Test");
			//for (int i = 0; i < 1000; i++)
			//         {

			(var userCard, var botCard) = SetCards();
			await ctx.Channel.SendMessageAsync("Kartlar atandı");
			
			await ctx.Channel.SendMessageAsync(userCard.SelectedCard);
			await ctx.Channel.SendMessageAsync(botCard.SelectedCard);

			if (userCard.SelectedCard != botCard.SelectedCard)
			{
				await ctx.Channel.SendMessageAsync("If Main");

				var userCarEmbed = new DiscordEmbedBuilder()
				{

					Title = $"In {ctx.User.Username}'s hand ",
					Description = $"{userCard.SelectedCard}",
					Color = DiscordColor.Blue
				};
				await ctx.Channel.SendMessageAsync(content: userCarEmbed.Title);
				var botCarEmbed = new DiscordEmbedBuilder()
				{

					Title = $"In my hand ",
					Description = $"{botCard.SelectedCard}",
					Color = DiscordColor.Blue
				};
				await ctx.Channel.SendMessageAsync(embed: botCarEmbed);
				if (userCard.SelectedNumber > botCard.SelectedNumber)
				{
					var winMessage = new DiscordEmbedBuilder
					{
						Title = "Congratulations, You Win !!!",
						Color = DiscordColor.Gold
					};
					await ctx.Channel.SendMessageAsync(embed: winMessage);
				}
				else if ((userCard.SelectedNumber == botCard.SelectedNumber))
				{
					var drawMessage = new DiscordEmbedBuilder
					{
						Title = "Draw, lets play again !!!",
						Color = DiscordColor.Lilac
					};
					await ctx.Channel.SendMessageAsync(embed: drawMessage);
					await CardGame(ctx);
				}
				else
				{
					var loseMessage = new DiscordEmbedBuilder
					{
						Title = "You Lost the game!!",
						Color = DiscordColor.Red
					};	
					await ctx.Channel.SendMessageAsync(embed: loseMessage);
				}
				await ctx.Channel.SendMessageAsync("Başarrılı");
			}
		}

		public (CardSystem userCard, CardSystem botCard) SetCards()
		{
			var userCard = new CardSystem();
			System.Threading.Thread.Sleep(2000);
			var botCard = new CardSystem();
			System.Threading.Thread.Sleep(2000);
			return (userCard, botCard);
		}
	}

}


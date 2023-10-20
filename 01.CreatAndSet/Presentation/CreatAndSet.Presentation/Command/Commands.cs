using CreadAndSet.CardSystem;
using Presentation;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Commands
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
			await ctx.Channel.SendMessageAsync($"{(number1 + number2)}");


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


		[Command("Interactivity")]
		public async Task InteractivityMesSage(CommandContext ctx)
		{
			var interactivity = Program.Client.GetInteractivity();
			var messageToRetrieve = await interactivity.WaitForMessageAsync(message => message.Content == "Hello");
			if (messageToRetrieve.Result.Content == "Hello")
				await ctx.Channel.SendMessageAsync($" {ctx.User.Username} said Hello");

		}

		[Command("InteractivityEmoj")]
		public async Task InteractivityEmoj(CommandContext ctx)
		{
			var lastMessage = await ctx.Channel.GetMessagesAsync(1);
			if (lastMessage.Count > 0)
			{
				var lastMessageId = lastMessage.First().Id;

				var interactivity = Program.Client.GetInteractivity();
				var messageToReact = await interactivity.WaitForReactionAsync(message => message.Message.Id == lastMessageId);
				if (messageToReact.Result.Message.Id == lastMessageId)
				{
					await ctx.Channel.SendMessageAsync($" {ctx.User.Username} used the emoji with name {messageToReact.Result.Emoji.Name}");
				}

			}



			else
			{
				await InteractivityEmoj(ctx);
			}
		}

		[Command("Poll")]
		public async Task Poll(CommandContext ctx, string option1, string option2, string option3, string option4, [RemainingText] string pollTitle)

		{

			var interactivity = Program.Client.GetInteractivity();
			var pollTime = TimeSpan.FromSeconds(10);

			DiscordEmoji[] emojiOptions = {DiscordEmoji.FromName(Program.Client, ":one:"),
										   DiscordEmoji.FromName(Program.Client, ":two:"),
										   DiscordEmoji.FromName(Program.Client, ":three:"),
										   DiscordEmoji.FromName(Program.Client, ":four:")};
			string optionsDescription = $"{emojiOptions[0]}|{option1} \n" +
										$"{emojiOptions[1]}|{option2} \n" +
										$"{emojiOptions[2]}|{option3} \n" +
										$"{emojiOptions[3]}|{option4} \n";

			var pollMessage = new DiscordEmbedBuilder
			{
				Color = DiscordColor.Red,
				Title = pollTitle,
				Description = optionsDescription
			};

			var sentPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);

			foreach (var emoji in emojiOptions)
			{
				await sentPoll.CreateReactionAsync(emoji);

			}
			var totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);
			int count1 = 0, count2 = 0, count3 = 0, count4 = 0;

			foreach (var emoji in totalReactions)
			{
				switch (emoji.Emoji)
				{
					case var e when e == emojiOptions[0]:
						count1++;
						break;
					case var e when e == emojiOptions[1]:
						count2++;
						break;
					case var e when e == emojiOptions[2]:
						count3++;
						break;
					case var e when e == emojiOptions[3]:
						count4++;
						break;
				}
			}
			int totalVotes = count1 + count2 + count3 + count4;
			string resultsDescription = $"{emojiOptions[0]}: {count1} Votes \n" +
			$"{emojiOptions[1]}: {count2} Votes \n" +
			$"{emojiOptions[2]}: {count3} Votes \n" +
			$"{emojiOptions[3]}: {count4} Votes \n\n" +
			$"Total Votes: {totalVotes}";

			var resultEmbed = new DiscordEmbedBuilder
			{
				Color = DiscordColor.Green,
				Title = "Results of the Poll",
				Description = resultsDescription
			};
			await ctx.Channel.SendMessageAsync(embed: resultEmbed);
		}
		[Command("cardgame")]
		public async Task CardGame(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("Test");
			for (int i = 0; i < 1000; i++)
			{
				(var userCard, var botCard) = SetCards();
				if (userCard.SelectedCard != botCard.SelectedCard)
				{
					await ctx.Channel.SendMessageAsync("If Main");

					var userCarEmbed = new DiscordEmbedBuilder()
					{

						Title = $"In {ctx.User.Username}'s hand ",
						Description = $"{userCard.SelectedCard}",
						Color = DiscordColor.LightGray
					};
					await ctx.Channel.SendMessageAsync(embed: userCarEmbed);
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
						break;
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
						break;
					}
					else
					{
						var loseMessage = new DiscordEmbedBuilder
						{
							Title = "You Lost the game!!",
							Color = DiscordColor.Red
						};
						await ctx.Channel.SendMessageAsync(embed: loseMessage);
						break;
					}

				}
			}
		}

		public (CardSystem userCard, CardSystem botCard) SetCards()
		{
			var userCard = new CardSystem();
			var botCard = new CardSystem();
			return (userCard, botCard);
		}
	}


}

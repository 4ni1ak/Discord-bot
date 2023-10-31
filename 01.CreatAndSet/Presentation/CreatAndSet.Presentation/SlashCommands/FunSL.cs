using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Presentation;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatAndSet.Presentation.SlashCommands
{
	internal class FunSL : ApplicationCommandModule

	{
		[SlashCommand("Test", "This is first Slash Command")]
		public async Task TestSlashCommand(InteractionContext ctx, [Choice("Pre-Defined Text","ıwghoıgahaghıwhowh" )] [Option("String", "Type in anything you want ") ] string text )

		{
			await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Starting Slash Command..."));

			var embedMessage = new DiscordEmbedBuilder()
			{
				Title = text
			};
			await ctx.Channel.SendMessageAsync(embed: embedMessage);
		}

		[SlashCommand("poll", "Create your own poll")]
		public async Task PollCommand(InteractionContext ctx, [Option("question", "The main poll subject/question")] string Question,
															  [Option("timelimit", "The time set on this poll")] long TimeLimit,
															  [Option("option1", "Option 1")] string Option1,
															  [Option("option2", "Option 1")] string Option2,
															  [Option("option3", "Option 1")] string Option3,
															  [Option("option4", "Option 1")] string Option4)
		{
			await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
																				.WithContent("..."));

			var interactvity = Program.Client.GetInteractivity(); 
			TimeSpan timer = TimeSpan.FromSeconds(TimeLimit); 

			DiscordEmoji[] optionEmojis = { DiscordEmoji.FromName(Program.Client, ":one:", false),
											DiscordEmoji.FromName(Program.Client, ":two:", false),
											DiscordEmoji.FromName(Program.Client, ":three:", false),
											DiscordEmoji.FromName(Program.Client, ":four:", false) }; 

			string optionsString = optionEmojis[0] + " | " + Option1 + "\n" +
								   optionEmojis[1] + " | " + Option2 + "\n" +
								   optionEmojis[2] + " | " + Option3 + "\n" +
								   optionEmojis[3] + " | " + Option4; 

			var pollMessage = new DiscordMessageBuilder()
				.AddEmbed(new DiscordEmbedBuilder()
					.WithColor(DiscordColor.Azure)
					.WithTitle(string.Join(" ", Question))
					.WithDescription(optionsString)); 

			var putReactOn = await ctx.Channel.SendMessageAsync(pollMessage); 

			foreach (var emoji in optionEmojis)
			{
				await putReactOn.CreateReactionAsync(emoji); 
			}

			var result = await interactvity.CollectReactionsAsync(putReactOn, timer); 

			int count1 = 0; 
			int count2 = 0;
			int count3 = 0;
			int count4 = 0;

			foreach (var emoji in result) 
			{
				if (emoji.Emoji == optionEmojis[0])
				{
					count1++;
				}
				if (emoji.Emoji == optionEmojis[1])
				{
					count2++;
				}
				if (emoji.Emoji == optionEmojis[2])
				{
					count3++;
				}
				if (emoji.Emoji == optionEmojis[3])
				{
					count4++;
				}
			}

			int totalVotes = count1 + count2 + count3 + count4;

			string resultsString = optionEmojis[0] + ": " + count1 + " Votes \n" +
								   optionEmojis[1] + ": " + count2 + " Votes \n" +
								   optionEmojis[2] + ": " + count3 + " Votes \n" +
								   optionEmojis[3] + ": " + count4 + " Votes \n\n" +
								   "The total number of votes is " + totalVotes; 

			var resultsMessage = new DiscordMessageBuilder()
				.AddEmbed(new DiscordEmbedBuilder()
					.WithColor(DiscordColor.Green)
					.WithTitle("Results of Poll")
					.WithDescription(resultsString));

			await ctx.Channel.SendMessageAsync(resultsMessage);          
		}
		[SlashCommand("caption","Give an any image a caption")]
        public async Task CaptionCommand(InteractionContext ctx, [Option("caption", "Give an any image a caption")] string caption, [Option("image", "The image you want to upload")] DiscordAttachment picture)
        {
			await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("..."));

			var captionMessage = new DiscordEmbedBuilder()
			{
				Title = caption,
				ImageUrl = picture.Url

			};

			await ctx.CreateResponseAsync(embed: captionMessage);
		}

    }
}

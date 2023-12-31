﻿using Commands;
using Config;
using CreatAndSet.Presentation.SlashCommands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;
namespace Presentation
{

	public class Program
	{
		public static DiscordClient Client { get; set; }
		private static CommandsNextExtension Commands { get; set; }
		static async Task Main(string[] args)
		{
			var jsonReader = new JSONReader();
			await jsonReader.ReadJSON();
			var discordConfig = new DiscordConfiguration()
			{
				Intents = DiscordIntents.All,
				Token = jsonReader.Token,
				TokenType = TokenType.Bot,
				AutoReconnect = true,
			};
			Client = new DiscordClient(discordConfig);

			Client.UseInteractivity(new InteractivityConfiguration()
			{
				Timeout = TimeSpan.FromMinutes(2)
			}); ;


            Client.Ready += OnClientReady;
			Client.ComponentInteractionCreated += ButtonPressResponse;
			//Client.MessageCreated += MessageCreatedHandler;
            Client.VoiceStateUpdated += VoiceChanelHandler;

			

			var commandsConfig = new CommandsNextConfiguration()
			{
				StringPrefixes = new string[] { jsonReader.Prefix },
				EnableMentionPrefix = true,
				EnableDms = true,
				EnableDefaultHelp = false,


			};
			Commands = Client.UseCommandsNext(commandsConfig);

			var slashCommandsConfig = Client.UseSlashCommands();


			Commands.CommandErrored += CommandEventHandler;

			//prefix BasedCommands
			Commands.RegisterCommands<TestCommands>();

			//Slash Commands
			slashCommandsConfig.RegisterCommands<FunSL>(1162471989068443690); 



			await Client.ConnectAsync();
			await Task.Delay(-1);
		}

        private static async Task ButtonPressResponse(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
			if (e.Interaction.Data.CustomId == "1")
			{
				await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("YOU prest the first button "));
			}
			else if (e.Interaction.Data.CustomId == "2")
			{
				await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("YOU prest the Second button "));


            }
			

			
			else if (e.Interaction.Data.CustomId == "funButton")
			{
				string funCommandsList = "!message ==> Send a message\n" +
										 "!emmbedmessage1 ==> Sends an embed message\n" +
										 "!poll ==> Stars a poll";
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(funCommandsList));
            }
			else if (e.Interaction.Data.CustomId == "gameButton")
			{
				string gameList = "!cardgame ==> Card game";
				
                
				var gamesCommandList = new DiscordInteractionResponseBuilder()
				{
					Title = "Game Commands List",
					Content = gameList
				};
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,gamesCommandList);


            }
        
        }

        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
		private static async Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
		{
			if (e.Exception is ChecksFailedException exception)
			{
				string timeLeft = string.Empty;
				foreach (var check in exception.FailedChecks)

				{
					var coolDown = (CooldownAttribute)check;
					timeLeft = coolDown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");


				}
				var coolDownMessage = new DiscordEmbedBuilder
				{
					Color = DiscordColor.Red,
					Title = "Please wait for the cooldown to end",
					Description = $"Time : {timeLeft}"
				};
				await e.Context.Channel.SendMessageAsync(embed: coolDownMessage);
			}
		}

		private static async Task VoiceChanelHandler(DiscordClient sender, VoiceStateUpdateEventArgs e)
		{
			if (e.Before is null && e.Channel.Name == "Salon")
			{
				await e.Channel.SendMessageAsync($"{e.User.Username} joined the Voice Chanel");
			}
		}
        private static async Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (e.Author.IsBot) return;

            ulong wordGameChannelID = 0000000000000000;//ur channel id

            if (e.Channel.Id != wordGameChannelID) return;

            string lastMessageContent = e.Message.Content.ToLower();

            var lastMessages = await e.Channel.GetMessagesAsync(2);

            if (lastMessages.Count != 2) return;

            string previousMessageContent = lastMessages[1].Content.ToLower();

            if (previousMessageContent.EndsWith(lastMessageContent[0].ToString()))
            {
                await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("✅"));
            }
            else
            {
                await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("❌"));
            }
        }


        //private static async Task MessageCreatedHandler(DiscordClient sender, MessageCreateEventArgs e)
        //{
        //	await e.Channel.SendMessageAsync("godmfdeam");
        //}

        //private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        //{
        //	return Task.CompletedTask;
        //}

    }
}

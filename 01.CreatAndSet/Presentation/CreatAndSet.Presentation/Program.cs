﻿using CreadAndSet.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
namespace CreatAndSet.Presentation
{

    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args)
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();
            var discordConfig=new DiscordConfiguration()
            {
                Intents=DiscordIntents.All,
                Token=jsonReader.Token,
                TokenType=TokenType.Bot,
                AutoReconnect=true,
            };
            Client = new DiscordClient(discordConfig);
            Client.Ready += Client_Ready;

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready1(DiscordClient sender, ReadyEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static Task Client_Ready(DiscordClient sender,ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        
    }
}

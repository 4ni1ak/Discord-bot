using Commands;
using Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
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
            var discordConfig=new DiscordConfiguration()
            {
                Intents=DiscordIntents.All,
                Token=jsonReader.Token,
                TokenType=TokenType.Bot,
                AutoReconnect=true,
            };
            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            }); ;


            Client.Ready += Client_Ready;
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes= new string[] {jsonReader.Prefix},
                EnableMentionPrefix= true,
                EnableDms= true,
                EnableDefaultHelp= false,

               
            };
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<TestCommands >();



            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        private static Task Client_Ready(DiscordClient sender,ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        
    }
}

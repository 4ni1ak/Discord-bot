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
        public async Task TestSlashCommand(InteractionContext ctx, [Choice("Pre-Defined Text", "ıwghoıgahaghıwhowh")][Option("String", "Type in anything you want ")] string text)

        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Starting Slash Command..."));

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = text
            };
            await ctx.Channel.SendMessageAsync(embed: embedMessage);
        }
        [SlashCommand("rnb", "Roll a dice")]
        public async Task RollDice(InteractionContext ctx,
    [Option("min", "Minimum value of the dice")] int min,
    [Option("max", "Maximum value of the dice")] int max)
        {
            if (min >= max)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Please enter a minimum value lower than the maximum value."));
                return;
            }

            Random random = new Random();
            int diceResult = random.Next(min, max + 1);

            string diceOutput = $"The dice rolled: {diceResult}";


            var embed = new DiscordEmbedBuilder()
                .WithTitle("Dice Roll Result")
                .WithDescription($"Congratulations, {ctx.User.Mention}! Your dice roll result: {diceOutput}")
                .WithColor(DiscordColor.Green);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        [SlashCommand("encrypt", "Encryption command")]
        public async Task EncryptCommand(InteractionContext ctx,
               [Option("word", "Word to be encrypted")] string word,
               [Option("key", "Encryption key (exp 3)")] int key,
               [Option("algorithm", "Encryption algorithm (caesar or reverse)")] string algorithm)
        {
            string result = "";

            switch (algorithm.ToLower())
            {
                case "caesar":
                    result = EncryptCaesarCipher(word, key);
                    break;
                case "reverse":
                    result = ReverseString(word);
                    break;
                default:
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Invalid encryption algorithm. Please use 'caesar' or 'reverse'."));
                    return;
            }

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Encryption Result")
                .WithDescription($"Encrypted word: {result}")
                .WithColor(DiscordColor.Green);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        private string EncryptCaesarCipher(string word, int key)
        {
            string encryptedWord = "";

            foreach (char letter in word)
            {
                if (char.IsLetter(letter))
                {
                    char baseLetter = char.IsUpper(letter) ? 'A' : 'a';
                    encryptedWord += (char)(((letter + key - baseLetter) % 26) + baseLetter);
                }
                else
                {
                    encryptedWord += letter;
                }
            }

            return encryptedWord;
        }

        private string ReverseString(string word)
        {
            char[] charArray = word.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        [SlashCommand("decrypt", "Text decryption command")]
        public async Task DecryptCommand(InteractionContext ctx,
    [Option("text", "Text to be decrypted")] string text,
    [Option("algorithm", "Decryption algorithm (ceasar, reverse, or base64)")] string algorithm,
    [Option("key", "Decryption key (exp 3)")] int key = 0)
        {
            string result = "";

            switch (algorithm.ToLower())
            {
                case "ceasar":
                    result = DecryptCaesarCipher(text, key);
                    break;
                case "reverse":
                    result = ReverseString(text);
                    break;
                case "base64":
                    result = Base64Decode(text);
                    break;
                default:
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Invalid decryption algorithm. Please use 'ceasar', 'reverse', or 'base64'."));
                    return;
            }

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Decryption Result")
                .WithDescription($"Result: {result}")
                .WithColor(DiscordColor.Green);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        private string DecryptCaesarCipher(string text, int key)
        {
            return EncryptCaesarCipher(text, -key); 
        }

        private string Base64Decode(string text)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(text);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
                return "Invalid Base64 decoding.";
            }
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
        [SlashCommand("caption", "Give an any image a caption")]
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
        [SlashCommand("funfact", "Get a random fun fact")]
        public async Task FunFactCommand(InteractionContext ctx)
        {
            string[] funFacts = new string[]
            {
                "Penguins have an organ above their eyes that converts seawater into freshwater.",
                "Honey never spoils. Archaeologists have found pots of honey in ancient Egyptian tombs that are over 3000 years old and still perfectly good to eat.",
                "Bananas are berries, but strawberries are not.",
                "The shortest war in history was between Britain and Zanzibar on August 27, 1896. Zanzibar surrendered after 38 minutes."
            };

            Random random = new Random();
            int index = random.Next(funFacts.Length);
            string randomFact = funFacts[index];

            var factEmbed = new DiscordEmbedBuilder()
                .WithTitle("Random Fun Fact")
                .WithDescription(randomFact);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(factEmbed));
        }

    }
}

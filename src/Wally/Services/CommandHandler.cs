﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Wally.Messages;
using Wally.Extensions;
// up

namespace Wally.Services
{
    public class CommandHandler
    {
        // setup fields to be set later in the constructor
        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private List<ulong> _adminUsers = new List<ulong>() { 449229759055003659 };
        private List<ulong> essiam = new List<ulong>(){ 721696110657142814 , 678979054778449968, 569120640888733696 };

        public CommandHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // take action when we execute a command
            _commands.CommandExecuted += CommandExecutedAsync;

            // take action when we receive a message (so we can process it, and see if it is a valid command)
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        // this class is where the magic starts, and takes actions upon receiving messages
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            // sets the argument position away from the prefix we set
            var argPos = 0;

            // get prefix from the configuration file
            char prefix = Char.Parse(_config["Prefix"]);
            #region Manage static messages
            if (new Random().Next(0,3) == 1)
            {
                if (essiam.Contains(message.Author.Id))
                {
                    await message.Channel.SendMessageAsync(EsiamMessages.messages.RandomItem());
                }
            }
            if (message.Content.Equals("4"))
            {
                if (_adminUsers.Contains(message.Author.Id))
                    await message.Channel.SendMessageAsync("مطبعة");
                else
                    await message.Channel.SendMessageAsync("طيزك مربعة");
                return;
            }
            else if (message.Content.Equals("اربعه"))
            {
                await message.Channel.SendMessageAsync("هيخوهيخو طيزك مربعة بردوا");
                return;
            }
            #endregion
            // determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                return;
            }
            var context = new SocketCommandContext(_client, message);

            // execute command if one is found that matches
            
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"Command failed to execute for [] <-> []!");
                return;
            }


            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [] executed for -> []");
                return;
            }


            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Sorry, ... something went wrong -> []!");
        }
    }
}

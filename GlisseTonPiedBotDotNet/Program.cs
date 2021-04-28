﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GlisseTonPiedBot
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;


        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug


            });

            

            commands = new CommandService();

            client.Log += Log;

            client.Ready += () =>
            {
                Console.WriteLine("Je suis prêt");
                return Task.CompletedTask;
            };
            await InstallCommandAsync();

            await client.LoginAsync(TokenType.Bot, "ODI5Njk5NzIzODI0MzMyODU0.YG78FQ.wazRWXqMYB7NKeCgjNHRPkmaEJ4");
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task InstallCommandAsync()
        {
            
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage pMessage)
        {
            var message = (SocketUserMessage)pMessage;


            if (message == null)
            {
                return;
            }

            int argPos = 0;

            if(!message.HasCharPrefix('%', ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(client, message);
            var result = await commands.ExecuteAsync(context, argPos, null);

            //ERROR
            if (!result.IsSuccess) 
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

       /* private Task Getheure()
        {
            DateTime dt = DateTime.Now;
                Console.WriteLine(dt.ToString("HH:mm:ss") + "sdfdfssdffds");
            return Task.CompletedTask;
        }*/
    }
}

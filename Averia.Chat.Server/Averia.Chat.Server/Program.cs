using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Averia.Core.Domain;
using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Averia.Chat.Server
{
    using Averia.Core.Domain.Server.Commands.Handlers;
    using Averia.Storage.Entity;
    using Averia.Transport.Server;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    internal class Program
    {
        private static IConfiguration Configuration { get; set; }

        private static TcpChatServer TcpChatServer { get; set; }

        private static WsChatServer WsChatServer { get; set; }

        private static ICommandDispather CommandDispather { get; set; }

        private static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    serviceCollection =>
                        {
                            serviceCollection.AddDbContext<ChatContext>(w => w.UseInMemoryDatabase("Averia.Chat"));
                        })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(
                    containerBuilder =>
                        {
                            ConfigureCommands(containerBuilder);
                            ConfigureSettings(containerBuilder);
                            ConfigureTcpChatServer(containerBuilder);
                        }))
                .Build();

            ConfigureJson();

            var autofacRoot = host.Services.GetAutofacRoot();
            CommandDispather = autofacRoot.Resolve<ICommandDispather>();

            Console.WriteLine("Start tcp socket chat server");
            TcpChatServer = autofacRoot.Resolve<TcpChatServer>();
            TcpChatServer.Start();
            Console.WriteLine("Tcp socket server started");

            Console.WriteLine("Start ws socket chat server");
            WsChatServer = autofacRoot.Resolve<WsChatServer>();
            WsChatServer.Start();
            Console.WriteLine("Tcp ws server started");

            WaitCommand();
        }

        private static void WaitCommand()
        {
            Console.WriteLine("Write 'exit' to stop the server or 'ls' to get the server connections");

            var waitCommand = true;
            while (waitCommand)
            {
                var command = Console.ReadLine();
                if (!string.IsNullOrEmpty(command))
                {
                    CommandDispather.Execute(new ConsoleCommand(command));
                    if (command.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) waitCommand = false;
                }
            }
        }

        private static void ConfigureCommands(ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispather>().SingleInstance();

            builder.RegisterType<ConsoleCommandHandler>().As<ICommandHandler<ConsoleCommand>>();
            builder.RegisterType<SessionCommandHandler>().As<ICommandHandler<CreateSession>>();
            builder.RegisterType<SessionCommandHandler>().As<ICommandHandler<DeleteSession>>();

            builder.RegisterType<AuthCommandHandler>().As<ICommandHandler<AuthUser>>();
            builder.RegisterType<UserMessageCommandHandler>().As<ICommandHandler<AddMessage>>();
            builder.RegisterType<UserMessageCommandHandler>().As<ICommandHandler<SendExistingMessages>>();
        }

        private static void ConfigureSettings(ContainerBuilder builder)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            builder.RegisterInstance(Configuration);
        }

        private static void ConfigureTcpChatServer(ContainerBuilder builder)
        {
            builder.RegisterType<TcpChatServer>().SingleInstance();
            builder.RegisterType<WsChatServer>().SingleInstance();
        }

        private static void ConfigureJson()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
                                                    {
                                                        TypeNameHandling = TypeNameHandling.All
                                                    };
        }
    }
}

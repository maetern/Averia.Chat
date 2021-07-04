using System;
using System.Net;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Averia.Core.Domain;
using Averia.Core.Domain.Commands;
using Averia.Core.Domain.Interfaces;
using Averia.Core.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Averia.Chat.Server
{
    using Averia.Core.Domain.Server.Commands.Handlers;

    internal class Program
    {
        private static IConfiguration Configuration { get; set; }

        private static TcpChatServer ChatServer { get; set; }

        private static ICommandDispather CommandDispather { get; set; }

        private static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    serviceCollection =>
                        {
                            serviceCollection.AddMemoryCache();
                        })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(
                    containerBuilder =>
                        {
                            ConfigureCommands(containerBuilder);
                            ConfigureSettings(containerBuilder);
                            ConfigureTcpChatServer(containerBuilder);
                        }))
                .Build();

            var autofacRoot = host.Services.GetAutofacRoot();
            CommandDispather = autofacRoot.Resolve<ICommandDispather>();

            Console.WriteLine("Start tcp socket chat server");
            ChatServer = autofacRoot.Resolve<TcpChatServer>();
            ChatServer.Start();
            Console.WriteLine("Tcp socket server started");

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
        }

        private static void ConfigureSettings(ContainerBuilder builder)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            builder.RegisterInstance(Configuration);
        }

        private static void ConfigureTcpChatServer(ContainerBuilder builder)
        {
            builder.RegisterType<TcpChatServer>().SingleInstance();
        }
    }
}

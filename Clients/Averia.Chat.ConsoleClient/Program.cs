using Autofac;
using Autofac.Extensions.DependencyInjection;
using Averia.Core.Domain;
using Averia.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Averia.Core.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Averia.Chat.ConsoleClient
{
    public class Program
    {
        private static IConfiguration Configuration { get; set; }

        private static ICommandDispather CommandDispather { get; set; }

        private static TcpChatClient ChatClient { get; set; }

        private static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(
                    containerBuilder =>
                        {
                            ConfigureCommands(containerBuilder);
                            ConfigureSettings(containerBuilder);
                            ConfigureTcpChatClient(containerBuilder);
                        }))
                .Build();

            var autofacRoot = host.Services.GetAutofacRoot();
            CommandDispather = autofacRoot.Resolve<ICommandDispather>();
            ChatClient = autofacRoot.Resolve<TcpChatClient>();
            ChatClient.Connect();

            Console.ReadLine();

            // Disconnect the client
            Console.Write("Client disconnecting...");
            ChatClient.DisconnectAndStop();
            Console.WriteLine("Done!");

            WaitCommand();
        }

        private static void WaitCommand()
        {
            Console.WriteLine("Write username to sign in");

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
        }

        private static void ConfigureSettings(ContainerBuilder builder)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            builder.RegisterInstance(Configuration);
        }

        private static void ConfigureTcpChatClient(ContainerBuilder builder)
        {
            builder.RegisterType<TcpChatClient>().SingleInstance();
        }
    }
}

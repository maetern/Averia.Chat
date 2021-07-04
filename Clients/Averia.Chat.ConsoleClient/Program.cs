using Autofac;
using Autofac.Extensions.DependencyInjection;
using Averia.Core.Domain;
using Averia.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Averia.Chat.ConsoleClient
{
    using Averia.Core.Domain.Client.Commands.Handlers;
    using Averia.Core.Domain.Commands;
    using Averia.Transport.Client;

    using Newtonsoft.Json;

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

            ConfigureJson();

            var autofacRoot = host.Services.GetAutofacRoot();
            CommandDispather = autofacRoot.Resolve<ICommandDispather>();
            ChatClient = autofacRoot.Resolve<TcpChatClient>();
            ChatClient.ConnectAsync();

            SignIn();
            WaitCommand();
        }

        private static void SignIn()
        {
            Console.WriteLine("Write username to sign in");
            var userName = Console.ReadLine();
            CommandDispather.Execute(new SignIn(userName));
        }

        private static void WaitCommand()
        {
            Console.WriteLine("Write message to send to everyone or write $exit to disconnect");
            var waitCommand = true;
            while (waitCommand)
            {
                var command = Console.ReadLine();
                if (!string.IsNullOrEmpty(command))
                {
                    CommandDispather.Execute(new ConsoleCommand(command));
                    if (command.Equals("$exit", StringComparison.CurrentCultureIgnoreCase)) waitCommand = false;
                }
            }
        }

        private static void ConfigureCommands(ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispather>().SingleInstance();

            builder.RegisterType<ConsoleCommandHandler>().As<ICommandHandler<ConsoleCommand>>();
            builder.RegisterType<SigInCommandHandler>().As<ICommandHandler<SignIn>>();
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

        private static void ConfigureJson()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
                                                    {
                                                        TypeNameHandling = TypeNameHandling.All
                                                    };
        }
    }
}

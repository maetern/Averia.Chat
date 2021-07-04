using System;
using Averia.Core.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Averia.Core.Domain
{
    public sealed class CommandDispatcher : ICommandDispather
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public void Execute<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            handler.Execute(command);
        }
    }
}
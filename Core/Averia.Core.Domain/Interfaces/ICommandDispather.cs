namespace Averia.Core.Domain.Interfaces
{
    public interface ICommandDispather
    {
        void Execute<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
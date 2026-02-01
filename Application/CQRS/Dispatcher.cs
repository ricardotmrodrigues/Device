using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // execute a command wthtout return value
        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command, cancellationToken);
        }

        // execute a command with return value (TResult)
        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand<TResult>
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
            return await handler.HandleAsync(command, cancellationToken);
        }

        // execute a query with return value (TResult)
        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var queryType = query.GetType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

            var handler = _serviceProvider.GetRequiredService(handlerType);
            var handleMethod = handlerType.GetMethod("HandleAsync");

            var task = (Task<TResult>)handleMethod!.Invoke(handler, [query, cancellationToken])!;
            return await task;
        }
    }
}

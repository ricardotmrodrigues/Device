using Application.CQRS;
using FluentValidation;

namespace Application.Behaviors;

public class ValidationCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;
    private readonly IValidator<TCommand>? _validator;

    public ValidationCommandHandler(ICommandHandler<TCommand> inner, IValidator<TCommand>? validator = null)
    {
        _inner = inner;
        _validator = validator;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        if (_validator is not null)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                throw new Exceptions.ValidationException(errors);
            }
        }
        await _inner.HandleAsync(command, cancellationToken);
    }
}

public class ValidationCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;
    private readonly IValidator<TCommand>? _validator;

    public ValidationCommandHandler(ICommandHandler<TCommand, TResult> inner, IValidator<TCommand>? validator = null)
    {
        _inner = inner;
        _validator = validator;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        if (_validator is not null)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                throw new Exceptions.ValidationException(errors);
            }
        }
        return await _inner.HandleAsync(command, cancellationToken);
    }
}

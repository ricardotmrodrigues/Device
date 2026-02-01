namespace Application.CQRS;

//simple command
public interface ICommand
{
}

//command with return value
public interface ICommand<TResult> : ICommand
{
}
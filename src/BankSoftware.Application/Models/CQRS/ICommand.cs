using MediatR;

namespace BankSoftware.Application.Models.CQRS
{
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}

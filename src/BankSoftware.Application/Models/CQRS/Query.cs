using MediatR;

namespace BankSoftware.Application.Models.CQRS
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}

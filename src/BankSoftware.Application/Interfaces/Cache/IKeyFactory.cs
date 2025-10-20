using MediatR;

namespace BankSoftware.Application.Interfaces.Cache
{
    public interface IKeyFactory
    {
        string Key(IBaseRequest request);
    }
}

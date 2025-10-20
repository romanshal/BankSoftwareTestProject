using BankSoftware.Application.Interfaces.Cache;
using MediatR;

namespace BankSoftware.Infrastructure.Cache
{
    internal class RedisKeyFactory : IKeyFactory
    {
        public string Key(IBaseRequest request)
        {
            return $"{request.GetType().Name}-{request.GetHashCode()}";
        }
    }
}

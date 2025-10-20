using BankSoftware.Domain.Constants.Errors;
using System.Text.Json.Serialization;

namespace BankSoftware.Application.Models
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        [JsonConstructor]
        protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error) =>
            _value = value;

        public T? Value => _value;

        public static implicit operator Result<T>(T? value) => Create(value);
    }
}

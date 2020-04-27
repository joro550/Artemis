using Microsoft.Extensions.Options;

namespace Artemis.Web.Server.Tests
{
    public class OptionsImpl<T> : IOptions<T> where T : class, new()
    {
        private readonly T _value;

        public T Value => _value;

        public OptionsImpl(T value)
        {
            _value = value;
        }
    }
}
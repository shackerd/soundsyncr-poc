using Spectre.Console.Cli;

namespace Midicontrol.Infrastructure
{
    internal sealed class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public TypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public object Resolve(Type? type)
        {
#pragma warning disable CS8603
            if (type == null)
            {
                return null;
            }
            return _provider.GetService(type);
#pragma warning restore CS8603
        }

        public void Dispose()
        {
            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
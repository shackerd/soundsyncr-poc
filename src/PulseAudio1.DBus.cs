using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace PulseAudio1.DBus
{
    [DBusInterface("org.PulseAudio.ServerLookup1")]
    interface IServerLookup1 : IDBusObject
    {
        Task<T> GetAsync<T>(string prop);
        Task<ServerLookup1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class ServerLookup1Properties
    {
        private string _Address = default(string);
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = (value);
            }
        }
    }

    static class ServerLookup1Extensions
    {
        public static Task<string> GetAddressAsync(this IServerLookup1 o) => o.GetAsync<string>("Address");
    }
}
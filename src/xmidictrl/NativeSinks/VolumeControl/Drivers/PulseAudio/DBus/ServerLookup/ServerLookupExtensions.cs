using System;
using System.Collections.Generic;
namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    static class ServerLookupExtensions
    {
        public static Task<string> GetAddressAsync(this IServerLookupProxy o) => o.GetAsync<string>("Address");
    }
}
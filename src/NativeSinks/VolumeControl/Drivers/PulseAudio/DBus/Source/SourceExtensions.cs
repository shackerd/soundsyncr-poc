using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace Midicontrol.PulseAudio.DBus
{
    static class SourceExtensions
    {
        public static Task<ObjectPath> GetMonitorOfSinkAsync(this ISource o) => o.GetAsync<ObjectPath>("MonitorOfSink");
    }
}
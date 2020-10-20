

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octolamp.Contracts.Settings
{
    public class BackendSettings
    {
        public string Address { get; set; }
    }

    public class AzureSettings
    {
        public SignalRSettings SignalR { get; set; }
    }

    public class SignalRSettings
    {
        public string ConnectionString { get; set; }
    }
}

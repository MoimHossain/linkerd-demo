using Grpc.Net.Client;

using System;
using System.Threading.Tasks;

namespace Octolamp.DaemonService
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceUri = Environment.GetEnvironmentVariable("MathBackendUri") ?? "https://172.18.0.2:443";

            
        }
    }
}

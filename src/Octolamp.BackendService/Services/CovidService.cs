


using Grpc.Core;
using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Protos;
using System;
using System.Threading.Tasks;

namespace Octolamp.BackendService.Services
{
    public class CovidService : Covid.CovidBase
    {
        private readonly ILogger<CovidService> _logger;

        public CovidService(ILogger<CovidService> logger)
        {
            _logger = logger;
        }

        public async override Task<HandshakeResponse> DoHandshake(HandshakeRequest request, ServerCallContext context)
        {
            await Task.CompletedTask;
            return new HandshakeResponse 
            {
                ClientToken = request.ClientToken,
                ServerToken = DateTime.Now.ToLongTimeString()
            };
        }
    }
}

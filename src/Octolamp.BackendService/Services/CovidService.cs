


using Grpc.Core;
using Microsoft.Extensions.Logging;
using Octolamp.BackendService.Data;
using Octolamp.Contracts.Protos;
using System;
using System.Threading.Tasks;

namespace Octolamp.BackendService.Services
{
    public class CovidService : Covid.CovidBase
    {
        private readonly CovidRepository _repository;
        private readonly ILogger<CovidService> _logger;

        public CovidService(CovidRepository repository, ILogger<CovidService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async override Task<HandshakeResponse> DoHandshake(
            HandshakeRequest request, ServerCallContext context)
        {
            await Task.CompletedTask;
            return new HandshakeResponse 
            {
                ClientToken = request.ClientToken,
                ServerToken = DateTime.Now.ToLongTimeString()
            };
        }

        public async override Task<HandshakeResponse> RegisterReportSummary(
            CovidGlobalReport request, ServerCallContext context)
        {
            await _repository.RegisterGlobalAsync(request);
            return new HandshakeResponse
            {
                ClientToken = DateTime.Now.ToLongTimeString(),
                ServerToken = DateTime.Now.ToLongTimeString()
            };
        }

        public async override Task<HandshakeResponse> RegisterCountryReport(
            CovidCountryReport request, ServerCallContext context)
        {
            await _repository.RegisterCountryAsync(request);
            return new HandshakeResponse
            {
                ClientToken = DateTime.Now.ToLongTimeString(),
                ServerToken = DateTime.Now.ToLongTimeString()
            };
        }
    }
}

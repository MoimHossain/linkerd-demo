using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Grpc.Core;
using Microsoft.Extensions.Logging;
using Octolamp.BackendService.Data;
using Octolamp.Contracts.Protos;

namespace Octolamp.BackendService.Services
{
    public class StockService : Stocks.StocksBase
    {
        private static readonly string ServerId = Guid.NewGuid().ToString("D");
        
        private readonly IStockRepository _repository;
        private readonly ILogger<StockService> _logger;
        
        public StockService(IStockRepository repository, ILogger<StockService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<GetResponse> Get(GetRequest request, ServerCallContext context)
        {
            if (request.Id < 1)
            {
                _logger.LogWarning("Invalid Stock ID received: {stockId}", request.Id);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid argument"), "Id must be greater than 1");
            }

            var stock = await _repository.GetAsync(request.Id);

            if (stock is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
            }

            var headers = new Metadata
            {
                {"server-id", ServerId}
            };

            await context.WriteResponseHeadersAsync(headers);
            
            return new GetResponse
            {
                Stock = new Contracts.Protos.Stock
                {
                    Id = stock.Id,
                    Symbol = stock.Symbol,
                    Name = stock.Name
                }
            };
        }
    }
}

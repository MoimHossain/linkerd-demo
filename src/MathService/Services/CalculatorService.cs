

using Grpc.Core;
using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathService.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        private readonly ILogger<CalculatorService> logger;

        public CalculatorService(ILogger<CalculatorService> logger)
        {
            this.logger = logger;
        }

        public async override Task<UnaryResult> Add(BinaryOperands request, ServerCallContext context)
        {
            await Task.CompletedTask;
            return new UnaryResult { Result = request.Operand1 + request.Operand2 };
        }
    }
}

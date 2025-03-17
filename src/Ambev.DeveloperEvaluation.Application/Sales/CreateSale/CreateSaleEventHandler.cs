using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Interfaces;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleEventHandler : IEventHandler<SaleCreatedEvent>
    {
        private readonly ISaleReadRepository _saleReadRepository;
        private readonly ILogger<CreateSaleEventHandler> _logger;

        public CreateSaleEventHandler(ISaleReadRepository saleReadRepository, ILogger<CreateSaleEventHandler> logger)
        {
            _saleReadRepository = saleReadRepository;
            _logger = logger;
        }

        public async Task HandleAsync(SaleCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var saleDoc = @event.Sale;
                await _saleReadRepository.InsertAsync(saleDoc);
                _logger.LogInformation($"Created MongoDB document for sale {saleDoc.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating MongoDB document for sale {@event.Id}");
                throw;
            }
        }
    }
}

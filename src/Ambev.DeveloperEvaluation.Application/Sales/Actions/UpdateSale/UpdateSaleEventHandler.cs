using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;

public class UpdateSaleEventHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly ILogger<UpdateSaleEventHandler> _logger;

    public UpdateSaleEventHandler(ISaleReadRepository saleReadRepository, ILogger<UpdateSaleEventHandler> logger)
    {
        _saleReadRepository = saleReadRepository;
        _logger = logger;
    }


    public async Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var saleDoc = notification.Sale;
            await _saleReadRepository.ReplaceAsync(saleDoc);
            _logger.LogInformation($"Updated MongoDB document for sale {saleDoc.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating MongoDB document for sale {notification.Sale.Id}");
            throw;
        }
    }
}

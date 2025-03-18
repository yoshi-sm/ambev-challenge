using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CancelSaleItemEventHandler : INotificationHandler<ItemCancelledEvent>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly ILogger<CancelSaleItemEventHandler> _logger;

    public CancelSaleItemEventHandler(ISaleReadRepository saleReadRepository, ILogger<CancelSaleItemEventHandler> logger)
    {
        _saleReadRepository = saleReadRepository;
        _logger = logger;
    }

    public async Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var saleDoc = notification.SaleDocument;
            await _saleReadRepository.ReplaceAsync(saleDoc);
            _logger.LogInformation($"Replaced old sale MongoDB document for new version with same SaleId: {saleDoc.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error replacing old sale for new sale of SaleId: {notification.SaleDocument.Id}");
            throw;
        }
    }
}

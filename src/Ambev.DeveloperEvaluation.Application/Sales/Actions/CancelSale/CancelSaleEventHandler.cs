using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.CancelSale;

public class CancelSaleEventHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly ILogger<CancelSaleEventHandler> _logger;

    public CancelSaleEventHandler(ISaleReadRepository saleReadRepository, ILogger<CancelSaleEventHandler> logger)
    {
        _saleReadRepository = saleReadRepository;
        _logger = logger;
    }

    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var saleDoc = notification.Sale;
            await _saleReadRepository.ReplaceAsync(saleDoc);
            _logger.LogInformation($"Replaced old sale MongoDB document for new version with same SaleId: {saleDoc.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error replacing old sale for new sale of SaleId: {notification.Sale.Id}");
            throw;
        }
    }
}

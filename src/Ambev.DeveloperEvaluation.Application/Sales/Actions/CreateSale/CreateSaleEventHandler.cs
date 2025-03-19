using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;

public class CreateSaleEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly ILogger<CreateSaleEventHandler> _logger;

    public CreateSaleEventHandler(ISaleReadRepository saleReadRepository, ILogger<CreateSaleEventHandler> logger)
    {
        _saleReadRepository = saleReadRepository;
        _logger = logger;
    }


    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var saleDoc = notification.Sale;
            await _saleReadRepository.InsertAsync(saleDoc);
            _logger.LogInformation($"Created MongoDB document for sale {saleDoc.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating MongoDB document for sale {notification.Sale.Id}");
            throw;
        }
    }
}

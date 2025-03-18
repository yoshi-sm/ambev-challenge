using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, SaleResult>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly ISaleWriteRepository _saleWriteRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _eventPublisher;

    public CancelSaleItemHandler(ISaleReadRepository saleReadRepository, 
        ISaleWriteRepository saleWriteRepository, IMapper mapper, IPublisher eventPublisher)
    {
        _saleReadRepository = saleReadRepository;
        _saleWriteRepository = saleWriteRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var mongoDoc = await _saleReadRepository.GetSaleByItemIdAsync(request.ItemId);
        if (mongoDoc == null)
            return SaleResult.Failure([], StatusCodes.Status404NotFound);

        var sale = _mapper.Map<Sale>(mongoDoc);

        sale.Items.First(x => x.Id == request.ItemId).Cancel();
        sale.CalculateTotalAmount();

        await _saleWriteRepository.UpdateAsync(sale);
        var saleDocument = _mapper.Map<SaleDocument>(sale);
        await _eventPublisher.Publish(new ItemCancelledEvent(saleDocument));
        return SaleResult.Ok(StatusCodes.Status200OK, saleDocument);
    }
}

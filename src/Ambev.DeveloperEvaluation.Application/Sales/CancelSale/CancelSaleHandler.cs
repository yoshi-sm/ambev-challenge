using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, SaleResult>
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly ISaleWriteRepository _saleWriteRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _eventPublisher;

    public CancelSaleHandler(ISaleReadRepository saleReadRepository, 
        ISaleWriteRepository saleWriteRepository, IMapper mapper, IPublisher eventPublisher)
    {
        _saleReadRepository = saleReadRepository;
        _saleWriteRepository = saleWriteRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var mongoDoc = await _saleReadRepository.GetByIdAsync(request.Id);
        if (mongoDoc == null)
            return SaleResult.Failure([], StatusCodes.Status404NotFound);

        var sale = _mapper.Map<Sale>(mongoDoc);
        sale.Cancel();
        await _saleWriteRepository.UpdateAsync(sale);
        var saleDocument = _mapper.Map<SaleDocument>(sale);
        await _eventPublisher.Publish(new SaleCancelledEvent(saleDocument));
        return SaleResult.Ok(StatusCodes.Status200OK, saleDocument);
    }
}

using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, SaleResult<SaleDocument>>
{
    private readonly ISaleWriteRepository _writeRepository;
    private readonly ISaleReadRepository _readRepository;
    private readonly ISaleDataService _saleDataService;
    private readonly IMapper _mapper;
    private readonly IPublisher _eventPublisher;

    public UpdateSaleHandler(ISaleWriteRepository writeRepository, ISaleReadRepository readRepository, ISaleDataService saleDataService, IMapper mapper, IPublisher eventPublisher)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _saleDataService = saleDataService;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult<SaleDocument>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var mongoDoc = await _readRepository.GetByIdAsync(request.Id);
        if (mongoDoc == null)
            return SaleResult<SaleDocument>.Failure([], StatusCodes.Status404NotFound);

        var sale = _mapper.Map<Sale>(mongoDoc);
        var saleUpdateResult = await _saleDataService.UpdateSaleAsync(request, sale);
        
        
        if(!saleUpdateResult.IsValid)
            return SaleResult<SaleDocument>.Failure(saleUpdateResult.ValidationResult.Errors, 
                StatusCodes.Status400BadRequest);

        var updatedSale = saleUpdateResult.Sale;
        await _writeRepository.UpdateSaleCancelItemsAsync(updatedSale);
        
        var saleDocument = _mapper.Map<SaleDocument>(updatedSale);
        await _eventPublisher.Publish(new SaleModifiedEvent(saleDocument));

        return SaleResult<SaleDocument>.Ok(StatusCodes.Status200OK, saleDocument);
    }
}

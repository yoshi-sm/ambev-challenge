using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Services;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResult<SaleDocument>>
{
    private readonly ISaleWriteRepository _writeRepository;
    private readonly ISaleDataService _saleDataService;
    private readonly IMapper _mapper;
    private readonly IPublisher _eventPublisher;

    public CreateSaleHandler(ISaleWriteRepository writeRepository, ISaleDataService saleDataService, IMapper mapper, IPublisher eventPublisher)
    {
        _writeRepository = writeRepository;
        _saleDataService = saleDataService;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult<SaleDocument>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var saleResult = await _saleDataService.CreateSaleAsync(request);

        if (!saleResult.IsValid)
            return SaleResult<SaleDocument>.Failure(saleResult.ValidationResult.Errors, StatusCodes.Status400BadRequest);

        var sale = saleResult.Sale;
        await _writeRepository.CreateAsync(sale);

        var saleDocument = _mapper.Map<SaleDocument>(sale);
        await _eventPublisher.Publish(new SaleCreatedEvent(saleDocument));

        return SaleResult<SaleDocument>.Ok(StatusCodes.Status201Created, saleDocument);
    }
}


using Ambev.DeveloperEvaluation.Application.Sales.Common;
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
    private readonly IFakeRepository _fakeRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _eventPublisher;

    public CreateSaleHandler(ISaleWriteRepository writeRepository, IMapper mapper, IFakeRepository fakeRepository, IPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
        _writeRepository = writeRepository;
        _mapper = mapper;
        _fakeRepository = fakeRepository;
    }


    public async Task<SaleResult<SaleDocument>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var (sale, validation) = GenerateSale(request);
        if (!validation.IsValid)
            return SaleResult<SaleDocument>.Failure(validation.Errors, StatusCodes.Status400BadRequest);

        await _writeRepository.CreateAsync(sale);

        var saleDocument = _mapper.Map<SaleDocument>(sale);
        await _eventPublisher.Publish(new SaleCreatedEvent(saleDocument));

        return SaleResult<SaleDocument>.Ok(StatusCodes.Status201Created, saleDocument);
    }

    private (Sale, ValidationResult) GenerateSale(CreateSaleCommand request)
    {
        var productIds = request.Items.Select(x => x.ProductId);
        var (customer, branch, products) = GetExternalData(request.CustomerId, request.BranchId, productIds);
        var sale = _mapper.Map<Sale>((request, customer, branch));
        var saleItems = MapSaleItems(request.Items, products, sale.Id);
        var validation = sale.SetSale(saleItems);
        return (sale, validation);
    }

    private (CustomerInfo, BranchInfo, IEnumerable<ProductInfo>) GetExternalData(Guid customerId, Guid branchId,
        IEnumerable<Guid> productIds)
    {
        var customer = _fakeRepository.GetCustomerById(customerId);
        var branch = _fakeRepository.GetBranchById(branchId);
        var products = _fakeRepository.GetProductsByIds(productIds);
        return (customer, branch, products);
    }

    private List<SaleItem> MapSaleItems(List<CreateSaleItemCommand> commandItems, IEnumerable<ProductInfo> products, Guid id)
    {
        List<SaleItem> saleItems = new();
        foreach (var commandItem in commandItems)
        {
            var product = products.FirstOrDefault(p => p.Id == commandItem.ProductId);
            if (product == null) continue;
            var saleItem = _mapper.Map<SaleItem>((commandItem, product));
            saleItems.Add(saleItem);
        }
        return saleItems;
    }
}


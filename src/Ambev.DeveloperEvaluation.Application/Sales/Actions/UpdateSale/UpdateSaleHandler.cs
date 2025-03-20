using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
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
    private readonly IFakeRepository _fakeRepository;
    private readonly IMapper _mapper;
    private readonly IPublisher _eventPublisher;

    public UpdateSaleHandler(ISaleWriteRepository writeRepository, ISaleReadRepository readRepository,
        IFakeRepository fakeRepository, IMapper mapper, IPublisher eventPublisher)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _fakeRepository = fakeRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResult<SaleDocument>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var mongoDoc = await _readRepository.GetByIdAsync(request.Id);
        if (mongoDoc == null)
            return SaleResult<SaleDocument>.Failure([], StatusCodes.Status404NotFound);

        var sale = _mapper.Map<Sale>(mongoDoc);
        var (newSale, validation) = UpdateSale(request, sale);
        if (!validation.IsValid)
            return SaleResult<SaleDocument>.Failure(validation.Errors, StatusCodes.Status400BadRequest);

        var oldItems = sale.Items.ToList();
        var newItems = newSale.Items.ToList();
        _mapper.Map(newSale, sale);
        await _writeRepository.UpdateSaleCancelItemsAsync(sale, oldItems, newItems);
        var saleDocument = _mapper.Map<SaleDocument>(sale);
        await _eventPublisher.Publish(new SaleModifiedEvent(saleDocument));

        return SaleResult<SaleDocument>.Ok(StatusCodes.Status200OK, saleDocument);
    }

    private (Sale, ValidationResult) UpdateSale(UpdateSaleCommand request, Sale oldSale)
    {
        var productIds = request.Items.Select(x => x.ProductId);
        var (customer, branch, products) = GetExternalData(request.CustomerId, request.BranchId, productIds);

        var sale = _mapper.Map<Sale>((request, customer, branch));
        var saleItems = MapSaleItems(request.Items, products);

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

    private List<SaleItem> MapSaleItems(List<CreateSaleItemCommand> commandItems, IEnumerable<ProductInfo> products)
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

using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Interfaces;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleWriteRepository _writeRepository;
    private readonly IFakeRepository _fakeRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public CreateSaleHandler(ISaleWriteRepository writeRepository, IMapper mapper, IFakeRepository fakeRepository, IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
        _writeRepository = writeRepository;
        _mapper = mapper;
        _fakeRepository = fakeRepository;
    }


    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = GenerateSale(request);
        await _writeRepository.CreateAsync(sale);

        var saleDocument = _mapper.Map<SaleDocument>(sale);
        await _eventPublisher.PublishAsync(new SaleCreatedEvent(saleDocument));

        return _mapper.Map<CreateSaleResult>(sale);
    }

    private Sale GenerateSale(CreateSaleCommand request)
    {
        var productIds = request.Items.Select(x => x.ProductId);
        var (customer, branch, products) = GetExternalData(request.CustomerId, request.BranchId, productIds);
        var sale = _mapper.Map<Sale>((request, customer, branch));
        var saleItems = MapSaleItems(request.Items, products, sale.Id);
        sale.SetSale(saleItems);

        return sale;
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


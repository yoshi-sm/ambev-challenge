using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Dto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Services;

public class SaleDataService : ISaleDataService
{
    private readonly IFakeRepository _fakeRepository;
    private readonly IMapper _mapper;

    public SaleDataService(IFakeRepository fakeRepository, IMapper mapper)
    {
        _fakeRepository = fakeRepository;
        _mapper = mapper;
    }

    public async Task<SaleDataResult> CreateSaleAsync(CreateSaleCommand command)
    {
        var productIds = command.Items.Select(x => x.ProductId);
        var (customer, branch, products) = await GetExternalDataAsync(
            command.CustomerId,
            command.BranchId,
            productIds);

        var sale = _mapper.Map<Sale>((command, customer, branch));
        var saleItems = MapSaleItems(command.Items, products);

        var validationResult = sale.SetSale(saleItems);
        if (!validationResult.IsValid)
        {
            return SaleDataResult.Failure(sale, validationResult);
        }

        return SaleDataResult.Success(sale);
    }

    public async Task<SaleDataResult> UpdateSaleAsync(UpdateSaleCommand command, Sale existingSale)
    {
        foreach (var item in existingSale.Items)
            item.Cancel();

        var productIds = command.Items.Select(x => x.ProductId);
        var (customer, branch, products) = await GetExternalDataAsync(
            command.CustomerId, 
            command.BranchId, 
            productIds);

        var updatedSale = _mapper.Map<Sale>((command, customer, branch));
        var saleItems = MapSaleItems(command.Items, products);
        
        var validationResult = updatedSale.SetSale(saleItems);
        if (!validationResult.IsValid)
        {
            return SaleDataResult.Failure(updatedSale, validationResult);
        }

        _mapper.Map(updatedSale, existingSale);
        return SaleDataResult.Success(existingSale);
    }

    private async Task<(CustomerInfo, BranchInfo, IEnumerable<ProductInfo>)> GetExternalDataAsync(
        Guid customerId, 
        Guid branchId,
        IEnumerable<Guid> productIds)
    {
        Task<CustomerInfo> customerTask = Task.Run(() => _fakeRepository.GetCustomerById(customerId));
        Task<BranchInfo> branchTask = Task.Run(() => _fakeRepository.GetBranchById(branchId));
        Task<IEnumerable<ProductInfo>> productsTask = Task.Run(() => _fakeRepository.GetProductsByIds(productIds));

        await Task.WhenAll(customerTask, branchTask, productsTask);

        return (customerTask.Result, branchTask.Result, productsTask.Result);
    }

    private List<SaleItem> MapSaleItems(List<SaleItemDto> commandItems, IEnumerable<ProductInfo> products)
    {
        var saleItems = new List<SaleItem>();
        
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

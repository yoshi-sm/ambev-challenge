using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<(CreateSaleCommand Command, CustomerInfo Customer, BranchInfo Branch), Sale>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Command.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Command.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<(CreateSaleItemCommand Command, ProductInfo Product), SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Command.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.UnitPrice))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Command.Quantity))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => false));

        CreateMap<Sale, SaleDocument>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new CustomerInfo
            {
                Id = src.CustomerId,
                Name = src.CustomerName
            }))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new BranchInfo
            {
                Id = src.BranchId,
                Name = src.BranchName
            }))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleItem, SaleItemDocument>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductInfo
            {
                Id = src.ProductId,
                Name = src.ProductName,
                UnitPrice = src.UnitPrice
            }));

        CreateMap<Sale, CreateSaleResult>();
    }
}

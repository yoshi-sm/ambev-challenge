using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Dto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<Sale, Sale>()
            .ForMember(dest => dest.SaleNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.MapFrom((src, dest) =>
            {
                var allItems = dest.Items.Concat(src.Items.Where(x => !dest.Items.Contains(x))).ToList();
                return allItems;
            }));

        CreateMap<(CreateSaleCommand Command, CustomerInfo Customer, BranchInfo Branch), Sale>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Command.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Command.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<(SaleItemDto Command, ProductInfo Product), SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Command.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.UnitPrice))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Command.Quantity))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => false));

        CreateMap<(UpdateSaleCommand Command, CustomerInfo Customer, BranchInfo Branch), Sale>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Command.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Command.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Command.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<(SaleItemDto Command, ProductInfo Product), SaleItem>()
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

        CreateMap<SaleDocument, Sale>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src =>
                DateTime.SpecifyKind(src.SaleDate, DateTimeKind.Unspecified)))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch.Id))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleItemDocument, SaleItem>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.UnitPrice));

        CreateMap<GetAllSalesQuery, SaleDocumentFilter>();
    }
}

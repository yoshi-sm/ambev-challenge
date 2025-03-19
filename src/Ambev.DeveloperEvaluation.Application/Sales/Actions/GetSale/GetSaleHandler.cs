using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleQuery, SaleResult<SaleDocument>>
{
    private readonly ISaleReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetSaleHandler(ISaleReadRepository readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<SaleResult<SaleDocument>> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await _readRepository.GetByIdAsync(request.Id);
        if (sale == null)
            return SaleResult<SaleDocument>.Failure([], StatusCodes.Status404NotFound);
        return SaleResult<SaleDocument>.Ok(StatusCodes.Status200OK, sale);
    }
}

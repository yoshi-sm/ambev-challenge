using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.GetAllSales;

public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, SaleResult<IEnumerable<SaleDocument>>>
{
    private readonly ISaleReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetAllSalesHandler(ISaleReadRepository readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<SaleResult<IEnumerable<SaleDocument>>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var filter = _mapper.Map<SaleDocumentFilter>(request);
        (IEnumerable<SaleDocument> mongoDocs, long count) = await _readRepository.GetAllAsync(filter);
        if (mongoDocs == null)
            return SaleResult<IEnumerable<SaleDocument>>.Failure([], StatusCodes.Status404NotFound);

        return SaleResult<IEnumerable<SaleDocument>>.Ok(StatusCodes.Status200OK, mongoDocs, count);
    }
}

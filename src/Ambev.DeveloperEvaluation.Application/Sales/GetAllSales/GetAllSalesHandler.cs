using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, SaleResult>
{
    private readonly ISaleReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetAllSalesHandler(ISaleReadRepository readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<SaleResult> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var filter = _mapper.Map<SaleDocumentFilter>(request);
        var mongoDocs = await _readRepository.GetAllAsync(filter);
        if (mongoDocs == null)
            return SaleResult.Failure([], StatusCodes.Status404NotFound);

        return SaleResult.Ok(StatusCodes.Status200OK, mongoDocs);
    }
}

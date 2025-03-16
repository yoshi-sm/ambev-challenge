using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.ReadModels;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleWriteRepository : ISaleWriteRepository
{
    private readonly WriteDbContext _writeContext;
    public SaleWriteRepository(WriteDbContext writeContext)
    {
        _writeContext = writeContext;
    }

    public async Task CreateAsync(Sale sale)
    {
        await _writeContext.Sales.AddAsync(sale);
        await _writeContext.SaveChangesAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _writeContext.Sales.FindAsync(id);
    }

    public async Task UpdateAsync(Sale sale)
    {
        _writeContext.Sales.Update(sale);
        await _writeContext.SaveChangesAsync();
    }
}

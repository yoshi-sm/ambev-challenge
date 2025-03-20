using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Context;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

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
        return await _writeContext.Sales
            .Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Sale?> GetBySaleItemIdAsync(Guid id)
    {
        return await _writeContext.Sales.Include(x => x.Items).FirstOrDefaultAsync(x => x.Items.Select(x => x.Id).Contains(id));
    }

    public async Task UpdateAsync(Sale sale)
    {

        if (sale.Items.Count > 0)
            _writeContext.SaleItems.UpdateRange(sale.Items);
        _writeContext.Sales.Update(sale);
        await _writeContext.SaveChangesAsync();
    }

    public async Task UpdateSaleCancelItemsAsync(Sale sale)
    {
        _writeContext.SaleItems.UpdateRange(sale.Items.Where(x => x.IsCancelled));
        await _writeContext.SaleItems.AddRangeAsync(sale.Items.Where(x => !x.IsCancelled));
        _writeContext.Sales.Update(sale);
        await _writeContext.SaveChangesAsync();
    }
}

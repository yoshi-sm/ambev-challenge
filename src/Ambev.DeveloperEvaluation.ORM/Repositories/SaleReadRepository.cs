using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.ORM.Context;
using Ambev.DeveloperEvaluation.ORM.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleReadRepository : ISaleReadRepository
{
    private readonly ReadDbContext _readContext;
    private readonly IMongoCollection<SaleDocument> _saleDocuments;
    public SaleReadRepository(ReadDbContext readContext)
    {
        _readContext = readContext;
        _saleDocuments = _readContext.GetCollection();
    }

    public async Task<IEnumerable<SaleDocument>> GetAllAsync(SaleDocumentFilter filter)
    {
        var mongoFilter = MongoFilterHelper.CreateFilter(filter);
        int skip = (filter.PageNumber - 1) * filter.PageSize;

        return await _saleDocuments
            .Find(mongoFilter)
            .Skip(skip)
            .Limit(filter.PageSize)
            .ToListAsync();
    }

    public async Task<SaleDocument?> GetByIdAsync(Guid id)
    {
        return await _saleDocuments.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task InsertAsync(SaleDocument entity)
    {
        await _saleDocuments.InsertOneAsync(entity);
    }

    public async Task ReplaceAsync(SaleDocument entity)
    {
        var replaceDoc = Builders<SaleDocument>.Filter.Eq(p => p.Id, entity.Id);
        await _saleDocuments.ReplaceOneAsync(replaceDoc, entity);
    }

    public async Task<SaleDocument?> GetSaleByItemIdAsync(Guid id)
    {
        return await _saleDocuments.Find(x => x.Items.Select(y => y.Id).Contains(id)).FirstOrDefaultAsync();
    }
}

using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    //private readonly WriteDbContext _writeContext;
    //private readonly IMongoCollection<SaleDocument> _salesCollection;
    //private readonly IEventPublisher _eventPublisher;
    //public SaleRepository(
    //    WriteDbContext writeContext,
    //    IMongoDatabase mongoDatabase,
    //    IEventPublisher eventPublisher)
    //{
    //    _dbContext = dbContext;
    //    _salesCollection = mongoDatabase.GetCollection<SaleDocument>("Sales");
    //    _eventPublisher = eventPublisher;
    //}
}

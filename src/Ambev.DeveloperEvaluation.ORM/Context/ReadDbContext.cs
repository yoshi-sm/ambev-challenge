using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.ORM.ReadModels;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Serializers;


namespace Ambev.DeveloperEvaluation.ORM.Context;

public class ReadDbContext
{
    private readonly IMongoDatabase _database;

    public ReadDbContext(IOptions<MongoDbSettings> settings)
    {
        ConfigureMongoMapping();
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<SaleDocument> GetCollection()
    {

        return _database.GetCollection<SaleDocument>("SaleDocuments");
    }

    private void ConfigureMongoMapping()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));

        // Configure domain model mapping
        if (!BsonClassMap.IsClassMapRegistered(typeof(SaleDocument)))
        {
            BsonClassMap.RegisterClassMap<SaleDocument>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id);
            });
        }
    }
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
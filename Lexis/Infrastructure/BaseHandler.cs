using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Driver;

namespace LexisApi.Infrastructure;

public abstract class BaseHandler
{
    #region Data

    protected readonly IMongoDatabase Database;

    #endregion

    #region Constructors

    protected BaseHandler(IMongoClient client, IConfiguration config)
    {
        var databaseName = config.GetValue<string>("ConnectionStrings:DatabaseName");
        Database = client.GetDatabase(databaseName);
    }

    #endregion
}
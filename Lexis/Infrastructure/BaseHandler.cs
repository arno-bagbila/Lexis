using MongoDB.Driver;

namespace LexisApi.Infrastructure;

public abstract class BaseHandler
{
    #region Data

    protected IMongoDatabase Database;

    #endregion

    #region Constructors

    protected BaseHandler(IMongoClient client)
    {
        Database = client.GetDatabase("Lexis");
    }

    #endregion
}
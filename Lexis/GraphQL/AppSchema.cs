using GraphQL.Types;
using LexisApi.GraphQL.Queries;
using LexisApi.GraphQL.Types;

namespace LexisApi.GraphQL;

public class AppSchema : Schema
{
    public AppSchema(LexisQuery query)
    {
        Query = query;
        RegisterType(typeof(ObjectIdGraphType));
    }
}
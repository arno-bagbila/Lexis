using GraphQL.Types;
using MongoDB.Bson;
using GraphQLParser.AST;

namespace LexisApi.GraphQL.Types;

public class ObjectIdGraphType : ScalarGraphType
{
    public ObjectIdGraphType()
    {
        Name = "ObjectId";
        Description = "The `ObjectId` scalar type represents a MongoDB ObjectId.";
    }
    
    public override object? ParseLiteral(GraphQLValue value) =>
        value switch
        {
            GraphQLStringValue stringValue => ParseValue(stringValue.Value),
            _ => null
        };
    
    public override object? ParseValue(object? value) =>
        value switch
        {
            string s => ObjectId.Parse(s),
            ObjectId objectId => objectId,
            _ => null
        };
  
    public override object? Serialize(object? value) =>
        value switch
        {
            ObjectId objectId => objectId.ToString(),
            _ => null
        };
}
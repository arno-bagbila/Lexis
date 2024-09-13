
using GraphQL.Types;
using LexisApi.Models.Output.Users;

namespace LexisApi.GraphQL.Types;

public class UserType: ObjectGraphType<User>
{
    public UserType()
    {
        Field(x => x.Id, type: typeof(StringGraphType)).Description("The unique identifier of the user.");
        Field(x => x.FirstName, type: typeof(StringGraphType)).Description("The first name of the user.");
        Field(x => x.LastName, type: typeof(StringGraphType)).Description("The last name of the user.");
        Field(x => x.PublishedBlogsCount, type: typeof(IntGraphType)).Description("Number of blogs published by the user.");
        Field(x => x.TotalWordsCount, type: typeof(IntGraphType)).Description("Total number of words used by the user.");
    }
}
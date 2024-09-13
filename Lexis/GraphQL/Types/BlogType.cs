using GraphQL.Types;
using LexisApi.Models.Output.Blogs.GraphQL;

namespace LexisApi.GraphQL.Types;

public class BlogType : ObjectGraphType<Blog>
{
    public BlogType()
    {
        Field(x => x.Id, type: typeof(StringGraphType)).Description("The unique identifier of the blog.");
        Field(x => x.Text, type: typeof(StringGraphType)).Description("The text of the blog.");
        Field(x => x.CreatedOn, type: typeof(DateTimeGraphType)).Description("The date and time the blog was created.");
        Field(x => x.PublishedOn, type: typeof(DateTimeGraphType)).Description("The date and time the blog was published.");
        Field(x => x.Category, type: typeof(StringGraphType)).Description("The category of the blog.");
        
        Field(x => x.User, type: typeof(UserType)).Description("The author of the blog.");
    }
}
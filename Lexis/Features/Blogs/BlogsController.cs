using LexisApi.Features.Blogs.Create;
using LexisApi.Features.Blogs.Search;
using LexisApi.Infrastructure;
using LexisApi.Models.Output.Blogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LexisApi.Features.Blogs;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : MediatorAwareController
{
    #region Constructors

    public BlogsController(IMediator mediator) : base(mediator) { }

    #endregion

    #region Apis

    /// <summary>
    /// Create a Blog
    /// </summary>
    /// <param name="createBlog">Data for creating a blog</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A created <see cref="Blog"/></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Blog), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(Models.Input.Blogs.Create.CreateBlog createBlog, CancellationToken cancellationToken)
    {
        var command = new CreateCommand(createBlog);
        var result = await Mediator.Send(command, cancellationToken);
        var location = new Uri(Url.Link("BlogById", new { result.Id })!);

        return Created(location, result);
    }

    /// <summary>
    /// Get specific blog by Id
    /// </summary>
    /// <param name="id">Id of the blog</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A specific <see cref="Blog"/></returns>
    [HttpGet("{id}", Name = "BlogById")]
    [ProducesResponseType(typeof(Blog), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var searchQuery = new SearchQuery(id);
        var results = await Mediator.Send(searchQuery, cancellationToken);
        var result = results.FirstOrDefault();

        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Get blogs by authorId
    /// </summary>
    /// <param name="authorId">Id of the author</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A list of <see cref="Blog"/>filter by the author</returns>
    [HttpGet("author/{authorId}")]
    [ProducesResponseType(typeof(IEnumerable<Blog>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAuthor(string authorId, CancellationToken cancellationToken)
    {
        var searchQuery = new SearchQuery(AuthorId:authorId);
        var results = await Mediator.Send(searchQuery, cancellationToken);

        return Ok(results);
    }

    /// <summary>
    /// Get all the blogs
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of <see cref="Blog"/></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Blog>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var searchQuery = new SearchQuery();
        var results = await Mediator.Send(searchQuery, cancellationToken);

        return Ok(results);
    }

    #endregion
}
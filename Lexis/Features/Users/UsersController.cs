using LexisApi.Features.Users.Create;
using LexisApi.Features.Users.Search;
using LexisApi.Infrastructure;
using LexisApi.Models.Input.Users.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LexisApi.Features.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : MediatorAwareController
{
    #region Constructors

    public UsersController(IMediator mediator) : base(mediator) { }

    #endregion

    #region Apis

    /// <summary>
    /// Create a User
    /// </summary>
    /// <param name="createUser">Data for creating a user</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateUser createUser, CancellationToken cancellationToken)
    {
        var command = new CreateCommand(createUser);
        var result = await Mediator.Send(command, cancellationToken);
        var location = new Uri(Url.Link("UserById", new { result.Id })!);

        return Created(location, result);
    }

    /// <summary>
    /// Get specific user by Id
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "UserById")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var searchQuery = new SearchQuery(id);
        var results = await Mediator.Send(searchQuery, cancellationToken);
        var result = results.FirstOrDefault();

        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Get all the users
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var searchQuery = new SearchQuery();
        var results = await Mediator.Send(searchQuery, cancellationToken);

        return Ok(results);
    }

    #endregion

}
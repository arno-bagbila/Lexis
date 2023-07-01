using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LexisApi.Infrastructure;

public class MediatorAwareController : ControllerBase
{
    protected readonly IMediator Mediator;

    #region Constructors

    public MediatorAwareController(IMediator mediator)
    {
        Mediator = mediator;
    }

    #endregion
}
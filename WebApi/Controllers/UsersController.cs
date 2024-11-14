using Application.Common.Dto;
using Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ukid.Domain.Enums;

namespace WebApi.Controllers;

[Authorize(Roles = nameof(DomainRole.Administrator))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public partial class UsersController : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var request = new GetUserQuery { Id = id };
        return await Mediator.Send(request);
    }
}
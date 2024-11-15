﻿using Application.Common.Dto;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ukid.Domain.Enums;

namespace WebApi.Controllers;

[Authorize(Roles = nameof(DomainRole.Administrator))]
[Authorize(AuthenticationSchemes = RefreshTokenConfig.SchemeName)]
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
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Login(LoginCommand request)
    {
        var user = await Mediator.Send(request);
        return user;
    }
    
    [HttpPost("refresh-access-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = RefreshTokenConfig.SchemeName)]
    public async Task<ActionResult<AccessTokenDto>> RefreshAccessToken()
    {
        RefreshAccessTokenCommand request = new()
        {
            UserId = int.Parse(User.Identity?.Name ?? throw new BadHttpRequestException("User id not found")),
        };
        var user = await Mediator.Send(request);
        return user;
    }
}
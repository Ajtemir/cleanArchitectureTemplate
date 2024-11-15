using Application.Common.Dto;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Commands;

public class RefreshAccessTokenCommand : IRequest<AccessTokenDto>
{
    public required int UserId { get; set; }

    public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenCommand, AccessTokenDto>
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ITokenService _tokenService;

        public RefreshAccessTokenHandler(IUserAccountService userAccountService, ITokenService tokenService)
        {
            _userAccountService = userAccountService;
            _tokenService = tokenService;
        }

        public async Task<AccessTokenDto> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userAccountService.GetUserByIdAsync(request.UserId, cancellationToken);
            var accessToken = _tokenService.GetAccessToken(user);
            var accessTokenResult = new AccessTokenDto
            {
                AccessToken = accessToken,
            };
            return accessTokenResult;
        }
        
    }
}
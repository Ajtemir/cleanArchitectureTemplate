using Application.Common.Dto;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Commands;

public class LoginCommand : IRequest<UserDto>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUserAccountService userAccountService, IMapper mapper, ITokenService tokenService)
        {
            _userAccountService = userAccountService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userAccountService.Login(request.UserName, request.Password, cancellationToken);
            var dto = _mapper.Map<UserDto>(user);
            var accessToken = _tokenService.GetAccessToken(user);
            var refreshToken = _tokenService.GetRefreshToken(user);
            dto.RefreshToken = refreshToken;
            dto.AccessToken = accessToken;
            return dto;
        }
    }
}
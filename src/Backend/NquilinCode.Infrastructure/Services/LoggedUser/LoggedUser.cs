using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Domain.Services.LoggedUser;
using NquilinCode.Infrastructure.DataAccess;

namespace NquilinCode.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly NquilinCodeDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(NquilinCodeDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<User> User(CancellationToken cancellationToken)
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext.Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userIdentifier, cancellationToken);
    }
}
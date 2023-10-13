using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.Services.Interfaces;
using Common.DTO.AuthDTO;
using Common.Models;
using DAL.Repositories.Interfaces;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _hasher;
    private readonly JwtSettings _settings;

    public AuthService(IUserRepository repo, IPasswordHasher hasher, IOptions<JwtSettings> settings)
    {
        _repository = repo ?? throw new ArgumentNullException(nameof(repo));
        _hasher = hasher;
        _settings = settings.Value;
    }

    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user)
    {
        string hashedPassword = _hasher.HashPassword(user.Password);
        var existingUser = await _repository.FindByLoginAsync(user.Login);

        if (existingUser == null)
            throw new KeyNotFoundException(user.Login);

        if (BCrypt.Net.BCrypt.Verify(hashedPassword, existingUser.PasswordHash))
            throw new UnauthorizedAccessException(user.Login);

        return new AuthSuccessDTO(GenerateJwtToken(existingUser));
    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user)
    {
        string hashedPassword = _hasher.HashPassword(user.Password);
        var existingUser = await _repository.FindByLoginAsync(user.Login);

        if (existingUser != null)
            throw new InvalidOperationException(user.Login);

        var newUser = new User() {
            Login = user.Login,
            PasswordHash = hashedPassword,
        };
        await _repository.AddAsync(newUser);

        return new AuthSuccessDTO(GenerateJwtToken(newUser));
    }
    
    private string GenerateJwtToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }
}
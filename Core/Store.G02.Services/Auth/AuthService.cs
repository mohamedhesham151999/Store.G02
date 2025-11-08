using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Domain.Exceptionsn;
using Store.G02.Domain.Exceptionsn.BadRequest;
using Store.G02.Domain.Exceptionsn.Unauthorized;
using Store.G02.Services.Abstractions.Auth;
using Store.G02.Shared;
using Store.G02.Shared.Dtos.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Store.G02.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager ,IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null) throw new UserNotFoundException(request.Email);

            var flag = await _userManager.CheckPasswordAsync(user , request.Password);
            if (!flag) throw new UnauthorizedException();



            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
            
        } 
         
        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new RegisterationBadRequestException(result.Errors.Select(E => E.Description).ToList());

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };

        }


        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            // TOKEN 
            // 1. HEADER    (TYPE, ALGO)
            // 2. PAYLOAD   (CLAIMS)
            // 3. SIGNATURE (KEY)


            var authClaims = new List<Claim>()
            {
                 new Claim(ClaimTypes.GivenName, user.DisplayName),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };


            var roles = await  _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtOptions = options.Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience : jwtOptions.Audience,
                claims : authClaims,
                expires : DateTime.Now.AddDays(jwtOptions.DurationDays),
                signingCredentials : new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

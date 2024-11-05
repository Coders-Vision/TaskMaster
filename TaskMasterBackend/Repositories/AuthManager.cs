using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TaskMasterBackend.Contracts;
using TaskMasterBackend.Database;
using TaskMasterBackend.Dto.AppUser;
using TaskMasterBackend.Dto.Task;

namespace TaskMasterBackend.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _appUser;
        private readonly IConfiguration _configuration;
        private AppUser _user;
        public AuthManager(IMapper mapper,UserManager<AppUser> appUser, IConfiguration config)
        {
            this._mapper = mapper;
            this._appUser = appUser;    
            this._configuration = config; 
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _user= await _appUser.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _appUser.CheckPasswordAsync(_user, loginDto.Password);


       if(_user == null || isValidUser == false)
            {
                return null;
            }

            var token =await  GenerateToken();

            return new AuthResponseDto
            {
                Token = token,

                UserId = _user.Id,

                RefreshToken = await CreateRefreshToken()
            };

        }

        public async Task<IEnumerable<IdentityError>> Register(AppUserDto userDto)
        {
            _user = _mapper.Map<AppUser>(userDto);
            _user.UserName = userDto.Email;
            var result = await _appUser.CreateAsync(_user, userDto.Password);

            if(result.Succeeded) {
                await _appUser.AddToRoleAsync(_user, "User");
            }

            return result.Errors;
        }

        public async Task<string> GenerateToken()
        {
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credenials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);

            var roles = await _appUser.GetRolesAsync(_user);
            var rolesClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _appUser.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),
            }.Union(userClaims).Union(rolesClaims);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credenials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await _appUser.RemoveAuthenticationTokenAsync(_user, "TaskMasterApi", "RefreshToken");
            var newRefreshToken = await _appUser.GenerateUserTokenAsync(_user, "TaskMasterApi", "RefreshToken");
            var result = await _appUser.SetAuthenticationTokenAsync(_user, "TaskMasterApi", "RefreshToken", newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
      var jwtSecurityTokenHandler=new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var userName = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _appUser.FindByNameAsync(userName);
            if( _user == null || _user.Id!= request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _appUser.VerifyUserTokenAsync(_user, "TaskMasterApi", "RefreshToken",request.RefreshToken);
            if( isValidRefreshToken ) {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = request.RefreshToken,
                };
            }

            await _appUser.UpdateSecurityStampAsync(_user);

            return null;

        } 
    }
}

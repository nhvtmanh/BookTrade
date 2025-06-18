using AutoMapper;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookTradeAPI.Models.Request.MReq_User;

namespace BookTradeAPI.Services
{
    public interface IS_Auth
    {
        Task<ApiResponse<MRes_User>> Register(MReq_User_Register request);
        Task<ApiResponse<string>> Login(MReq_User_Login request);
    }
    public class S_Auth : IS_Auth
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public S_Auth(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ApiResponse<MRes_User>> Register(MReq_User_Register request)
        {
            var response = new ApiResponse<MRes_User>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = ["Email đã được sử dụng"];
                return response;
            }

            var data = _mapper.Map<User>(request);
            data.UserName = request.Email;

            var result = await _userManager.CreateAsync(data, request.Password);
            if (!result.Succeeded)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = result.Errors.Select(e => e.Description).ToList();
                return response;
            }

            // Assign "Member" role
            await _userManager.AddToRoleAsync(data, UserRoleConstant.MEMBER);

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_User>(data);
            response.Message = ["Đăng ký thành công"];
            return response;
        }

        public async Task<ApiResponse<string>> Login(MReq_User_Login request)
        {
            var response = new ApiResponse<string>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = ["Email hoặc mật khẩu không đúng"];
                return response;
            }

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var jwtConfig = _configuration.GetSection("Jwt");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]!));

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                expires: DateTime.UtcNow.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = new JwtSecurityTokenHandler().WriteToken(token);
            response.Message = ["Đăng nhập thành công"];
            return response;
        }
    }
}

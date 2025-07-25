using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookTradeAPI.Services
{
    public interface IS_Auth
    {
        Task<ApiResponse<MRes_User>> Register(MReq_User_Register request);
        Task<ApiResponse<MRes_Shop>> RegisterSeller(MReq_Seller_Register request);
        Task<ApiResponse<string>> Login(MReq_User_Login request);
    }
    public class S_Auth : IS_Auth
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public S_Auth(UserManager<User> userManager, IMapper mapper, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<MRes_User>> Register(MReq_User_Register request)
        {
            var response = new ApiResponse<MRes_User>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = [MessageErrorConstant.EMAIL_ALREADY_EXISTS];
                return response;
            }

            var data = _mapper.Map<User>(request);
            data.UserName = request.Email;
            data.AvatarUrl = "images/avatar.png";

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
            response.Message = ["You have successfully registered"];
            return response;
        }

        public async Task<ApiResponse<MRes_Shop>> RegisterSeller(MReq_Seller_Register request)
        {
            var response = new ApiResponse<MRes_Shop>();

            var user = await _userManager.FindByEmailAsync(request.User.Email);
            if (user != null)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = [MessageErrorConstant.EMAIL_ALREADY_EXISTS];
                return response;
            }

            var userData = _mapper.Map<User>(request.User);
            userData.UserName = request.User.Email;
            userData.AvatarUrl = "images/avatar.png";

            var result = await _userManager.CreateAsync(userData, request.User.Password);
            if (!result.Succeeded)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = result.Errors.Select(e => e.Description).ToList();
                return response;
            }

            // Assign "Seller" role
            await _userManager.AddToRoleAsync(userData, UserRoleConstant.SELLER);

            var shop = new Shop
            {
                Name = request.Name,
                Description = request.Description,
                BannerUrl = request.BannerUrl,
                Status = (byte)EN_Shop.Status.Pending,
                CreatedAt = DateTime.Now,
                UserId = userData.Id
            };

            _dbContext.Shops.Add(shop);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_Shop>(shop);
            response.Message = ["You have successfully registered. Please wait for confirmation"];
            return response;
        }

        public async Task<ApiResponse<string>> Login(MReq_User_Login request)
        {
            var response = new ApiResponse<string>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = ["Incorrect email or password"];
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
            response.Message = ["You have successfully logged in"];
            return response;
        }
    }
}

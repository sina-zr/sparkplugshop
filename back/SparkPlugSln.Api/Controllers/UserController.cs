using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SparkPlugSln.Application.Helpers;
using SparkPlugSln.Application.Security;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Models.User;

namespace SparkPlugSln.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet]
    [Route("/test")]
    public IActionResult Test()
    {
        return Ok("Testing 123");
    }

    /// <summary>
    /// Used to get login sms
    /// </summary>
    /// <param name="tell">The user's tell</param>
    /// <response code="200">Returns Ok if sms sent</response>
    /// <response code="400">Invalid input</response>
    [HttpPost("/login/{tell}")]
    public async Task<IActionResult> Login([Required] [Length(11,11)] string tell)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid input");
        }
        
        await _userService.UpsertUser(tell);
        
        return Ok();
    }
    
    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginDto">The user's login credentials.</param>
    /// <returns>A JWT token and user details.</returns>
    /// <response code="400">invalid input</response>
    /// <response code="400">code incorrect</response>
    /// <response code="404">user not found</response>
    [HttpPost("/login")]
    [ProducesResponseType(typeof(LoginResponseVm), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("invalid input");
        }

        // Fetch the user from the database
        var user = await _userService.GetUserByTell(loginDto.Tell);
        if (user == null)
        {
            return NotFound("user not found");
        }

        if (loginDto.Code != user.VerificationCode)
        {
            return BadRequest("code incorrect");
        }

        // Create claims for the JWT token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.MobilePhone, user.Tell),
            new Claim(ClaimTypes.Role, ((int)user.Role).ToString())
        };

        // Generate the JWT token
        var token = GenerateJwtToken(claims);
        
        return Ok(new LoginResponseVm
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Tell = user.Tell,
            Role = user.Role
        });
    }
    
    /// <summary>
    /// GetUserAddresses
    /// </summary>
    /// <response code="404">user not found</response>
    /// <response code="204">user has no address</response>
    /// <returns>Returns a list of user addresses</returns>
    [HttpGet("/[action]")]
    [ProducesResponseType(typeof(List<AddressVm>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetUserAddresses()
    {
        var userId = User.GetId();
        var user = await _userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound("user not found");
        }
        
        var addressList = await _userService.GetUserAddresses(userId);
        if (!addressList.Any())
        {
            return NoContent();
        }

        var addressVmList = addressList.Select(a => new AddressVm
        {
            Id = a.Id,
            Address = a.Address,
        }).ToList();
        
        return Ok(addressVmList);
    }
    
    /// <summary>
    /// GetUserAddresses
    /// </summary>
    /// <response code="400">invalid input</response>
    /// <response code="404">user not found</response>
    /// <response code="500">server error</response>
    /// <returns>Returns a list of user addresses</returns>
    [HttpGet("/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> AddUserAddresses([Required] string address)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("invalid input");
        }

        var userId = User.GetId();
        var user = await _userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound("user not found");
        }

        var result = await _userService.AddUserAddress(userId, address);
        if (!result)
        {
            return StatusCode(500, "server error");
        }
        return Ok();
    }
    
    private string GenerateJwtToken(List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationTime = int.Parse(jwtSettings["ExpirationDays"]);
        if (expirationTime < 1)
        {
            expirationTime = 7;
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expirationTime),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    #region Admin

    /// <summary>
    /// Get List of users for admin
    /// </summary>
    /// <param name="pageId">default is 1</param>
    /// <param name="pageSize">default is 100</param>
    /// <param name="filterName">default is empty</param>
    /// <param name="filterTell">default is empty</param>
    /// <response code="403">forbidden</response>
    /// <response code="404">no user found</response>
    /// <returns>list of users with pagination</returns>
    [HttpGet("/admin/[action]")]
    [ProducesResponseType(typeof(UsersListVm), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUsersList(int pageId = 1, int pageSize = 100, string? filterName = null, string? filterTell = null)
    {
        // Call the UserService to get the paginated list of users
        var usersListVm = await _userService.GetUsersListAsync(pageId, pageSize, filterName, filterTell);

        if (usersListVm.Users.Count < 1 || !usersListVm.Users.Any())
        {
            return NotFound("No users found.");
        }
        
        return Ok(usersListVm);
    }
    
    /// <summary>
    /// Get details of one user
    /// </summary>
    /// <param name="userId"></param>
    /// <response code="400">invalid input</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">user not found</response>
    /// <returns>user with details</returns>
    [HttpGet("/admin/[action]")]
    [ProducesResponseType(typeof(UserDetailsVm), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUserDetails(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("invalid input");
        }

        var user = await _userService.GetUserWithDetails(userId);
        if (user == null)
        {
            return NotFound("user not found");
        }

        var userDetailsVm = new UserDetailsVm
        {
            Id = user.Id,
            FullName = user.FullName,
            Tell = user.Tell,
            Role = user.Role,
            IsTellVerified = user.IsTellVerified,
            ProfileImageName = user.ProfileImageName,
            Addresses = user.UserAddresses.Select(a => a.Address).ToList(),
            Password = user.Password,
            VerificationCode = user.VerificationCode,
        };

        return Ok(userDetailsVm);
    }
        
    /// <summary>
    /// Edit a user by admin
    /// </summary>
    /// <param name="editUserDto"></param>
    /// <response code="200">edit successful</response>
    /// <response code="400">invalid input</response>
    /// <response code="403">forbidden</response>
    /// <response code="404">user not found</response>
    /// <response code="500">server error</response>
    /// <returns>ok if edit successful</returns>
    [HttpGet("/admin/[action]")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> EditUser(EditUserDto editUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("invalid input");
        }
        
        var user = await _userService.GetUserById(editUserDto.Id);
        if (user == null)
        {
            return NotFound("user not found");
        }

        var result = await _userService.EditUser(editUserDto);
        if (!result)
        {
            return StatusCode(500, "server error");
        }

        return Ok();
    }
    
    /// <summary>
    /// Delete Product By Admin
    /// </summary>
    /// <param name="userId"></param>
    /// <response code="403">forbidden</response>
    /// <response code="404">user not found</response>
    /// <response code="500">server error</response>
    /// <returns>Ok if successfully deleted</returns>
    [HttpPost("/admin/[action]")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound("user not found");
        }

        var result = await _userService.DeleteUser(userId);
        if (!result)
        {
            return StatusCode(500, "server error");
        }

        return Ok();
    }

    #endregion
}
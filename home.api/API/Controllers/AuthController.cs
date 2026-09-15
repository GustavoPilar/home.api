using home.api.Application.Entities.DTOs.auth;
using home.api.Application.Entities.DTOs.Auth;
using home.api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace home.api.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController(
        ILogger<AuthController> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager) : ControllerBase
    {
        #region Fields

        private readonly ILogger<AuthController> logger = logger;
        private readonly UserManager<User> userManager = userManager;
        private readonly SignInManager<User> signInManager = signInManager;

        #endregion

        #region HttpActions

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (request is null)
                    return this.BadRequest(new RegisterResponse() { Message = "Requisição inválida." });

                User? user = await this.userManager.FindByEmailAsync(request.Email);

                if (user is not null)
                    return BadRequest(new RegisterResponse() { Message = "Requisição inválida." });

                user = new User()
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Birthday = request.Birthday,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                };

                IdentityResult result = await this.userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return this.BadRequest(new RegisterResponse() { Message = "Requisição inválida." });
                }

                return this.Created(user.Id.ToString(), new RegisterResponse() { IsSucceeded = true, Message = "Usuário criado", User = new Application.Entities.DTOs.User.UserResponse(user) });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Message = "false" });
            }
        }

        #endregion

    }
}

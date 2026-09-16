using home.api.Application.Entities;
using home.api.Application.Entities.DTOs;
using home.api.Application.Entities.DTOs.Auth;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace home.api.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController(
        ILogger<AuthController> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IUserMapper userMapper) : ControllerBase
    {
        #region Fields

        private readonly ILogger<AuthController> logger = logger;
        private readonly UserManager<User> userManager = userManager;
        private readonly SignInManager<User> signInManager = signInManager;
        private readonly ITokenService tokenService = tokenService;
        private readonly IUserMapper userMapper = userMapper;

        #endregion

        #region HttpActions :: RegisterAsync(), LoginAsync()

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="request">Dados do cadastro</param>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UserResponse>>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (request is null)
                    return this.BadRequest(new ApiResponse<UserResponse> { Message = "Requisição inválida." });

                User? user = await this.userManager.FindByEmailAsync(request.Email);

                if (user is not null)
                    return this.BadRequest(new ApiResponse<UserResponse> { Message = "Requisição inválida." });

                user = new User
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Birthday = request.Birthday,
                    Active = true,
                    CreatedAt = DateTime.UtcNow
                };

                IdentityResult result = await this.userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    this.logger.LogWarning(
                        "Falha ao cadastrar usuário: {Errors}",
                        string.Join("; ", result.Errors.Select(x => x.Description)));

                    return this.BadRequest(new ApiResponse<UserResponse> { Message = "Requisição inválida." });
                }

                return this.Created(
                    user.Id.ToString(),
                    new ApiResponse<UserResponse>
                    {
                        Success = true,
                        Message = "Usuário criado com sucesso!",
                        Data = this.userMapper.ToResponse(user)
                    });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<UserResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Autentica o usuário e devolve o token de acesso
        /// </summary>
        /// <param name="request">Credenciais</param>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> LoginAsync(LoginRequest request)
        {
            try
            {
                if (request is null)
                    return this.BadRequest(new ApiResponse<AuthResponse> { Message = "Credenciais inválidas." });

                User? user = await this.userManager.FindByEmailAsync(request.Email);

                if (user is null)
                    return this.BadRequest(new ApiResponse<AuthResponse> { Message = "Credenciais inválidas." });

                Microsoft.AspNetCore.Identity.SignInResult result = await this.signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                    return this.BadRequest(new ApiResponse<AuthResponse> { Message = "Credenciais inválidas." });

                if (!user.Active)
                    return this.BadRequest(new ApiResponse<AuthResponse> { Message = "Credenciais inválidas." });

                return this.Ok(new ApiResponse<AuthResponse>
                {
                    Success = true,
                    Message = "Bem-vindo de volta!",
                    Data = new AuthResponse
                    {
                        User = this.userMapper.ToResponse(user),
                        Token = this.tokenService.GetToken(user)
                    }
                });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<AuthResponse> { Message = "Erro interno." });
            }
        }

        #endregion
    }
}

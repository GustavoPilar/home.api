using home.api.Application.Entities;
using home.api.Application.Entities.DTOs;
using home.api.Application.Interfaces;
using home.api.Exceptions;
using home.api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace home.api.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HomeController(
        ILogger<HomeController> logger,
        IHomeService homeService) : ControllerBase
    {
        #region Fields

        private readonly ILogger<HomeController> logger = logger;
        private readonly IHomeService homeService = homeService;

        #endregion

        #region HttpActions :: GetHomesAsync(), GetHomeByIdAsync(), CreateHomeAsync(), UpdateHomeAsync(), DeleteHomeAsync()

        /// <summary>
        /// Lista os lares do usuário autenticado
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<HomeResponse>>>> GetHomesAsync()
        {
            try
            {
                IEnumerable<HomeResponse> homes = await this.homeService.GetEntitiesAsync(this.User.GetUserId());

                return this.Ok(new ApiResponse<IEnumerable<HomeResponse>>
                {
                    Success = true,
                    Message = "Lares listados com sucesso.",
                    Data = homes
                });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<IEnumerable<HomeResponse>> { Message = "Requisição inválida." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<IEnumerable<HomeResponse>> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Busca um lar do usuário autenticado
        /// </summary>
        /// <param name="id">Lar ID</param>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<HomeResponse>>> GetHomeByIdAsync(Guid id)
        {
            try
            {
                HomeResponse? home = await this.homeService.GetEntityByIdAsync(this.User.GetUserId(), id);

                if (home is null)
                    return this.NotFound(new ApiResponse<HomeResponse> { Message = "Lar não encontrado." });

                return this.Ok(new ApiResponse<HomeResponse>
                {
                    Success = true,
                    Message = "Lar encontrado.",
                    Data = home
                });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<HomeResponse> { Message = "Requisição inválida." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Cria um lar para o usuário autenticado
        /// </summary>
        /// <param name="request">Dados do lar</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<HomeResponse>>> CreateHomeAsync(HomeRequest request)
        {
            try
            {
                HomeResponse home = await this.homeService.CreateEntityAsync(this.User.GetUserId(), request);

                return this.CreatedAtAction(
                    nameof(this.GetHomeByIdAsync),
                    new { id = home.Id },
                    new ApiResponse<HomeResponse>
                    {
                        Success = true,
                        Message = "Lar criado com sucesso.",
                        Data = home
                    });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<HomeResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Não foi possível criar o lar." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Atualiza um lar do usuário autenticado
        /// </summary>
        /// <param name="request">Dados do lar</param>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<HomeResponse>>> UpdateHomeAsync(HomeUpdate request)
        {
            try
            {
                HomeResponse home = await this.homeService.UpdateEntityAsync(this.User.GetUserId(), request);

                return this.Ok(new ApiResponse<HomeResponse>
                {
                    Success = true,
                    Message = "Lar atualizado com sucesso.",
                    Data = home
                });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<HomeResponse> { Message = "Lar não encontrado." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<HomeResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Não foi possível atualizar o lar." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Remove um lar do usuário autenticado
        /// </summary>
        /// <param name="id">Lar ID</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<HomeResponse>>> DeleteHomeAsync(Guid id)
        {
            try
            {
                bool deleted = await this.homeService.DeleteEntityAsync(this.User.GetUserId(), id);

                if (!deleted)
                    return this.NotFound(new ApiResponse<HomeResponse> { Message = "Lar não encontrado." });

                return this.Ok(new ApiResponse<HomeResponse>
                {
                    Success = true,
                    Message = "Lar removido com sucesso."
                });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<HomeResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Não foi possível remover o lar." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<HomeResponse> { Message = "Erro interno." });
            }
        }

        #endregion
    }
}

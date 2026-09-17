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
    public class FamilyController(
        ILogger<FamilyController> logger,
        IFamilyService familyService) : ControllerBase
    {
        #region Fields

        private readonly ILogger<FamilyController> logger = logger;
        private readonly IFamilyService familyService = familyService;

        #endregion

        #region HttpActions :: GetFamiliesAsync(), GetFamilyByIdAsync(), CreateFamilyAsync(), UpdateFamilyAsync(), DeleteFamilyAsync()

        /// <summary>
        /// Lista as famílias das quais o usuário autenticado é membro
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FamilyResponse>>>> GetFamiliesAsync()
        {
            try
            {
                IEnumerable<FamilyResponse> families = await this.familyService.GetEntitiesAsync(this.User.GetUserId());

                return this.Ok(new ApiResponse<IEnumerable<FamilyResponse>>
                {
                    Success = true,
                    Message = "Famílias listadas com sucesso.",
                    Data = families
                });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<IEnumerable<FamilyResponse>> { Message = "Requisição inválida." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<IEnumerable<FamilyResponse>> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Busca uma família da qual o usuário autenticado é membro
        /// </summary>
        /// <param name="id">Família ID</param>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<FamilyResponse>>> GetFamilyByIdAsync(Guid id)
        {
            try
            {
                FamilyResponse? family = await this.familyService.GetEntityByIdAsync(this.User.GetUserId(), id);

                if (family is null)
                    return this.NotFound(new ApiResponse<FamilyResponse> { Message = "Família não encontrada." });

                return this.Ok(new ApiResponse<FamilyResponse>
                {
                    Success = true,
                    Message = "Família encontrada.",
                    Data = family
                });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyResponse> { Message = "Requisição inválida." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Cria uma família para o usuário autenticado
        /// </summary>
        /// <param name="request">Dados da família</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<FamilyResponse>>> CreateFamilyAsync(FamilyRequest request)
        {
            try
            {
                FamilyResponse family = await this.familyService.CreateEntityAsync(this.User.GetUserId(), request);

                return this.CreatedAtAction(
                    nameof(this.GetFamilyByIdAsync),
                    new { id = family.Id },
                    new ApiResponse<FamilyResponse>
                    {
                        Success = true,
                        Message = "Família criada com sucesso.",
                        Data = family
                    });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Não foi possível criar a família." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Atualiza uma família da qual o usuário autenticado é membro
        /// </summary>
        /// <param name="request">Dados da família</param>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<FamilyResponse>>> UpdateFamilyAsync(FamilyUpdate request)
        {
            try
            {
                FamilyResponse family = await this.familyService.UpdateEntityAsync(this.User.GetUserId(), request);

                return this.Ok(new ApiResponse<FamilyResponse>
                {
                    Success = true,
                    Message = "Família atualizada com sucesso.",
                    Data = family
                });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyResponse> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Não foi possível atualizar a família." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Remove uma família da qual o usuário autenticado é membro
        /// </summary>
        /// <param name="id">Família ID</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<FamilyResponse>>> DeleteFamilyAsync(Guid id)
        {
            try
            {
                bool deleted = await this.familyService.DeleteEntityAsync(this.User.GetUserId(), id);

                if (!deleted)
                    return this.NotFound(new ApiResponse<FamilyResponse> { Message = "Família não encontrada." });

                return this.Ok(new ApiResponse<FamilyResponse>
                {
                    Success = true,
                    Message = "Família removida com sucesso."
                });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Não foi possível remover a família." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Erro interno." });
            }
        }

        #endregion
    }
}

using home.api.Application.Entities;
using home.api.Application.Entities.DTOs.Families;
using home.api.Application.Interfaces;
using home.api.Exceptions;
using home.api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace home.api.API.Controllers
{
    /// <summary>
    /// Catálogo de títulos de uma família. Leitura para qualquer membro,
    /// escrita restrita ao host.
    /// </summary>
    [ApiController]
    [Route("family/{familyId:guid}/titles")]
    public class FamilyTitleController(
        ILogger<FamilyTitleController> logger,
        IFamilyService familyService) : ControllerBase
    {
        #region Fields

        private readonly ILogger<FamilyTitleController> logger = logger;
        private readonly IFamilyService familyService = familyService;

        #endregion

        #region HttpActions :: GetTitlesAsync(), CreateTitleAsync(), UpdateTitleAsync(), DeleteTitleAsync()

        /// <summary>
        /// Lista os títulos globais e os próprios da família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FamilyTitleResponse>>>> GetTitlesAsync(Guid familyId)
        {
            try
            {
                IEnumerable<FamilyTitleResponse> titles = await this.familyService.GetTitlesAsync(this.User.GetUserId(), familyId);

                return this.Ok(new ApiResponse<IEnumerable<FamilyTitleResponse>>
                {
                    Success = true,
                    Message = "Títulos listados com sucesso.",
                    Data = titles
                });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<IEnumerable<FamilyTitleResponse>> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<IEnumerable<FamilyTitleResponse>> { Message = "Requisição inválida." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<IEnumerable<FamilyTitleResponse>> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Cria um título próprio da família. Restrito ao host.
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">Dados do título</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<FamilyTitleResponse>>> CreateTitleAsync(Guid familyId, FamilyTitleRequest request)
        {
            try
            {
                FamilyTitleResponse title = await this.familyService.CreateTitleAsync(this.User.GetUserId(), familyId, request);

                return this.CreatedAtAction(
                    nameof(this.GetTitlesAsync),
                    new { familyId },
                    new ApiResponse<FamilyTitleResponse>
                    {
                        Success = true,
                        Message = "Título criado com sucesso.",
                        Data = title
                    });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<FamilyTitleResponse> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyTitleResponse> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyTitleResponse> { Message = exception.Message });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyTitleResponse> { Message = "Não foi possível criar o título." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyTitleResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Renomeia um título próprio da família. Restrito ao host.
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">Dados do título</param>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<FamilyTitleResponse>>> UpdateTitleAsync(Guid familyId, FamilyTitleUpdate request)
        {
            try
            {
                FamilyTitleResponse title = await this.familyService.UpdateTitleAsync(this.User.GetUserId(), familyId, request);

                return this.Ok(new ApiResponse<FamilyTitleResponse>
                {
                    Success = true,
                    Message = "Título atualizado com sucesso.",
                    Data = title
                });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<FamilyTitleResponse> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyTitleResponse> { Message = "Título não encontrado." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyTitleResponse> { Message = exception.Message });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyTitleResponse> { Message = "Não foi possível atualizar o título." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyTitleResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Remove um título próprio da família. Restrito ao host.
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="titleId">Título ID</param>
        [HttpDelete("{titleId:guid}")]
        public async Task<ActionResult<ApiResponse<FamilyTitleResponse>>> DeleteTitleAsync(Guid familyId, Guid titleId)
        {
            try
            {
                bool deleted = await this.familyService.DeleteTitleAsync(this.User.GetUserId(), familyId, titleId);

                if (!deleted)
                    return this.NotFound(new ApiResponse<FamilyTitleResponse> { Message = "Título não encontrado." });

                return this.Ok(new ApiResponse<FamilyTitleResponse>
                {
                    Success = true,
                    Message = "Título removido com sucesso."
                });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<FamilyTitleResponse> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyTitleResponse> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyTitleResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyTitleResponse> { Message = "Não foi possível remover o título." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyTitleResponse> { Message = "Erro interno." });
            }
        }

        #endregion
    }
}

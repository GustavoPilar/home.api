using home.api.Application.Entities;
using home.api.Application.Entities.DTOs.Families;
using home.api.Application.Interfaces;
using home.api.Exceptions;
using home.api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace home.api.API.Controllers
{
    /// <summary>
    /// Convites de uma família. Todas as ações são restritas ao host.
    /// </summary>
    [ApiController]
    [Route("family/{familyId:guid}/invites")]
    public class FamilyInviteController(
        ILogger<FamilyInviteController> logger,
        IFamilyService familyService) : ControllerBase
    {
        #region Fields

        private readonly ILogger<FamilyInviteController> logger = logger;
        private readonly IFamilyService familyService = familyService;

        #endregion

        #region HttpActions :: GetInvitesAsync(), CreateInviteAsync(), RevokeInviteAsync()

        /// <summary>
        /// Lista os convites emitidos pela família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FamilyInviteResponse>>>> GetInvitesAsync(Guid familyId)
        {
            try
            {
                IEnumerable<FamilyInviteResponse> invites = await this.familyService.GetInvitesAsync(this.User.GetUserId(), familyId);

                return this.Ok(new ApiResponse<IEnumerable<FamilyInviteResponse>>
                {
                    Success = true,
                    Message = "Convites listados com sucesso.",
                    Data = invites
                });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<IEnumerable<FamilyInviteResponse>> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<IEnumerable<FamilyInviteResponse>> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<IEnumerable<FamilyInviteResponse>> { Message = "Requisição inválida." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<IEnumerable<FamilyInviteResponse>> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Emite um convite. O token em claro volta apenas nesta resposta.
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">E-mail do destinatário, ou vazio para link aberto</param>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<FamilyInviteCreatedResponse>>> CreateInviteAsync(Guid familyId, FamilyInviteRequest request)
        {
            try
            {
                FamilyInviteCreatedResponse invite = await this.familyService.CreateInviteAsync(this.User.GetUserId(), familyId, request);

                return this.Ok(new ApiResponse<FamilyInviteCreatedResponse>
                {
                    Success = true,
                    Message = "Convite criado. Guarde o token: ele não é recuperável.",
                    Data = invite
                });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<FamilyInviteCreatedResponse> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyInviteCreatedResponse> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyInviteCreatedResponse> { Message = exception.Message });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyInviteCreatedResponse> { Message = "Não foi possível criar o convite." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyInviteCreatedResponse> { Message = "Erro interno." });
            }
        }

        /// <summary>
        /// Revoga um convite
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="inviteId">Convite ID</param>
        [HttpDelete("{inviteId:guid}")]
        public async Task<ActionResult<ApiResponse<FamilyInviteResponse>>> RevokeInviteAsync(Guid familyId, Guid inviteId)
        {
            try
            {
                bool revoked = await this.familyService.RevokeInviteAsync(this.User.GetUserId(), familyId, inviteId);

                if (!revoked)
                    return this.NotFound(new ApiResponse<FamilyInviteResponse> { Message = "Convite não encontrado." });

                return this.Ok(new ApiResponse<FamilyInviteResponse>
                {
                    Success = true,
                    Message = "Convite revogado com sucesso."
                });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<FamilyInviteResponse> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyInviteResponse> { Message = "Família não encontrada." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyInviteResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyInviteResponse> { Message = "Não foi possível revogar o convite." });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyInviteResponse> { Message = "Erro interno." });
            }
        }

        #endregion
    }
}

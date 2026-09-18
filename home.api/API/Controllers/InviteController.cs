using home.api.Application.Entities;
using home.api.Application.Entities.DTOs.Families;
using home.api.Application.Interfaces;
using home.api.Exceptions;
using home.api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace home.api.API.Controllers
{
    /// <summary>
    /// Aceite de convite pelo token recebido no link.
    /// Exige autenticação: quem ainda não tem conta precisa se registrar antes.
    /// </summary>
    [ApiController]
    [Route("invites")]
    public class InviteController(
        ILogger<InviteController> logger,
        IFamilyService familyService) : ControllerBase
    {
        #region Fields

        private readonly ILogger<InviteController> logger = logger;
        private readonly IFamilyService familyService = familyService;

        #endregion

        #region HttpActions :: AcceptInviteAsync()

        /// <summary>
        /// Aceita o convite e entra na família como membro comum
        /// </summary>
        /// <param name="token">Token recebido no link</param>
        [HttpPost("{token}/accept")]
        public async Task<ActionResult<ApiResponse<FamilyResponse>>> AcceptInviteAsync(string token)
        {
            try
            {
                FamilyResponse family = await this.familyService.AcceptInviteAsync(
                    this.User.GetUserId(),
                    this.User.GetUserEmail(),
                    token);

                return this.Ok(new ApiResponse<FamilyResponse>
                {
                    Success = true,
                    Message = "Você entrou na família.",
                    Data = family
                });
            }
            catch (ForbiddenOperationException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<FamilyResponse> { Message = exception.Message });
            }
            catch (EntityNotFoundException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.NotFound(new ApiResponse<FamilyResponse> { Message = "Convite inválido." });
            }
            catch (ArgumentException exception)
            {
                this.logger.LogWarning(exception, exception.Message);
                return this.BadRequest(new ApiResponse<FamilyResponse> { Message = "Requisição inválida." });
            }
            catch (PersistenceException exception)
            {
                this.logger.LogError(exception, exception.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<FamilyResponse> { Message = "Não foi possível aceitar o convite." });
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

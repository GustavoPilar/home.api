using home.api.Application.Entities.DTOs;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;
using home.api.Infra.Repositories;

namespace home.api.Application.Services
{
    /// <summary>
    /// Serviço de lares: herda todo o CRUD da base e recebe o mapeador de Home
    /// </summary>
    public class HomeService(
        UnitOfWork unitOfWork,
        IMapperBase<Home, HomeRequest, HomeUpdate, HomeResponse> mapper,
        ILogger<HomeService> logger) : ServiceBase<Home, HomeRequest, HomeUpdate, HomeResponse>(unitOfWork, mapper, logger), IHomeService
    {
    }
}

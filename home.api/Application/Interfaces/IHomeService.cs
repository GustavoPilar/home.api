using home.api.Application.Entities.DTOs;
using home.api.Domain.Entities;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato do serviço de lares.
    /// Fecha os tipos genéricos do serviço base e é o ponto de extensão
    /// para as regras que forem específicas de Home.
    /// </summary>
    public interface IHomeService : IServiceBase<Home, HomeRequest, HomeUpdate, HomeResponse>
    {
    }
}

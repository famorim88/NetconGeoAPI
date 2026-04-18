using NetconGeoAPI.Domain.Entities;

namespace NetconGeoAPI.Domain.Interfaces
{
    public interface IFeasibilityRepository
    {
        Task<IEnumerable<GeographicElement>> GetAllActiveOrReservedAsync();
    }
}

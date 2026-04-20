using NetconGeoAPI.Application.Dto;
using NetconGeoAPI.Domain.Interfaces;

namespace NetconGeoAPI.Application.Services
{
    public class FeasibilityService
    {
        private readonly IFeasibilityRepository _repository;

        public FeasibilityService(IFeasibilityRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FeasibilityResponse>> GetNearbyElementsAsync(double centerLat, double centerLon, double radius, int page, int size = 20)
        {
            var elements = await _repository.GetAllActiveOrReservedAsync();

            var result = elements
                .Select(e => new FeasibilityResponse(
                    e.Id,
                    e.Name,
                    e.Latitude,
                    e.Longitude,
                    CalculateHaversine(centerLat, centerLon, e.Latitude, e.Longitude)
                ))
                // Filtra pelo raio informado (em metros)
                .Where(x => x.Radius <= radius)
                // Ordena pela menor distância
                .OrderBy(x => x.Radius)
                // Paginação: 20 registros por página
                .Skip((page - 1) * 20)
                .Take(size)
                .ToList();

            return result;
        }

        private double CalculateHaversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Raio da Terra em metros

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double angle) => Math.PI * angle / 180.0;
    }
}

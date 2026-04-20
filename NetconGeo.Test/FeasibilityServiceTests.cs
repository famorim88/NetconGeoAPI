using Moq;
using NetconGeoAPI.Application.Services;
using NetconGeoAPI.Domain.Entities;
using NetconGeoAPI.Domain.Interfaces;

namespace NetconGeo.Test
{
    public class FeasibilityServiceTests
    {
        private readonly Mock<IFeasibilityRepository> _repoMock;
        private readonly FeasibilityService _service;

        public FeasibilityServiceTests()
        {
            _repoMock = new Mock<IFeasibilityRepository>();
            _service = new FeasibilityService(_repoMock.Object);
        }

        [Fact]
        public async Task GetNearbyElements_ShouldFilterOnlyActiveAndReserved()
        {
            // Arrange
            var elements = new List<GeographicElement>
        {
            new() { Id = 1, Name = "Ativo", Status = "ACTIVE", Latitude = -22.910, Longitude = -43.182 },
            new() { Id = 2, Name = "Planejado", Status = "PLANNED", Latitude = -22.911, Longitude = -43.183 },
            new() { Id = 3, Name = "Reservado", Status = "RESERVED", Latitude = -22.912, Longitude = -43.184 }
        };
            _repoMock.Setup(r => r.GetAllActiveOrReservedAsync()).ReturnsAsync(elements.Where(e => e.Status == "ACTIVE" || e.Status == "RESERVED"));

            // Act
            var result = await _service.GetNearbyElementsAsync(-22.910, -43.182, 5000, 1);

            // Assert
            Assert.Equal(2, result.ToList().Count());
            Assert.DoesNotContain(result.ToList(), x => x.Nome == "Planejado");
        }

        [Fact]
        public async Task GetNearbyElements_ShouldCalculateCorrectDistance()
        {
            var elements = new List<GeographicElement>
        {
            new() { Id = 1, Name = "Ponto Próximo", Status = "ACTIVE", Latitude = -22.9130, Longitude = -43.1850 }
        };
            _repoMock.Setup(r => r.GetAllActiveOrReservedAsync()).ReturnsAsync(elements);

            // Act
            var result = await _service.GetNearbyElementsAsync(-22.9108, -43.1822, 1000, 1);

            // Assert
            var item = result.ToList().First();
            Assert.True(item.Radius < 500); // Valida se o Haversine está no range correto
        }
    }
}
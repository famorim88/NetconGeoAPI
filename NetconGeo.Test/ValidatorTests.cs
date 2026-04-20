using NetconGeoAPI.Application.Dto;
using NetconGeoAPI.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetconGeo.Test
{
    public class ValidatorTests
    {
        private readonly FeasibilityRequestValidator _validator = new();

        [Theory]
        [InlineData(-22.91085, true)]  // 5 casas - Válido
        [InlineData(-22.910, false)]   // 3 casas - Inválido
        [InlineData(-22.123456, true)] // 6 casas - Válido
        public void Latitude_ShouldHaveMinimumFiveDecimalPlaces(double lat, bool expectedValid)
        {
            // Arrange
            var request = new FeasibilityRequest(lat, -43.18225, 500, 1);

            // Act
            var result = _validator.Validate(request);

            // Assert
            Assert.Equal(expectedValid, result.IsValid);
        }
    }
}

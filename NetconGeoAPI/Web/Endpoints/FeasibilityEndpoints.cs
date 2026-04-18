using FluentValidation;
using NetconGeoAPI.Application.Dto;
using NetconGeoAPI.Application.Services;

namespace NetconGeoAPI.Web.Endpoints
{
    public static class FeasibilityEndpoints
    {
        public static void MapFeasibilityEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/feasibility", async (
                [AsParameters] FeasibilityRequest request,
                IValidator<FeasibilityRequest> validator,
                FeasibilityService service) =>
            {
                // Validação
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var result = await service.GetNearbyElementsAsync(
                    request.Latitude,
                    request.Longitude,
                    request.Radius,
                    request.Page);

                return Results.Ok(result);
            });
        }
    }
}

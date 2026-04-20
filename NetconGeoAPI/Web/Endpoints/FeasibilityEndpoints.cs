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
                    return Results.BadRequest(new 
                    {
                        code = 400,
                        reason = validationResult.Errors,
                        message = validationResult.ToDictionary(),
                        status = "bad request",
                        timestamp = DateTime.UtcNow,

                    });
                }

                var result = await service.GetNearbyElementsAsync(
                    request.Latitude,
                    request.Longitude,
                    request.Radius,
                    request.Page,
                    request.Size);

                return Results.Ok(result);
            });
        }
    }
}

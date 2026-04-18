namespace NetconGeoAPI.Application.Dto
{
    public record FeasibilityRequest(double Latitude, double Longitude, double Radius, int Page = 1);
}

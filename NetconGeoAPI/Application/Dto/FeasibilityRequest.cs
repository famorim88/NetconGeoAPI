namespace NetconGeoAPI.Application.Dto
{
    public record FeasibilityRequest(double Latitude, double Longitude, int Radius, int Page, int Size = 20);
}

using NetconGeoAPI.Domain.Entities;
using System.Text.Json;

namespace NetconGeoAPI.Domain.Context
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Elements.Any()) return;

            var path = Path.Combine(AppContext.BaseDirectory, "dataset_v2.json");
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            using var doc = JsonDocument.Parse(json);

            var elements = new List<GeographicElement>();
            foreach (var item in doc.RootElement.EnumerateArray())
            {
                var geometry = item.GetProperty("geometry")[0];

                // Tratamento de NULL para evitar o erro de 'Number expected'
                double lat = geometry.GetProperty("y").ValueKind == JsonValueKind.Number ? geometry.GetProperty("y").GetDouble() : 0;
                double lon = geometry.GetProperty("x").ValueKind == JsonValueKind.Number ? geometry.GetProperty("x").GetDouble() : 0;

                elements.Add(new GeographicElement
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Name = item.GetProperty("name").GetString() ?? "",
                    Status = item.GetProperty("status").GetString() ?? "",
                    Latitude = lat,
                    Longitude = lon
                });
            }
            context.Elements.AddRange(elements);
            context.SaveChanges();
        }
    }
}

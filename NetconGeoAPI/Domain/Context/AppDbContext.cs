using Microsoft.EntityFrameworkCore;
using NetconGeoAPI.Domain.Entities;

namespace NetconGeoAPI.Domain.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<GeographicElement> Elements => Set<GeographicElement>();
    }
}

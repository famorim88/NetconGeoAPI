using Microsoft.EntityFrameworkCore;
using NetconGeoAPI.Domain.Context;
using NetconGeoAPI.Domain.Entities;
using NetconGeoAPI.Domain.Interfaces;
using System.Text.Json;

namespace NetconGeoAPI.Infrastructure.Repositories
{
    public class FeasibilityRepository : IFeasibilityRepository
    {
        private readonly AppDbContext _context;

        public FeasibilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GeographicElement>> GetAllActiveOrReservedAsync()
        {
            // Filtramos direto no banco em memória para performance
            return await _context.Elements
                .Where(e => e.Status == "ACTIVE" || e.Status == "RESERVED")
                .ToListAsync();
        }
    }
}

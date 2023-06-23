using System;
using CityInfo.DbContexts;
using CityInfo.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Services
{
	public class CityInfoRepository : ICityInfoRepository
	{
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
		{
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.City.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return await GetCitiesAsync();
            }

            filter = filter.Trim();
            return await _context.City
                                 .Where(x => string.Equals(x.Name, filter))
                                 .OrderBy(x => x.Name)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? filter, string? search)
        {
            if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(search))
            {
                return await GetCitiesAsync();
            }

            var collection = _context.City as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim();
                collection = collection.Where(x => string.Equals(x.Name, filter));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                collection = collection.Where(x => x.Name.Contains(search) ||
                                             (x.Description != null && x.Description.Contains(search)));
            }

            return await collection.ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return await _context.City.Where(x => x.Id == cityId)
                                          .Include(x => x.PointsOfInterest)
                                          .FirstOrDefaultAsync();
                                    
            }

            return await _context.City.Where(x => x.Id == cityId)
                                      .SingleOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestbyIdAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointOfInterest.Where(x => x.CityId == cityId && x.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterest.Where(x => x.CityId == cityId).ToListAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.City.AnyAsync(x => x.Id == cityId);
        }
    }
}


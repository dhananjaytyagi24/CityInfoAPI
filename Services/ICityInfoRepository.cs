using System;
using CityInfo.Entities;
using CityInfo.Models;

namespace CityInfo.Services
{
	public interface ICityInfoRepository
	{
		Task<IEnumerable<City>> GetCitiesAsync();

        Task<IEnumerable<City>> GetCitiesAsync(string? filter, string? search);

        // potentially can return null if city is not found
        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);

        Task<PointOfInterest?> GetPointOfInterestbyIdAsync(int cityId, int pointOfInterestId);

        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        Task<bool> CityExistsAsync(int cityId);
    }
}


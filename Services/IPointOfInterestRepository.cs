using System;
using CityInfo.Entities;
using CityInfo.Models;

namespace CityInfo.Services
{
	public interface IPointOfInterestRepository
	{
		Task<PointOfInterest> CreatePointOfInterestAsync(int cityId, CreatePointOfInterestDto createPointOfInterestDto);

		Task DeletePointOfInterestAsync(int city, PointOfInterest point);

		Task<bool> SaveChangesAsync();
	}
}


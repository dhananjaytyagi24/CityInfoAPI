using System;
using AutoMapper;
using CityInfo.DbContexts;
using CityInfo.Entities;
using CityInfo.Models;

namespace CityInfo.Services
{
	public class PointOfInterestRepository : IPointOfInterestRepository
	{
		private readonly CityInfoContext _context;
		private readonly ICityInfoRepository _cityInfoRepository;
		private readonly IMapper _mapper;

		public PointOfInterestRepository(CityInfoContext context, ICityInfoRepository cityInfoRepository, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<PointOfInterest> CreatePointOfInterestAsync(int cityId, CreatePointOfInterestDto createPointOfInterestDto)
        {
			var city = _context.City.Where(x => x.Id == cityId).FirstOrDefault();
			var point = _mapper.Map<PointOfInterest>(createPointOfInterestDto);

			city?.PointsOfInterest?.Add(point);

			await _context.SaveChangesAsync();

			return point;
		}

		public async Task DeletePointOfInterestAsync(int cityId, PointOfInterest point)
		{
			var city = await _cityInfoRepository.GetCityAsync(cityId, true);

			city?.PointsOfInterest?.Remove(point);
			await SaveChangesAsync();
		}

        public async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() >= 0;
		}
    }
}


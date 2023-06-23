using System;
using AutoMapper;
using CityInfo.Entities;
using CityInfo.Models;

namespace CityInfo.Profiles
{
	public class CityInfoProfile : Profile
	{
		public CityInfoProfile()
		{
			CreateMap<City, CityWithoutPointsOfInterestDto>();
			CreateMap<City, CityDto>();
		}	
	}
}


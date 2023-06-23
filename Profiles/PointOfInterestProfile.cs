using System;
using AutoMapper;
using CityInfo.Entities;
using CityInfo.Models;

namespace CityInfo.Profiles
{
	public class PointOfInterestProfile : Profile
	{
		public PointOfInterestProfile()
		{
            CreateMap<PointOfInterest, PointOfInterestDto>();
			CreateMap<PointOfInterestDto, PointOfInterest>();
			CreateMap<CreatePointOfInterestDto, PointOfInterest>();
            CreateMap<UpdatePointOfInterestDto, PointOfInterest>();
            CreateMap<PointOfInterest, UpdatePointOfInterestDto>();
        }
	}
}


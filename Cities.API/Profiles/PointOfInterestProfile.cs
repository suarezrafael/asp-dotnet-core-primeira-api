﻿using AutoMapper;

namespace Cities.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Models.PointOfInterestForCreationDto,Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>().ReverseMap();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
        }
    }
}

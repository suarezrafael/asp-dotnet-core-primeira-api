using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest,Models.PointOfInterestForCreationDto>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
        }
    }
}

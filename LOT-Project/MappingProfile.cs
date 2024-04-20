using AutoMapper;
using LOT_Project.Entities;
using LOT_Project.Models;

namespace LOT_Project
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Flight, FlightDto>();
            CreateMap<FlightDto, Flight>();
        }
    }
}

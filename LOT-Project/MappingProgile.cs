using AutoMapper;
using LOT_Project.Entities;
using LOT_Project.Models;

namespace LOT_Project
{
    public class MappingProgile : Profile
    {
        public MappingProgile()
        {
            CreateMap<Flight, FlightDto>();
            CreateMap<FlightDto, Flight>();
        }
    }
}

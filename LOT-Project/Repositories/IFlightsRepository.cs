using LOT_Project.Entities;
using LOT_Project.Models;

namespace LOT_Project.Repositories
{
    public interface IFlightsRepository
    {
        void Add(Flight flight);
        void Delete(Flight flight);
        IReadOnlyList<Flight> Get();
        Flight? GetByFlightNumber(string flightNumber);
        Flight? GetById(int id);
        void Update(Flight flight, UpdateFlightDto dto);
    }
}
﻿namespace LOT_Project.Models
{
    public class AddFlightDto
    {
        public string flightNumber { get; set; }

        public string departurePoint { get; set; }
        public string arrivalPoint { get; set; }
        public string aircraftType { get; set; }
    }
}

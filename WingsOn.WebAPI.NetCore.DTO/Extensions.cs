using System;
using System.Linq;
using WingsOn.Domain;

namespace WingsOn.WebAPI.NetCore.DTO
{
    public static class Extensions
    {
        #region "Person/PasengerDTO"
        public static PassengerDTO ToPassengerDto(this Person person)
        {
            if (person == null) return null;

            return new PassengerDTO
            {
                Id = person.Id,
                Name = person.Name,
                DateBirth = person.DateBirth.ToShortDateString(),
                Gender = person.Gender.ToString(),
                Address = person.Address,
                Email = person.Email
            };
        }

        public static Person ToPerson(this PassengerDTO passenger)
        {
            if (passenger == null) return null;

            return new Person
            {
                Id = passenger.Id,
                Name = passenger.Name,
                DateBirth = DateTime.Parse(passenger.DateBirth),
                Gender = Enum.TryParse(passenger.Gender, true, out GenderType genderEnum) ? genderEnum : GenderType.Male,
                Address = passenger.Address,
                Email = passenger.Email
            };
        }
        #endregion

        #region "Booking"
        public static Booking ToBooking(this BookingDTO bookingDto)
        {
            if (bookingDto == null) return null;

            return new Booking
            {
                Id = bookingDto.Id,
                Customer = bookingDto.Customer.ToPerson(),
                DateBooking = DateTime.Parse(bookingDto.DateBooking),
                Flight = bookingDto.Flight.ToFlight(),
                Number = bookingDto.Number,
                Passengers = bookingDto.Passengers.Select(passenger => passenger.ToPerson())
            };
        }

        public static BookingDTO ToBookingDto(this Booking booking)
        {
            if (booking == null) return null;

            return new BookingDTO
            {
                Id = booking.Id,
                Customer = booking.Customer.ToPassengerDto(),
                DateBooking = booking.DateBooking.ToString(),
                Flight = booking.Flight.ToFlightDto(),
                Number = booking.Number,
                Passengers = booking.Passengers.Select(person => person.ToPassengerDto())
            };
        }
        #endregion

        #region "Flight"
        public static Flight ToFlight(this FlightDTO flightDto)
        {
            if (flightDto == null) return null;

            return new Flight
            {
                Id = flightDto.Id,
                Number = flightDto.Number,
                ArrivalAirport = flightDto.ArrivalAirport.ToAirport(),
                ArrivalDate = DateTime.Parse(flightDto.ArrivalDate),
                Carrier = flightDto.Carrier.ToAirline(),
                DepartureAirport = flightDto.DepartureAirport.ToAirport(),
                DepartureDate = DateTime.Parse(flightDto.DepartureDate),
                Price = flightDto.Price
            };
        }

        public static FlightDTO ToFlightDto(this Flight flight)
        {
            if (flight == null) return null;

            return new FlightDTO
            {
                Id = flight.Id,
                Number = flight.Number,
                ArrivalAirport = flight.ArrivalAirport.ToAirportDto(),
                ArrivalDate = flight.ArrivalDate.ToString(),
                Carrier = flight.Carrier.ToAirlineDto(),
                DepartureAirport = flight.DepartureAirport.ToAirportDto(),
                DepartureDate = flight.DepartureDate.ToString(),
                Price = flight.Price
            };
        }
        #endregion

        #region "Airport"
        public static Airport ToAirport(this AirportDTO airportDto)
        {
            if (airportDto == null) return null;

            return new Airport
            {
                Id = airportDto.Id,
                City = airportDto.City,
                Code = airportDto.Code,
                Country = airportDto.Country
            };
        }

        public static AirportDTO ToAirportDto(this Airport airport)
        {
            if (airport == null) return null;

            return new AirportDTO
            {
                Id = airport.Id,
                City = airport.City,
                Code = airport.Code,
                Country = airport.Country
            };
        }
        #endregion

        #region "Airline"
        public static Airline ToAirline(this AirlineDTO airlineDto)
        {
            if (airlineDto == null) return null;

            return new Airline
            {
                Id = airlineDto.Id,
                Code = airlineDto.Code,
                Address = airlineDto.Address,
                Name = airlineDto.Name
            };
        }

        public static AirlineDTO ToAirlineDto(this Airline airline)
        {
            if (airline == null) return null;

            return new AirlineDTO
            {
                Id = airline.Id,
                Code = airline.Code,
                Address = airline.Address,
                Name = airline.Name
            };
        }
        #endregion
    }
}

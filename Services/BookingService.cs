using API.Contracts;
using API.DTOs.Bookings;
using API.Models;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public IEnumerable<BookingDTO>? GetBooking()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return null; // No bookings found
        }

        var toDTO = bookings.Select(booking => new BookingDTO
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        }).ToList();

        return toDTO; // Bookings found
    }

    public BookingDTO? GetBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return null; // No bookings found
        }

        var toDTO = new BookingDTO
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };

        return toDTO; // Bookings found
    }

    public BookingDTO? CreateBooking(NewBookingDTO newBookingDTO)
    {
        var booking = new Booking
        {
            StartDate = newBookingDTO.StartDate,
            EndDate = newBookingDTO.EndDate,
            Status = newBookingDTO.Status,
            Remarks = newBookingDTO.Remarks,
            RoomGuid = newBookingDTO.RoomGuid,
            EmployeeGuid = newBookingDTO.EmployeeGuid,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdBooking = _bookingRepository.Create(booking);
        if (createdBooking is null)
        {
            return null; // Booking not created
        }

        var toDTO = new BookingDTO
        {
            Guid = createdBooking.Guid,
            StartDate = createdBooking.StartDate,
            EndDate = createdBooking.EndDate,
            Status = createdBooking.Status,
            Remarks = createdBooking.Remarks,
            RoomGuid = createdBooking.RoomGuid,
            EmployeeGuid = createdBooking.EmployeeGuid
        };

        return toDTO; // Booking created
    }

    public int UpdateBooking(BookingDTO bookingDTO)
    {
        var isExist = _bookingRepository.IsExist(bookingDTO.Guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var getBooking = _bookingRepository.GetByGuid(bookingDTO.Guid);

        var booking = new Booking
        {
            Guid = bookingDTO.Guid,
            StartDate = bookingDTO.StartDate,
            EndDate = bookingDTO.EndDate,
            Status = bookingDTO.Status,
            Remarks = bookingDTO.Remarks,
            RoomGuid = bookingDTO.RoomGuid,
            EmployeeGuid = bookingDTO.EmployeeGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getBooking!.CreatedDate
        };

        var isUpdate = _bookingRepository.Update(booking);
        if (!isUpdate)
        {
            return 0; // Booking not found
        }

        return 1;
    }

    public int DeleteBooking(Guid guid)
    {
        var isExist = _bookingRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var booking = _bookingRepository.GetByGuid(guid);
        var isDelete = _bookingRepository.Delete(booking!);
        if (!isDelete)
        {
            return 0; // Booking not deleted
        }

        return 1;
    }
}

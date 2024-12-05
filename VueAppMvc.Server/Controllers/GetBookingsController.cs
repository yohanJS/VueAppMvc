using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VueAppMvc.Server.Data;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetBookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public GetBookingsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<BookingModel> Get()
        {
            List<BookingModel> bookings = new List<BookingModel>();
            List<AddressModel> addresses = new List<AddressModel>();
        /*
        When to Use Pagination
        Use pagination if:
        Your dataset is large (hundreds or more records).
        The data is displayed in a UI grid, table, or list where users navigate through pages.
        You expect frequent data retrievals where performance is critical. */
            bookings = _dbContext.bookings.OrderBy(b => b.Id).ToList();
            addresses = _dbContext.addresses.OrderBy(b => b.Id).ToList();
            foreach (var address in addresses)
            {
                foreach (var booking in bookings)
                {
                    if (booking.Id == address.Id)
                    {
                        booking.Address = address;
                    }                 
                }
            }
            if (bookings.Any())
            {
                return bookings;
            }
            return bookings;
        }
    }
}

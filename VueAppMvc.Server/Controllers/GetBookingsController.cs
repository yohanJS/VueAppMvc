using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        /// <summary>
        /// Returns all bookings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BookingModel Get()
        {
            BookingModel bookings = new BookingModel();
            try
            {
                /*
                When to Use Pagination
                Use pagination if:
                Your dataset is large (hundreds or more records).
                The data is displayed in a UI grid, table, or list where users navigate through pages.
                You expect frequent data retrievals where performance is critical. */
                if (_dbContext != null)
                {
                    List<UserModel> users = new List<UserModel>();
                    List<ServiceAppModel> serviceApps = new List<ServiceAppModel>();

                    if (_dbContext.users != null && _dbContext.serviceApps != null)
                    {
                        users = _dbContext.users.ToList();
                        serviceApps = _dbContext.serviceApps.ToList();
                        bookings.Users = users;
                    }
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
            }
            return bookings;
        }
    }
}

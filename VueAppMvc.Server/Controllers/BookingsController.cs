using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueAppMvc.Server.Data;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all bookings
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBookings")]
        public BookingModel Get()
        {
            BookingModel Services = new BookingModel();
            try
            {
                if (_dbContext != null)
                {
                    using (_dbContext)
                    {
                        List<UserModel> users = new List<UserModel>();
                        List<ServiceAppModel> services = new List<ServiceAppModel>();

                        if (_dbContext.users != null && _dbContext.services != null)
                        {
                            users = _dbContext.users.FromSql($"EXEC GetUsers").ToList();
                            services = _dbContext.services.FromSql($"EXEC GetServices").ToList();
                            Services.Services = services;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
            }

            return Services;
        }

        /// <summary>
        /// Inserts a new booking into the db
        /// </summary>
        /// <param name="bookFormModel"></param>
        /// <returns></returns>
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> PostAsync([FromBody] BookFormModel bookFormModel)
        {
            using (_dbContext)
            {
                List<UserModel> users = new List<UserModel>();

                if (ModelState.IsValid && _dbContext.users != null)
                {
                    users = _dbContext.users.ToList();

                    if (users.Any())
                    {
                        UserModel? existingUser = users.FirstOrDefault(us =>
                            us.Email == bookFormModel.Email ||
                            us.Name == bookFormModel.Name ||
                            us.Phone == bookFormModel.Phone);

                        if (existingUser != null)
                        {
                            if (string.IsNullOrEmpty(existingUser.Street) && !string.IsNullOrEmpty(bookFormModel.Street))
                                existingUser.Street = bookFormModel.Street;

                            if (string.IsNullOrEmpty(existingUser.City) && !string.IsNullOrEmpty(bookFormModel.City))
                                existingUser.City = bookFormModel.City;

                            if (string.IsNullOrEmpty(existingUser.State) && !string.IsNullOrEmpty(bookFormModel.State))
                                existingUser.State = bookFormModel.State;

                            if (string.IsNullOrEmpty(existingUser.Zip) && !string.IsNullOrEmpty(bookFormModel.ZipCode))
                                existingUser.Zip = bookFormModel.ZipCode;

                            await _dbContext.SaveChangesAsync();

                            ServiceAppModel serviceAppModel = new ServiceAppModel
                            {
                                Service = bookFormModel.Service ?? "",
                                Date = bookFormModel.Date ?? "",
                                Time = bookFormModel.Time ?? ""
                            };

                            if (_dbContext.services != null)
                            {
                                _dbContext.services.Add(serviceAppModel);
                                await _dbContext.SaveChangesAsync();
                                return Ok(string.Format("Existing user {0}. New service added to db...", existingUser.Name));
                            }
                        }
                        else
                        {
                            return await CreateNewUserAndService(bookFormModel);
                        }
                    }
                    else
                    {
                        return await CreateNewUserAndService(bookFormModel);
                    }
                }

                return BadRequest("Something went wrong...");
            }
        }
        
        [HttpPost("DeleteBooking")]
        public async Task<IActionResult> PostAsync([FromBody] DeleteModel deleteModel)
        {
            using (var dbContext = _dbContext)
            {
                if (dbContext.services != null)
                {
                    DbSet<ServiceAppModel> services = dbContext.services;
                    foreach (ServiceAppModel service in services)
                    {
                        if (service.Id.Equals(deleteModel.serviceId))
                        {
                            services.Remove(service);
                            await dbContext.SaveChangesAsync();
                            return Ok();
                        }
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            return BadRequest(string.Format("Could not delete service with booking Id:{0}", deleteModel.serviceId));
        }
        /// <summary>
        /// Helper function
        /// </summary>
        /// <param name="bookFormModel"></param>
        /// <returns></returns>
        private async Task<IActionResult> CreateNewUserAndService(BookFormModel bookFormModel)
        {
            ServiceAppModel newServiceApp = new ServiceAppModel
            {
                Service = bookFormModel.Service ?? "",
                Date = bookFormModel.Date ?? "",
                Time = bookFormModel.Time ?? ""
            };

            if (_dbContext.services != null)
            {
                _dbContext.services.Add(newServiceApp);
                await _dbContext.SaveChangesAsync();
            }

            UserModel newUser = new UserModel
            {
                ServiceId = newServiceApp.Id,
                Name = bookFormModel.Name ?? "",
                Email = bookFormModel.Email ?? "",
                Phone = bookFormModel.Phone ?? "",
                Street = bookFormModel.Street ?? "",
                City = bookFormModel.City ?? "",
                State = bookFormModel.State ?? "",
                Zip = bookFormModel.ZipCode ?? ""
            };

            if (_dbContext.users != null)
            {
                _dbContext.users.Add(newUser);
                await _dbContext.SaveChangesAsync();
            }

            return Ok(string.Format("New user created {0}", newUser.Name));
        }
    }
}
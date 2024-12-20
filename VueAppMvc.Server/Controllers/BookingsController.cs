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
            BookingModel bookings = new BookingModel();
            try
            {
                if (_dbContext != null)
                {
                    using (_dbContext)
                    {
                        List<UserModel> users = new List<UserModel>();
                        List<ServiceAppModel> serviceApps = new List<ServiceAppModel>();

                        if (_dbContext.users != null && _dbContext.serviceApps != null)
                        {
                            users = _dbContext.users.FromSql($"EXEC GetAllUsers").ToList();
                            serviceApps = _dbContext.serviceApps.FromSql($"EXEC GetAllServiceApps").ToList();
                            bookings.Users = users;
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

            return bookings;
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
                                UserId = existingUser.Id,
                                Service = bookFormModel.Service ?? "",
                                Date = bookFormModel.Date ?? "",
                                Time = bookFormModel.Time ?? ""
                            };

                            if (_dbContext.serviceApps != null)
                            {
                                _dbContext.serviceApps.Add(serviceAppModel);
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
                if (dbContext.serviceApps != null)
                {
                    DbSet<ServiceAppModel> services = dbContext.serviceApps;
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
        private async Task<IActionResult> CreateNewUserAndService(BookFormModel bookFormModel)
        {
            UserModel newUser = new UserModel
            {
                Name = bookFormModel.Name ?? "",
                Email = bookFormModel.Email ?? "",
                Phone = bookFormModel.Phone ?? "",
                Street = bookFormModel.Street ?? "",
                City = bookFormModel.City ?? "",
                State = bookFormModel.State ?? "",
                Zip = bookFormModel.ZipCode ?? ""
            };

            _dbContext.users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            ServiceAppModel newServiceApp = new ServiceAppModel
            {
                UserId = newUser.Id,
                Service = bookFormModel.Service ?? "",
                Date = bookFormModel.Date ?? "",
                Time = bookFormModel.Time ?? ""
            };

            if (_dbContext.serviceApps != null)
            {
                _dbContext.serviceApps.Add(newServiceApp);
                await _dbContext.SaveChangesAsync();
            }

            return Ok(string.Format("New user created {0}", newUser.Name));
        }
    }
}
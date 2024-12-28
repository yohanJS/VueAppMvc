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
        public List<ResponseDataModel> Get()
        {
            List<ResponseDataModel> response = new List<ResponseDataModel>();
            try
            {
                if (_dbContext != null)
                {
                    using (_dbContext)
                    {
                        List<UserModel> users = new List<UserModel>();
                        List<ServiceModel> services = new List<ServiceModel>();

                        if (_dbContext.users != null && _dbContext.services != null)
                        {
                            users = _dbContext.users.ToList();
                            services = _dbContext.services.ToList();

                            // Group services by date
                            var groupedServices = services.GroupBy(s => s.Date).ToList();

                            foreach (var group in groupedServices)
                            {
                                ResponseDataModel responseDataModel = new ResponseDataModel
                                {
                                    ServiceDate = group.Key, // Use the actual service date
                                    Services = new List<Service>() // Initialize services for this date
                                };

                                foreach (var service in group)
                                {
                                    // Check if this time is already added for this serviceDate in the Services list
                                    var existingService = responseDataModel.Services
                                        .FirstOrDefault(s => s.Time == service.Time);

                                    if (existingService == null) // Avoid adding duplicates for the same time
                                    {
                                        // Find the user for the service (if any)
                                        var userForService = users.FirstOrDefault(user => user.Id.Equals(service.UserId)); // Assuming `UserId` is the matching property for `service`

                                        if (userForService != null)
                                        {
                                            // Add the service with the matched user to the Services list
                                            responseDataModel.Services.Add(new Service
                                            {
                                                Time = service.Time,
                                                Users = new List<UserModel> { userForService }
                                            });
                                        }
                                    }
                                    else
                                    {
                                        // If the service already exists, just add the new user to the existing service's users list
                                        var userForService = users.FirstOrDefault(user => user.Id.Equals(service.UserId));
                                        if (userForService != null && !existingService.Users.Any(u => u.Id == userForService.Id))
                                        {
                                            existingService.Users.Add(userForService);
                                        }
                                    }
                                }

                                // Add the responseDataModel for this particular date
                                response.Add(responseDataModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception for better diagnostics
                throw new ApplicationException("An error occurred while fetching bookings.", ex);
            }

            return response;
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
                        UserModel? existingUser = users.FirstOrDefault(us => us.Email == bookFormModel.Email || us.Phone == bookFormModel.Phone);

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

                            ServiceModel serviceAppModel = new ServiceModel
                            {
                                UserId = existingUser.Id,
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
                    DbSet<ServiceModel> services = dbContext.services;
                    foreach (ServiceModel service in services)
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
        /// Helper function that creates a new User and Service
        /// </summary>
        /// <param name="bookFormModel"></param>
        /// <returns></returns>
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

            if (_dbContext.users != null)
            {
                _dbContext.users.Add(newUser);
                await _dbContext.SaveChangesAsync();
            }

            ServiceModel newServiceApp = new ServiceModel
            {
                UserId = newUser.Id,
                Service = bookFormModel.Service ?? "",
                Date = bookFormModel.Date ?? "",
                Time = bookFormModel.Time ?? ""
            };

            if (_dbContext.services != null)
            {
                _dbContext.services.Add(newServiceApp);
                await _dbContext.SaveChangesAsync();
            }

            return Ok(string.Format("New user created {0}", newUser.Name));
        }
    }
}
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
        public List<ResponseDataModel> Get([FromQuery] string businessId)
        {
            List<ResponseDataModel> response = new List<ResponseDataModel>();
            try
            {
                if (_dbContext != null)
                {
                    using (_dbContext)
                    {
                        // Users in db
                        List<UserModel> users = new List<UserModel>();
                        // Services in db
                        List<ServiceModel> services = new List<ServiceModel>();

                        if (_dbContext.users != null && _dbContext.services != null)
                        {
                            users = _dbContext.users.ToList();
                            services = _dbContext.services.Where(s => s.BusinessId.Equals(businessId)).ToList();

                            // Group services by date
                            var groupedServicesByDate = services.GroupBy(s => s.Date)
                                .Select(group => new ResponseDataModel
                                {
                                    ServiceDate = group.Key,
                                    Services = group.Select(s =>
                                    {
                                        var user = users.FirstOrDefault(u => u.Id.Equals(s.UserId));
                                        return new Service
                                        {
                                            Time = s.Time,
                                            Name = user != null ? user.Name : string.Empty,
                                            ServiceId = s.Id,
                                            ServiceName = s.Service,
                                            Phone = user != null ? user.Phone : string.Empty,
                                            Email = user != null ? user.Email : string.Empty,
                                            Address = user != null ? string.Format("{0} {1} {2} {3}", user.Street, user.City, user.State, user.Zip) : string.Empty,
                                        };
                                    }).ToList()
                                }).ToList();
                            foreach (var service in groupedServicesByDate)
                            {
                                if (service != null && service.Services != null)
                                {
                                    //orders the services by date
                                    service.Services = service.Services.OrderBy(s => DateTime.Parse(s.Time)).ToList();
                                }
                            }
                            //Orders the record by date
                            groupedServicesByDate = groupedServicesByDate.OrderBy(s => DateTime.Parse(s.ServiceDate)).ToList();

                            response = groupedServicesByDate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching bookings.", ex);
            }
            return response;
        }

        /// <summary>
        /// Returns all bookings
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTimes")]
        public List<string> GetTimes([FromQuery] string serviceDate)
        {
            List<string> takenTimes = new List<string>();
            try
            {
                if (_dbContext != null)
                {
                    using (_dbContext)
                    {
                        List<ServiceModel> services = new List<ServiceModel>();

                        if (_dbContext.services != null)
                        {
                            services = _dbContext.services.ToList();
                            takenTimes = services.Where(s => s.Date.Equals(serviceDate)).Select(s => s.Time).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching bookings.", ex);
            }
            return takenTimes;
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
                                BusinessId = bookFormModel.BusinessId,
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
            if (_dbContext.services != null)
            {
                var serviceToBeRemoved = await _dbContext.services.FirstOrDefaultAsync(s => s.Id == deleteModel.serviceId);

                if (serviceToBeRemoved != null)
                {
                    _dbContext.services.Remove(serviceToBeRemoved);
                    await _dbContext.SaveChangesAsync();
                    return Ok("Service deleted");
                }
            }
            return BadRequest($"Could not delete service with Id: {deleteModel.serviceId}");
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
                BusinessId = bookFormModel.BusinessId ?? "",
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
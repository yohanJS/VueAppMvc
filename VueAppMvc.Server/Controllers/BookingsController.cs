using Microsoft.AspNetCore.Mvc;
using VueAppMvc.Server.Data;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Controllers
{
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
        [HttpGet]
        public BookingModel Get()
        {
            BookingModel bookings = new BookingModel();
            try
            {
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
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookFormModel bookFormModel)
        {
            if (ModelState.IsValid && _dbContext.users != null)
            {
                List<UserModel> users = _dbContext.users.ToList();
                if (users.Any())
                {
                    UserModel? existingUser = users.Where(us => us.Email == bookFormModel.Email
                                            || us.Name == bookFormModel.Name
                                            || us.Phone == bookFormModel.Phone).FirstOrDefault();
                    if (existingUser != null)
                    {
                        if (string.IsNullOrEmpty(existingUser.Street) && !string.IsNullOrEmpty(bookFormModel.Street))
                        {
                            existingUser.Street = bookFormModel.Street;
                        }
                        if (string.IsNullOrEmpty(existingUser.City) && !string.IsNullOrEmpty(bookFormModel.City))
                        {
                            existingUser.City = bookFormModel.City;
                        }
                        if (string.IsNullOrEmpty(existingUser.State) && !string.IsNullOrEmpty(bookFormModel.State))
                        {
                            existingUser.State = bookFormModel.State;
                        }
                        if (string.IsNullOrEmpty(existingUser.Zip) && !string.IsNullOrEmpty(bookFormModel.ZipCode))
                        {
                            existingUser.Zip = bookFormModel.ZipCode;
                        }

                        await _dbContext.SaveChangesAsync();

                        ServiceAppModel serviceAppModel = new ServiceAppModel();
                        serviceAppModel.UserId = existingUser.Id;
                        serviceAppModel.Service = !string.IsNullOrEmpty(bookFormModel.Service) ?            bookFormModel.Service : "";
                        serviceAppModel.Date = !string.IsNullOrEmpty(bookFormModel.Date) ? bookFormModel.Date : "";
                        serviceAppModel.Time = !string.IsNullOrEmpty(bookFormModel.Time) ? bookFormModel.Time : "";

                        if (_dbContext.serviceApps != null)
                        {
                            _dbContext.serviceApps.Add(serviceAppModel);
                            await _dbContext.SaveChangesAsync();
                            return Ok(string.Format("Existing user {0}. New service addedd to db...", existingUser.Name));
                        }
                    }
                }
                else
                {
                    UserModel newUser = new UserModel();
                    if (!string.IsNullOrEmpty(bookFormModel.Name))
                    {
                        newUser.Name = bookFormModel.Name;
                    }
                    if (!string.IsNullOrEmpty(bookFormModel.Email))
                    {
                        newUser.Email = bookFormModel.Email;
                    }
                    if (!string.IsNullOrEmpty(bookFormModel.Phone))
                    {
                        newUser.Phone = bookFormModel.Phone;
                    }

                    if (!string.IsNullOrEmpty(bookFormModel.Street))
                    {
                        newUser.Street = bookFormModel.Street;
                    }

                    if (!string.IsNullOrEmpty(bookFormModel.City))
                    {
                        newUser.City = bookFormModel.City;
                    }

                    if (!string.IsNullOrEmpty(bookFormModel.State))
                    {
                        newUser.State = bookFormModel.State;
                    }

                    if (!string.IsNullOrEmpty(bookFormModel.ZipCode))
                    {
                        newUser.Zip = bookFormModel.ZipCode;
                    }

                    _dbContext.users.Add(newUser);
                    await _dbContext.SaveChangesAsync();

                    ServiceAppModel newserviceApp = new ServiceAppModel();
                    newserviceApp.UserId = newUser.Id;
                    if (!string.IsNullOrEmpty(bookFormModel.Service))
                    {
                        newserviceApp.Service = bookFormModel.Service;
                    }

                    if (!string.IsNullOrEmpty(bookFormModel.Date))
                    {
                        newserviceApp.Date = bookFormModel.Date;
                    }

                    if (!string.IsNullOrEmpty(bookFormModel.Time))
                    {
                        newserviceApp.Time = bookFormModel.Time;
                    }

                    if (_dbContext.serviceApps != null)
                    {
                        _dbContext.serviceApps.Add(newserviceApp);
                        await _dbContext.SaveChangesAsync();
                    }

                    return Ok(string.Format("New user created {0}", newUser.Name));
                }
            }
            return BadRequest();
        }
    }
}

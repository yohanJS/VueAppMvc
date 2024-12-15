using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueAppMvc.Server.Data;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DeleteBookedServiceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public DeleteBookedServiceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
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
    }
}

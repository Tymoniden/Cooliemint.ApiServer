using Cooliemint.ApiServer.Services.Repositories;
using Cooliemint.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController(NotificationRepository notificationRepository) : ControllerBase
    {

        // GET: api/<NotifcationController>
        [HttpGet]
        public IAsyncEnumerable<Notification> Get(CancellationToken cancellationToken)
        {
            return notificationRepository.GetAll(0, 100, cancellationToken);
        }

        // GET api/<NotifcationController>/5
        [HttpGet("{id}")]
        public async Task<Notification> Get(long id, CancellationToken cancellationToken)
        {
            return await notificationRepository.Get(id, cancellationToken);
        }

        // POST api/<NotifcationController>
        [HttpPost]
        public async Task<Notification> Post([FromBody] Notification notification, CancellationToken cancellationToken)
        {
            return await notificationRepository.Add(notification, cancellationToken);
        }

        // PUT api/<NotifcationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Notification notification)
        {
        }

        // DELETE api/<NotifcationController>/5
        [HttpDelete("{id}")]
        public Task Delete(long id, CancellationToken cancellationToken)
        {
            return notificationRepository.Remove(id, cancellationToken);
        }
    }
}

using Cooliemint.ApiServer.Services.Repositories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserRepository userRepository, UserNotificationRepository userNotificationRepository) : ControllerBase
    {

        // GET: api/<UserController>
        [HttpGet]
        public IAsyncEnumerable<User> Get(CancellationToken cancellationToken)
        {
            return userRepository.GetAll(cancellationToken);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Task<User> Get(int id, CancellationToken cancellationToken)
        {
            return userRepository.Get(id, cancellationToken);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<User> Post([FromBody] User value, CancellationToken cancellationToken)
        {
            return await userRepository.Add(value, cancellationToken);
        }

        [HttpPost("{id}/Notification")]
        public async Task<User> PostNotification(int id, UserNotification value, CancellationToken cancellationToken)
        {
            await userNotificationRepository.AddUserNotification(id, value.NotificationId, value.IsActive, cancellationToken);
            return await userRepository.Get(id, cancellationToken);
        }

        [HttpPut("{userId}/Notification/{userNotificationId}")]
        public async Task<User> PutNotification(int userId, int userNotificationId, UserNotification value, CancellationToken cancellationToken)
        {
            await userNotificationRepository.UpdateUserNotification(userNotificationId, value.IsActive, cancellationToken);
            return await userRepository.Get(userId, cancellationToken);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<User> Put(int id, [FromBody] User user, CancellationToken cancellationToken)
        {
            await userRepository.Update(id, user, cancellationToken);
            return await userRepository.Get(id, cancellationToken);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await userRepository.Remove(id, cancellationToken);
        }
    }
}

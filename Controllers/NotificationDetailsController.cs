using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/NotificationDetails")]
    [ApiController]
    public class NotificationDetailsController(NotificationDetailsRepository notificationDetailsRepository) : ControllerBase
    {
        [HttpGet()]
        public IAsyncEnumerable<NotificationDetailsDto> Get(int skip, int take = 50, CancellationToken cancellationToken = default)
        {
            return notificationDetailsRepository.GetNotificationDetailsUntracked(skip, take, cancellationToken);
        }

        [HttpPut()]
        public async Task<NotificationDetailsDto> Update(int id, [FromBody] NotificationDetailsDto notificationDetailsDto, CancellationToken cancellationToken)
        {
            return await notificationDetailsRepository.UpdateNotificationDetails(id, notificationDetailsDto, cancellationToken);
        }
    }
}

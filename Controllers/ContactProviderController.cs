using Cooliemint.ApiServer.Services.Repositories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class ContactProviderController(ContactProviderRepository contactProviderRepository, UserRepository userRepository) : ControllerBase
    {
        [HttpGet("User/{id}/ContactProvider")]
        public IAsyncEnumerable<ContactProviderDto> GetAll(int id, CancellationToken cancellationToken)
        {
            return contactProviderRepository.GetAll(id, cancellationToken);
        }

        [HttpGet("ContactProvider/{id}")]
        [ProducesResponseType(typeof(ContactProviderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await contactProviderRepository.Get(id, cancellationToken));

            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost("ContactProvider")]
        [ProducesResponseType(typeof(ContactProviderDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] ContactProviderDto contactProvider, CancellationToken cancellationToken)
        {
            return Ok(await contactProviderRepository.Create(contactProvider, cancellationToken));
        }

        [HttpPost("User/{id}/ContactProvider")]
        [ProducesResponseType(typeof(ContactProviderDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostToUser(int id, [FromBody] ContactProviderDto contactProvider, CancellationToken cancellationToken)
        {
            var user = await userRepository.Get(id, cancellationToken);
            if(user == null)
                return NotFound($"User not found id({id})");

            return Ok(await contactProviderRepository.CreateForUser(contactProvider, user, cancellationToken));
        }

        [HttpPut("ContactProvider/{id}")]
        [ProducesResponseType(typeof(ContactProviderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] ContactProviderDto contactProvider, CancellationToken cancellationToken)
        {
            try
            {
                if (id != contactProvider.id)
                    throw new ArgumentException($"contactprovider.Id({contactProvider.id}) != id({id}) ");

                return Ok(await contactProviderRepository.Update(contactProvider, cancellationToken));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("ContactProvider/{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await contactProviderRepository.Delete(id, cancellationToken);
        }
    }
}

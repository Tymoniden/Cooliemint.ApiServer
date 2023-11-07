using Cooliemint.ApiServer.Services.Messaging.Pushover;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushoverAccountController : ControllerBase
    {
        private readonly PushoverAccountStore _pushoverAccountStore;

        public PushoverAccountController(PushoverAccountStore pushoverAccountStore)
        {
            _pushoverAccountStore = pushoverAccountStore ?? throw new ArgumentNullException(nameof(pushoverAccountStore));
        }

        // GET: api/<PushoverAccountController>
        [HttpGet]
        public List<PushoverAccountDto> Get()
        {
            return _pushoverAccountStore.GetAll();
        }

        // GET api/<PushoverAccountController>/5
        [HttpGet("{id}")]
        public PushoverAccountDto? Get(int id)
        {
            return _pushoverAccountStore.Get(id);
        }

        // POST api/<PushoverAccountController>
        [HttpPost]
        public void Post([FromBody] PushoverAccountDto value)
        {
            _pushoverAccountStore.Add(value);
        }

        // PUT api/<PushoverAccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] PushoverAccountDto value)
        {
            value.Id = id;
            _pushoverAccountStore.Add(value);
        }

        // DELETE api/<PushoverAccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _pushoverAccountStore.Remove(id);
        }
    }
}

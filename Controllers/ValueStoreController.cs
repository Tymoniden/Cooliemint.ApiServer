using Cooliemint.ApiServer.Services.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueStoreController(ValueStore valueStore) : ControllerBase
    {
        // GET: api/<ValueStoreController>
        [HttpGet]
        public Dictionary<string, ValueSet> Get()
        {
            return valueStore.GetEverything();
        }

        // GET api/<ValueStoreController>/5
        [HttpGet("{key}")]
        public ValueSet? Get(string key)
        {
            return valueStore.GetValue(key) as ValueSet;
        }

        // POST api/<ValueStoreController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValueStoreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValueStoreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

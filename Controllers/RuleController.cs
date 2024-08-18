using Cooliemint.ApiServer.Shared.Dtos;
using Cooliemint.ApiServer.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController(RuleRepository ruleRepository, RulePartRepository rulePartRepository) : ControllerBase
    {
        // GET: api/<RuleController>
        [HttpGet]
        public IAsyncEnumerable<RuleDto> Get(CancellationToken cancellationToken, int skip, int take = 50)
        {
            if(skip < 0) throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take));

            return ruleRepository.GetRulesUntracked(skip, take, cancellationToken);
        }

        // GET api/<RuleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RuleController>
        [HttpPost]
        public async Task Post([FromBody] RuleDto rule, CancellationToken cancellationToken)
        {
            await ruleRepository.CreateRule(rule, cancellationToken);
        }

        // PUT api/<RuleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RuleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{ruleId}/RulePart")]
        public IAsyncEnumerable<RulePartDto> GetByRule(CancellationToken cancellationToken, int ruleId, int skip, int take = 50)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take));

            return rulePartRepository.GetRulePartsByRule(ruleId, skip, take, cancellationToken);
        }
    }
}

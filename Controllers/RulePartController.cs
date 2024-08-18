using Cooliemint.ApiServer.Services.Repositories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulePartController(RulePartRepository rulePartRepository) : ControllerBase
    {
        [HttpGet]
        public IAsyncEnumerable<RulePartDto> Get(CancellationToken cancellationToken, int skip, int take = 50)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take));

            return rulePartRepository.GetRulePartsUntracked(skip, take, cancellationToken);
        }

        // GET api/<RulePartController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RulePartController>
        [HttpPost]
        public async Task<RulePartDto> Post([FromBody] RulePartDto rulePart, CancellationToken cancellationToken)
        {
            return await rulePartRepository.CreateRulePart(rulePart, cancellationToken);
        }

        // PUT api/<RulePartController>/5
        [HttpPut("{id}")]
        public async Task<RulePartDto> Put(int id, [FromBody] RulePartDto rulePart, CancellationToken cancellationToken)
        {
            return await rulePartRepository.UpdateRulePart(id, rulePart, cancellationToken);
        }

        // DELETE api/<RulePartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

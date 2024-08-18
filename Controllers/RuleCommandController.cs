using Cooliemint.ApiServer.Services.Repositories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class RuleCommandController(RuleCommandRepository ruleCommandRepository) : ControllerBase
    {
        // GET: api/<RuleCommandController>
        [HttpGet("RuleCommand")]
        public IAsyncEnumerable<RuleCommandDto> Get(int skip, int take = 50, CancellationToken cancellationToken = default)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take));

            return ruleCommandRepository.GetRuleCommandsUntracked(skip, take, cancellationToken);
        }

        // GET: api/Rule/id/R<RuleCommandController>
        [HttpGet("Rule/{ruleId}/RuleCommand")]
        public IAsyncEnumerable<RuleCommandDto> GetByRuleId(int ruleId, int skip, int take = 50, CancellationToken cancellationToken = default)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take));

            return ruleCommandRepository.GetRuleCommandsByRuleIdUntracked(ruleId, skip, take, cancellationToken);
        }
    }
}

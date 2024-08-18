using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class RuleCommandRepository(IDbContextFactory<CooliemintDbContext> dbContextFactory, RuleCommandFactory ruleCommandFactory)
    {
        public async IAsyncEnumerable<RuleCommandDto> GetRuleCommandsUntracked(int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            await foreach (var rule in ctx.RuleCommands.AsSplitQuery().AsNoTracking().OrderBy(rc => rc.Id).Skip(skip).Take(take).AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                yield return ruleCommandFactory.CreateRuleCommand(rule);
            }
        }

        public async IAsyncEnumerable<RuleCommandDto> GetRuleCommandsByRuleIdUntracked(int ruleId, int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var query = ctx.Rules.AsSplitQuery().AsNoTracking()
                .Include(r => r.Commands)
                .Where(r => r.Id == ruleId)
                .OrderBy(rc => rc.Id)
                .Skip(skip)
                .Take(take)
                .SelectMany(r => r.Commands)
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken);

            await foreach (var ruleCommand in query)
            {
                yield return ruleCommandFactory.CreateRuleCommand(ruleCommand);
            }
        }
    }
}

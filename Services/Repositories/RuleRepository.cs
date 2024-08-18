using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Factories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class RuleRepository(IDbContextFactory<CooliemintDbContext> dbContextFactory, RuleFactory ruleFactory)
    {
        public async IAsyncEnumerable<RuleDto> GetRulesUntracked(int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            await foreach(var rule in ctx.Rules.AsSplitQuery().AsNoTracking().OrderBy(r => r.Id).Skip(skip).Take(take).AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                yield return ruleFactory.CreateRule(rule);
            }
        }

        internal async Task<RuleDto> CreateRule(RuleDto rule, CancellationToken cancellationToken)
        {
            using var ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var model = ruleFactory.CreateRuleModel(rule);
            var entity = ctx.Rules.Add(model);
            await ctx.SaveChangesAsync(cancellationToken);

            return ruleFactory.CreateRule(entity.Entity);
        }
    }
}

using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Factories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class RulePartRepository(IDbContextFactory<CooliemintDbContext> dbContextFactory, RulePartFactory rulePartFactory)
    {
        public async IAsyncEnumerable<RulePartDto> GetRulePartsUntracked(int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            await foreach (var rulePart in ctx.RuleParts.AsSplitQuery().AsNoTracking().Skip(skip).Take(take).AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                yield return rulePartFactory.CreateRulePartDto(rulePart);
            }
        }

        public async IAsyncEnumerable<RulePartDto> GetRulePartsByRule(int ruleId, int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var ruleParts = ctx.Rules
                .Include(r => r.Parts)
                .Where(r => r.Id == ruleId)
                .SelectMany(r => r.Parts)
                .Skip(skip)
                .Take(take)
                .AsSplitQuery()
                .AsNoTracking()
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken);

            await foreach (var rulePart in ruleParts)
            {
                yield return rulePartFactory.CreateRulePartDto(rulePart);
            }
        }

        public async Task<RulePartDto> CreateRulePart(RulePartDto rulePart, CancellationToken cancellationToken)
        {
            using var ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var rulePartModel = rulePartFactory.CreateRulePartModel(rulePart);
            var entity = ctx.Add(rulePartModel);

            await ctx.SaveChangesAsync(cancellationToken);
            return rulePartFactory.CreateRulePartDto(entity.Entity);
        }

        public async Task<RulePartDto> UpdateRulePart(int id, RulePartDto rulePart, CancellationToken cancellationToken)
        {
            using var ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var rulePartModel = rulePartFactory.CreateRulePartModel(rulePart);
            var entity = await ctx.RuleParts.FirstOrDefaultAsync(r => r.Id == id);
            if (entity == null)
            {
                throw new ArgumentException("Could not find RulePart with {id} = id", nameof(id));
            }
            
            entity.Description = rulePart.Description;
            entity.Descriptor = rulePart.Descriptor;
            entity.OperandLeft = rulePart.OperandLeft;
            entity.OperandRight = rulePart.OperandRight;
            entity.Operation = rulePart.Operation;

            await ctx.SaveChangesAsync(cancellationToken);
            return rulePartFactory.CreateRulePartDto(entity);
        }
    }
}

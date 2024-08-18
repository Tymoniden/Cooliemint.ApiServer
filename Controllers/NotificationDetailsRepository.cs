using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Cooliemint.ApiServer.Controllers
{
    public class NotificationDetailsRepository(IDbContextFactory<CooliemintDbContext> dbContextFactory, NotificationDetailsFactory notificationDetailsFactory)
    {
        public async IAsyncEnumerable<NotificationDetailsDto> GetNotificationDetailsUntracked(int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var query = ctx.NotificationDetails.AsSplitQuery().AsNoTracking()
                .OrderBy(nd => nd.Id)
                .Skip(skip)
                .Take(take)
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken);

            await foreach (var notificationDetail in query)
            {
                yield return notificationDetailsFactory.CreateNotificationDetailsDto(notificationDetail);
            }
        }

        public async Task<NotificationDetailsDto> UpdateNotificationDetails(int id, NotificationDetailsDto notificationDetailsDto, CancellationToken cancellationToken)
        {
            using CooliemintDbContext ctx = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.NotificationDetails.FirstOrDefaultAsync(nd => nd.Id == id, cancellationToken);
            if(entity == null)
            {
                throw new ArgumentException("Could not find NotificationDetails" , nameof(id));
            }

            entity.Title = notificationDetailsDto.Title;
            entity.Description = notificationDetailsDto.Description;
            entity.LongDescription = notificationDetailsDto.LongDescription;
            entity.Url = notificationDetailsDto.Url;
            entity.Icon = notificationDetailsDto.Icon;

            await ctx.SaveChangesAsync(cancellationToken);
            return notificationDetailsFactory.CreateNotificationDetailsDto(entity);
        }
    }
}

﻿using Cooliemint.ApiServer.Services.Factories;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class NotificationRepository(Models.CooliemintDbContext dbContext, NotificationFactory notificationFactory)
    {
        public async IAsyncEnumerable<NotificationDto> GetAll(int skip, int take,[EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach(var notification in dbContext.Notifications.Skip(skip).Take(take).AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                yield return notificationFactory.CreateNotification(notification);
            }
        }

        public async Task<NotificationDto> Get(long id, CancellationToken cancellationToken)
        {
            return notificationFactory.CreateNotification(await dbContext.Notifications.FirstAsync(n => n.Id == id, cancellationToken));
        }

        public async Task<NotificationDto> Add(NotificationDto notification, CancellationToken cancellationToken)
        {
            var notificationModel = dbContext.Notifications.Add(new() { Title = notification.Title , Description = notification.Description });
            await dbContext.SaveChangesAsync(cancellationToken);

            return notificationFactory.CreateNotification(notificationModel.Entity);
        }

        public void Update()
        {
            // TODO implement!
            throw new NotImplementedException();
        }

        public Task Remove(NotificationDto notification, CancellationToken cancellationToken)
        {
            return Remove(notification.Id, cancellationToken);
        }

        internal async Task Remove(long id, CancellationToken cancellationToken)
        {
            await dbContext.Notifications.Where(n => n.Id == id).ExecuteDeleteAsync(cancellationToken);
        }
    }
}

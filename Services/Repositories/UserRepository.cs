using Cooliemint.ApiServer.Services.Factories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using CooliemintDbContext = Cooliemint.ApiServer.Models.CooliemintDbContext;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class UserRepository(CooliemintDbContext dbContext, UserFactory userFactory)
    {
        public async IAsyncEnumerable<Shared.Dtos.User> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach(var user in dbContext.Users.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                yield return userFactory.CreateUser(user);
            }
        }

        public async Task<Shared.Dtos.User> Get(int id, CancellationToken cancellationToken)
        {
            var user = await dbContext
                .Users.Include(u => u.Notifications)
                .ThenInclude(n => n.Notification)
                .FirstAsync(x => x.Id == id, cancellationToken);
            return userFactory.CreateUser(user);
        }

        public async Task<Shared.Dtos.User> Add(Shared.Dtos.User user, CancellationToken cancellationToken)
        {
            var userModel = userFactory.CreateUserModel(user);
            dbContext.Users.Add(userModel);
            await dbContext.SaveChangesAsync();

            return await Get(userModel.Id, cancellationToken);
        }

        public async Task Update(int userId, Shared.Dtos.User user, CancellationToken cancellationToken)
        {
            var userModel = await dbContext.Users.Include(u => u.Notifications).FirstAsync(u => u.Id == userId, cancellationToken);
            userModel.Name = user.Name;
            userModel.Email = user.Email;
            foreach(var notification in userModel.Notifications.Where(n => user.Notifications.Any(un => un.Id == n.Id )))
            {
                notification.IsActive = user.Notifications.First(n => n.Id == notification.Id).IsActive;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(Shared.Dtos.User user, CancellationToken cancellationToken)
        {
            await dbContext.Users.Where(u => u.Id == user.Id).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task Remove(int userId, CancellationToken cancellationToken)
        {
            await dbContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync(cancellationToken);
        }
    }
}

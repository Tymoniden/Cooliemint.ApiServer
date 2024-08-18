using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class ContactProviderRepository(IDbContextFactory<CooliemintDbContext> dbContextFactory)
    {
        public async IAsyncEnumerable<ContactProviderDto> GetAll(int userId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var ctx = dbContextFactory.CreateDbContext();
            var userContactProviders = ctx.UserContactProviders
                .Include(x => x.ContactProvider)
                .Where(ucp => ucp.User.Id == userId)
                .AsSplitQuery()
                .AsNoTracking()
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken);

            await foreach(var userContactProvider in userContactProviders)
            {
                yield return ContactProviderFactory.CreateContactProviderDto(userContactProvider.ContactProvider);
            }
        }

        public async Task<ContactProviderDto> Get(int id, CancellationToken cancellationToken)
        {
            using var ctx = dbContextFactory.CreateDbContext();

            var contactProvider = await ctx.ContactProviders.FirstOrDefaultAsync(cp => cp.Id == id, cancellationToken);
            if (contactProvider == null)
                throw new ArgumentException($"ContactProvider with id: {id} not found.", nameof(id));

            return ContactProviderFactory.CreateContactProviderDto(contactProvider);
        }

        public async Task<ContactProvider> Create(ContactProviderDto contactProviderDto, CancellationToken cancellationToken)
        {
            var contactProviderModel = ContactProviderFactory.CreateContactProviderModel(contactProviderDto);

            using var ctx = dbContextFactory.CreateDbContext();
            var addedEntity = ctx.Add(contactProviderModel);
            await ctx.SaveChangesAsync(cancellationToken);

            return addedEntity.Entity;
        }

        public async Task<ContactProvider?> Update(ContactProviderDto contactProviderDto, CancellationToken cancellationToken)
        {
            using var ctx = dbContextFactory.CreateDbContext();
            var contactProvider = await ctx.ContactProviders.FirstOrDefaultAsync(cp => cp.Id == contactProviderDto.id, cancellationToken);
            if (contactProvider == null)
                throw new ArgumentException($"ContactProvider with id: {contactProviderDto.id} not found.", nameof(contactProviderDto));

            ContactProviderFactory.UpdateContactProvider(contactProvider, contactProviderDto);
            await ctx.SaveChangesAsync(cancellationToken);

            return contactProvider;
        }

        public async Task<ContactProviderDto> CreateForUser(ContactProviderDto contactProviderDto, Shared.Dtos.User userDto, CancellationToken cancellationToken)
        {
            try
            {
                using var ctx = dbContextFactory.CreateDbContext();
                var user = await ctx.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);
                if (user == null)
                    throw new ArgumentException($"User not found id: {userDto.Id}");

                var contactProviderModel = ContactProviderFactory.CreateContactProviderModel(contactProviderDto);
                var userContactProvider = ctx.UserContactProviders.Add(new UserContactProvider { ContactProvider = contactProviderModel, User = user });
                await ctx.SaveChangesAsync(cancellationToken);
                return ContactProviderFactory.CreateContactProviderDto(userContactProvider.Entity.ContactProvider);
            }
            catch
            {
                throw;
            }
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                using var ctx = dbContextFactory.CreateDbContext();
                await ctx.UserContactProviders
                    .Where(ucp => ucp.ContactProvider.Id == id)
                    .ExecuteDeleteAsync(cancellationToken);

                await ctx.ContactProviders
                    .Where(cp => cp.Id == id)
                    .ExecuteDeleteAsync(cancellationToken);

                await ctx.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
        }
    }
}

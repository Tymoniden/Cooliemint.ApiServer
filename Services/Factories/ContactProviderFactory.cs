using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;

public static class ContactProviderFactory
{
    public static void UpdateContactProvider(ContactProvider contactProvider, ContactProviderDto contactProviderDto)
    {
        contactProvider.Description = contactProviderDto.Description;
        contactProvider.Configuration = contactProviderDto.Configuration;
    }

    public static ContactProvider CreateContactProviderModel(ContactProviderDto contactProviderDto)
    {
        ContactProvider contactProvider = new ContactProvider
        {
            Configuration = contactProviderDto.Configuration,
            Description = contactProviderDto.Description
        };

        switch (contactProviderDto.Type)
        {
            case ContactProviderTypeDto.Email:
                contactProvider.Type = ContactProviderModelType.Email;
                break;
            case ContactProviderTypeDto.Pushover:
                contactProvider.Type = ContactProviderModelType.Pushover;
                break;
            default:
                throw new ArgumentException(nameof(contactProvider.Type));
        }

        return contactProvider;
    }

    public static ContactProviderDto CreateContactProviderDto(ContactProvider contactProvider)
    {
        ContactProviderTypeDto mappedEnum;
        switch (contactProvider.Type)
        {
            case ContactProviderModelType.Email:
                mappedEnum = ContactProviderTypeDto.Email;
                break;
            case ContactProviderModelType.Pushover:
                mappedEnum = ContactProviderTypeDto.Pushover;
                break;
            default:
                throw new ArgumentException(nameof(contactProvider.Type));
        }

        return new ContactProviderDto(contactProvider.Id, mappedEnum, contactProvider.Description, contactProvider.Configuration);
    }
}

using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.ValueHistory
{
    public class ValueHistoryService
    {
        List<string> whiteListedKeys = [ "shellies/shelly12/command/switch:0" ];
        List<ValueHistoryEntryWrapper> values = [];

        public ValueHistoryService(IDbContextFactory<CooliemintDbContext> dbContextFactory, ValueStore valueStore)
        {
            valueStore.ValueSetChanged += (sender, eventArgs) =>
            {
                if (!whiteListedKeys.Contains(eventArgs.Key))
                    return;

                var valueEntry = values.FirstOrDefault(entry => entry.ValueStoreValueSet.Key == eventArgs.Key);
                if (valueEntry == null)
                {
                    values.Add(new ValueHistoryEntryWrapper
                    {
                        ValueStoreValueSet = eventArgs,
                        LastUpdated = DateTime.Now
                    });

                    using var ctx = dbContextFactory.CreateDbContext();
                    var entry = ctx.ValueHistoryEntries.Add(new ValueHistoryEntryModel
                    {
                        Key = eventArgs.Key,
                        Value = eventArgs.Value.ToString(),
                        From = DateTime.Now
                    });

                    ctx.SaveChanges();
                    return;
                }

                if (!valueEntry.ValueStoreValueSet.Value.Equals(eventArgs.Value))
                {
                    using var ctx = dbContextFactory.CreateDbContext();
                    var entry = ctx.ValueHistoryEntries.FirstOrDefault(entry => entry.Id == valueEntry.ValueHistoryEntryModel.Id);
                    
                    entry.To = DateTime.Now;

                    var newEntry = ctx.ValueHistoryEntries.Add(new ValueHistoryEntryModel
                    {
                        Key = eventArgs.Key,
                        Value = eventArgs.Value.ToString(),
                        From = DateTime.Now
                    });

                    ctx.SaveChanges();
                    return;
                }

                //valueEntry.LastUpdated = DateTime.Now;
            };
        }
    }

    public class ValueHistoryEntryWrapper
    {
        public required ValueSet ValueStoreValueSet { get; set; }
        public ValueHistoryEntryModel? ValueHistoryEntryModel { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

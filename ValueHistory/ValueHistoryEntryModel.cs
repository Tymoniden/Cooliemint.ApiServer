namespace Cooliemint.ApiServer.ValueHistory
{
    public class ValueHistoryEntryModel
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public string? Value { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}

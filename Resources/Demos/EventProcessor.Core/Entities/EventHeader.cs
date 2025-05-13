namespace EventProcessor.Core.Entities
{
    public class EventHeader
    {
        public int RecordId { get; set; }
        public int Version { get; set; }
        public DateTime Date { get; set; }
        public int? UniversalSeq { get; set; }
        public int? DailySeq { get; set; }
        public int? ServiceProvider { get; set; }
        public int EquipmentType { get; set; }
        public int EquipmentModel { get; set; }
        public int SerialNumber { get; set; }
        public int Line { get; set; }
        public int Station { get; set; }
        public int Hall { get; set; }
        public int Position { get; set; }
    }
}

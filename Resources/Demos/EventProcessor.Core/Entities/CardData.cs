namespace EventProcessor.Core.Entities
{
    public class CardData
    {
        public int CardDataVersion { get; set; }
        public long CardTagId { get; set; }
        public TPurseContext TPurseContext { get; set; }
        public int TPurseValue { get; set; }
        public TPurseLog TPurseLog { get; set; }
        public AppContext AppContext { get; set; }
        public ContractLists ContractLists { get; set; }
        public Contract? Contract { get; set; }
        public EnvironmentData Environment { get; set; }
    }

    public class TPurseContext
    {
        public int Status { get; set; }
        public bool TPurseAutoReloadActive { get; set; }
        public int SequenceNumber { get; set; }
        public int UnblockingNumber { get; set; }
    }

    public class TPurseLog
    {
        public DateTime DateStamp { get; set; }
        public TimeSpan TimeStamp { get; set; }
        public int ServiceProvider { get; set; }
        public int SerialNumber { get; set; }
        public int LocationId { get; set; }
        public int Device { get; set; }
        public long SAM { get; set; }
        public int PriceAmount { get; set; }
    }
}

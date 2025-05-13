namespace EventProcessor.Core.Entities
{
    public class AccountingOperation
    {
        public EventHeader Header { get; set; }
        public int OperationType { get; set; }
        public int OperationNumber { get; set; }
        public int PersonalizationFlag { get; set; }
        public long CardId { get; set; }
        public int Trips { get; set; }
        public int CardTrips { get; set; }
        public int OperationBalance { get; set; }
        public int CardBalance { get; set; }
        public int PassPrice { get; set; }
        public int PersonalizationFee { get; set; }
        public int CardDeposit { get; set; }
        public int TotalAmount { get; set; }
        public int Payment { get; set; }
        public int ServiceNumber { get; set; }
        public int UserCode { get; set; }
        public int? SequenceNumber { get; set; }
        public CardData CardData { get; set; }
    }
}

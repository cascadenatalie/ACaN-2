namespace ACaN2
{
    using System.Collections.Generic;

    public class DocumentDeliveryPayload
    {
        public string DocumentType { get; set; }
        public string LoanNumber { get; set; }
        public string LoanGuid { get; set; }
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();
    }
}

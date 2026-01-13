namespace CloudMart.Model
{
    public class ApprovalRequest
    {
        public string TaskToken { get; set; }
        public string OrderId { get; set; }
        public decimal OrderAmount { get; set; }
    }
}

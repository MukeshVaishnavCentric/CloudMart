using Amazon.DynamoDBv2.DataModel;

namespace CloudMart.Model
{
    [DynamoDBTable("CloudMartTable")]
    public class Order : BaseModel
    {
        [DynamoDBProperty] // Regular property
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using Amazon.DynamoDBv2.DataModel;

namespace CloudMart.Model
{
    [DynamoDBTable("CloudMartTable")]
    public class User : BaseModel
    {
        [DynamoDBProperty]
        public string Email { get; set; }
        public string LoyaltyTier { get; set; }
    }
}

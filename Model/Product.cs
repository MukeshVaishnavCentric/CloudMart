using Amazon.DynamoDBv2.DataModel;

namespace CloudMart.Model
{
    [DynamoDBTable("CloudMartTable")]
    public class Product : BaseModel
    {
        [DynamoDBProperty] // Regular property
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "ACTIVE";

        [DynamoDBGlobalSecondaryIndexHashKey("CMIndexV2")]
        public string EntityType { get; set; }

        [DynamoDBGlobalSecondaryIndexRangeKey("CMIndexV2")]
        public string EntityName { get; set; }
    }
}

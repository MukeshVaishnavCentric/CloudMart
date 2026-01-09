using Amazon.DynamoDBv2.DataModel;

namespace CloudMart.Model
{
    public class BaseModel
    {
        [DynamoDBHashKey] // Partition key
        public string PK { get; set; }

        [DynamoDBRangeKey] // Sort key
        public string SK { get; set; }
    }
}

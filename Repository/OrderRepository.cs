using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CloudMart.Model;

namespace CloudMart.Repository
{
    public class OrderRepository
    {
        private readonly DynamoDBContext _context;

        public OrderRepository()
        {
            var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);

            _context = new DynamoDBContextBuilder()
                .WithDynamoDBClient(() => client)
                .Build();
        }

        // get orders based on userid, tc of orderid later
        public async Task AddOrderAsync(Order order)
        {
            try
            {
                order.PK = $"USER#{order.UserId}";
                order.SK = $"ORDER{DateTime.UtcNow}";
                order.Status = "PENDING";

                await _context.SaveAsync(order);
            }
            catch (Exception) { throw; }
        }

        public async Task<List<Order>> GetByUser(string userId)
        {
            var conditions = new List<ScanCondition>
            {
                new("PK", ScanOperator.Equal, $"USER#{userId}")
            };

            return await _context.ScanAsync<Order>(conditions).GetRemainingAsync();
        }
    }
}

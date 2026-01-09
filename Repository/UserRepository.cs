using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CloudMart.Model;

namespace CloudMart.Repository
{
    public class UserRepository
    {
        private readonly DynamoDBContext _context;

        public UserRepository()
        {
            var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);

            _context = new DynamoDBContextBuilder()
                .WithDynamoDBClient(() => client)
                .Build();
        }

        public async Task SaveAsync(User user)
        {
            await _context.SaveAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _context.LoadAsync<User>($"USER#{userId}", "PROFILE");
        }
    }
}

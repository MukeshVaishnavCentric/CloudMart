using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CloudMart.Model;

namespace CloudMart.Repository
{
    public class ProductRepository
    {
        private readonly DynamoDBContext _context;

        public ProductRepository()
        {
            var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);

            _context = new DynamoDBContextBuilder()
                .WithDynamoDBClient(() => client)
                .Build();
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var item = new Product
            {
                PK = $"PRODUCT#{Guid.NewGuid()}",
                SK = "METADATA",
                Name = product.Name,
                Price = product.Price,
                EntityType = product.EntityType,
                EntityName = product.Name.ToLower()
            };

            await _context.SaveAsync(item);
            return item;
        }

        public async Task<Product?> GetAsync(string id)
        {
            return await _context.LoadAsync<Product>($"PRODUCT#{id}", "METADATA");
        }

        public async Task UpdateAsync(Product product)
        {
            var existing = await GetAsync(product.PK) ?? throw new Exception("Product not found");
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.EntityName = product.Name.ToLower();

            await _context.SaveAsync(existing);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteAsync<Product>($"PRODUCT#{id}", "METADATA");
        }

        public async Task<List<Product>> ListAllProductsAsync()
        {
            var conditions = new List<ScanCondition>
            {
                new("SK", ScanOperator.Equal, "METADATA"),
                new("PK", ScanOperator.BeginsWith, "PRODUCT#")
            };

            var results = await _context.ScanAsync<Product>(conditions).GetRemainingAsync();

            return results;
        }
        
        public async Task<List<Product>> ListAllMobilePhonesAsync()
        {
            var queryFilter = new QueryFilter("EntityType", QueryOperator.Equal, "PRODUCT"); // PRODUCT refers to mobile phones for now
            queryFilter.AddCondition("SK", QueryOperator.Equal, "METADATA");
            queryFilter.AddCondition("PK", QueryOperator.BeginsWith, "PRODUCT#");

            var queryConfig = new QueryOperationConfig
            {
                IndexName = "CMIndexV2",
                Filter = queryFilter
            };

            var results = await _context.FromQueryAsync<Product>(queryConfig).GetRemainingAsync();

            //var conditions = new List<ScanCondition>
            //{
            //    new("SK", ScanOperator.Equal, "METADATA"),
            //    new("PK", ScanOperator.BeginsWith, "PRODUCT#"),
            //    new("EntityType", ScanOperator.Equal, "PRODUCT")
            //};

            //var results = await _context.ScanAsync<Product>(conditions, search).GetRemainingAsync();

            return results;
        }

        public async Task<List<Product>> ListAllFruitsAsync()
        {
            var queryFilter = new QueryFilter("EntityType", QueryOperator.Equal, "FRUIT");
            queryFilter.AddCondition("SK", QueryOperator.Equal, "METADATA");
            queryFilter.AddCondition("PK", QueryOperator.BeginsWith, "PRODUCT#");

            var queryConfig = new QueryOperationConfig
            {
                IndexName = "CMIndexV2",
                Filter = queryFilter
            };

            var results = await _context.FromQueryAsync<Product>(queryConfig).GetRemainingAsync();

            return results;
        }
    }
}

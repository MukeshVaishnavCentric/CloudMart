using Amazon.Lambda.APIGatewayEvents;
using CloudMart.Helpers;
using CloudMart.Model;
using CloudMart.Repository;
using System.Text.Json;

namespace CloudMart.Controllers
{
    public class ProductController
    {
        private readonly ProductRepository _productRepository = new();

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest req)
        {
            return req.HttpMethod switch
            {
                "POST" => await AddProductsAsync(req),
                "GET" => await HandleGetReqAsync(req),
                "DELETE" => await DeleteAsync(req),
                "PUT" => await UpdateAsync(req),
                _ => Response.BadRequest("Unsupported method")
            };
        }

        private async Task<APIGatewayProxyResponse> UpdateAsync(APIGatewayProxyRequest req)
        {
            var product = JsonSerializer.Deserialize<Product>(req.Body!)!;
            await _productRepository.UpdateAsync(product);
            return Response.Ok("Product updated");
        }

        private async Task<APIGatewayProxyResponse> DeleteAsync(APIGatewayProxyRequest req)
        {
            if (req.PathParameters is null || !req.PathParameters.TryGetValue("id", out var id) || string.IsNullOrEmpty(id))
            {
                return Response.BadRequest("Product id is required");
            }

            await _productRepository.DeleteAsync(id);
            return Response.Ok($"Product with id {id} deleted successfully");
        }

        private async Task<APIGatewayProxyResponse> HandleGetReqAsync(APIGatewayProxyRequest req)
        {
            if (req.PathParameters != null && req.PathParameters.TryGetValue("id", out var id))
            {
                return await GetProductDetailsAsync(id);
            }

            if (req.QueryStringParameters?.TryGetValue("category", out var category) == true)
            {
                return category switch
                {
                    "mobile" => await GetAllMobilePhonesAsync(),
                    "fruit" => await GetAllFruitsAsync(),
                    _ => await GetAllProducstsAsync()
                };
            }

            return await GetAllProducstsAsync();
        }

        private async Task<APIGatewayProxyResponse> AddProductsAsync(APIGatewayProxyRequest req)
        {
            var product = JsonSerializer.Deserialize<Product>(req.Body!);
            var response = await _productRepository.AddProductAsync(product!);
            return Response.Created(response!);
        }

        private async Task<APIGatewayProxyResponse> GetProductDetailsAsync(string id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null) return Response.NotFound();
            return Response.Ok(product);
        }

        private async Task<APIGatewayProxyResponse> GetAllProducstsAsync()
        {
            var product = await _productRepository.ListAllProductsAsync();
            if (product.Count == 0) return Response.NotFound();
            return Response.Ok(product);
        }

        private async Task<APIGatewayProxyResponse> GetAllMobilePhonesAsync()
        {
            var product = await _productRepository.ListAllMobilePhonesAsync();
            if (product.Count == 0) return Response.NotFound();
            return Response.Ok(product);
        }

        private async Task<APIGatewayProxyResponse> GetAllFruitsAsync()
        {
            var product = await _productRepository.ListAllFruitsAsync();
            if (product.Count == 0) return Response.NotFound();
            return Response.Ok(product);
        }
    }
}

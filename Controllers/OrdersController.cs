using Amazon.Lambda.APIGatewayEvents;
using CloudMart.Helpers;
using CloudMart.Model;
using CloudMart.Repository;
using CloudMart.Services;
using System.Text.Json;

namespace CloudMart.Controllers
{
    public class OrdersController
    {
        private readonly OrderRepository _orderRepository = new();
        private readonly OrderService _orderSeravice = new();

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest req)
        {
            return req.HttpMethod switch
            {
                "POST" => await CreateOrderAsync(req),
                "GET" => (req.PathParameters != null && req.PathParameters.TryGetValue("userId", out var userId))
                    ? await GetByUserAsync(userId)
                    : Response.Ok("return all order // later"),
                _ => Response.BadRequest("Unsupported method")
            };
        }

        private async Task<APIGatewayProxyResponse> CreateOrderAsync(APIGatewayProxyRequest req)
        {
            try
            {
                var order = JsonSerializer.Deserialize<Order>(req.Body!);
                await _orderRepository.AddOrderAsync(order!);

                await _orderSeravice.SendSqsMessageForOrderAsync(order, Environment.GetEnvironmentVariable("OrderQueueSqsUrl")!); // todo: set env variable in sam template file
                
                return Response.Created(order!);
            }
            catch(Exception ex)
            {
                return Response.InternalServerError(ex.Message);
            }
        }

        private async Task<APIGatewayProxyResponse> GetByUserAsync(string userId)
        {
            try
            {
                var order = await _orderRepository.GetByUser(userId);
                if (order is null) return Response.NotFound();
                return Response.Ok(order);
            }
            catch (Exception ex)
            {
                return Response.InternalServerError(ex.Message);
            }
        }
    }
}

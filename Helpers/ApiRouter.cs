using Amazon.Lambda.APIGatewayEvents;
using CloudMart.Controllers;

namespace CloudMart.Helpers
{
    public class ApiRouter
    {
        private readonly ProductController _products = new();
        private readonly OrdersController _orders = new();
        private readonly UserController _users = new();

        public async Task<APIGatewayProxyResponse> RouteAsync(APIGatewayProxyRequest req)
        {
            var path = req.Path.ToLower();

            return path switch
            {
                var p when p.StartsWith("/products") => await _products.Handle(req),
                var p when p.StartsWith("/orders") => await _orders.Handle(req),
                var p when p.StartsWith("/users") => await _users.Handle(req),
                _ => new APIGatewayProxyResponse
                {
                    StatusCode = 404,
                    Body = "Not Found"
                }
            };
        }
    }
}

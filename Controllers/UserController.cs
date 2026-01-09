using Amazon.Lambda.APIGatewayEvents;
using CloudMart.Helpers;
using CloudMart.Model;
using CloudMart.Repository;
using System.Text.Json;

namespace CloudMart.Controllers
{
    public class UserController
    {
        private readonly UserRepository _userRepository = new();

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest req)
        {
            return req.HttpMethod switch
            {
                "POST" => await AddUserAsync(req),
                "GET" => (req.PathParameters != null && req.PathParameters.TryGetValue("userId", out var userId))
                    ? await GetByUserAsync(userId)
                    : Response.Ok("return all users // later"),
                _ => Response.BadRequest("Unsupported method")
            };
        }

        private async Task<APIGatewayProxyResponse> AddUserAsync(APIGatewayProxyRequest req)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(req.Body!); // email in email

                var userModel = new User
                {
                    PK = $"USER#{Guid.NewGuid()}",
                    SK = "PROFILE",
                    Email = user?.Email ?? string.Empty,
                    LoyaltyTier = "FREE"
                };

                await _userRepository.SaveAsync(userModel);

                return Response.Created(userModel);
            }
            catch (Exception ex)
            {
                return Response.InternalServerError(ex.Message);
            }
        }

        private async Task<APIGatewayProxyResponse> GetByUserAsync(string userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user is null) return Response.NotFound();
                return Response.Ok(user);
            }
            catch (Exception ex)
            {
                return Response.InternalServerError(ex.Message);
            }
        }
    }
}

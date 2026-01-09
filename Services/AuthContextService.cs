using Amazon.Lambda.APIGatewayEvents;

namespace CloudMart.Services
{
    public class AuthContextService
    {
        public static string GetUserId(APIGatewayProxyRequest req)
        {
            return req.RequestContext.Authorizer.Claims["sub"];
        }

        public static string GetEmail(APIGatewayProxyRequest req)
        {
            return req.RequestContext.Authorizer.Claims["email"];
        }

        public static string GetTier(APIGatewayProxyRequest req)
        {
            return req.RequestContext.Authorizer.Claims
                .GetValueOrDefault("custom:loyaltyTier", "FREE");
        }
    }
}

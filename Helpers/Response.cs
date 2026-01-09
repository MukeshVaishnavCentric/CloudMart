using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using System.Text.Json;

namespace CloudMart.Helpers
{
    public static class Response
    {
        public static APIGatewayProxyResponse Ok(object body) =>
            Create(HttpStatusCode.OK, body);

        public static APIGatewayProxyResponse Created(object body) =>
            Create(HttpStatusCode.Created, body);

        public static APIGatewayProxyResponse BadRequest(string msg) =>
            Create(HttpStatusCode.BadRequest, new { error = msg });

        public static APIGatewayProxyResponse NotFound() =>
            Create(HttpStatusCode.NotFound, new { error = "Not found" });

        public static APIGatewayProxyResponse InternalServerError(string message) =>
            Create(HttpStatusCode.InternalServerError, new { error = message });

        private static APIGatewayProxyResponse Create(HttpStatusCode code, object body) =>
            new()
            {
                StatusCode = (int)code,
                Body = JsonSerializer.Serialize(body),
                Headers = new Dictionary<string, string> { ["Content-Type"] = "application/json" }
            };
    }
}

using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using CloudMart.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CloudMart;

public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    //public string FunctionHandler(string input, ILambdaContext context)
    //{
    //    return input.ToUpper();
    //}

    private readonly ApiRouter _router = new();

    public async Task<APIGatewayProxyResponse> FunctionHandler(
        APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"{request.HttpMethod} {request.Path}");
            return await _router.RouteAsync(request);
        }
        catch(Exception ex)
        {
            context.Logger.LogError($"Error: {JsonConvert.SerializeObject(ex)}");
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = JsonConvert.SerializeObject(ex),
                Headers = new Dictionary<string, string>
                {
                    ["Content-Type"] = "application/json"
                }
            };

        }
    }

    //public APIGatewayProxyResponse FunctionHandler(
    //    APIGatewayProxyRequest request,
    //    ILambdaContext context)
    //{
    //    context.Logger.LogInformation($"Request: {request.HttpMethod} - {request.Path}");

    //    return request.Path.ToLower() switch
    //    {
    //        "/products" => Products(request),
    //        "/orders" => Orders(request),
    //        "/users" => Users(request),
    //        "/cart" => Cart(request),
    //        "/search" => Search(request),
    //        _ => NotFound()
    //    };
    //}

    //private APIGatewayProxyResponse Products(APIGatewayProxyRequest req)
    //{
    //    return req.HttpMethod switch
    //    {
    //        "GET" => Ok("List products"),
    //        "POST" => Ok("Create product"),
    //        "PUT" => Ok("Update product"),
    //        "DELETE" => Ok("Delete product"),
    //        _ => MethodNotAllowed()
    //    };
    //}

    //private APIGatewayProxyResponse Orders(APIGatewayProxyRequest req)
    //{
    //    return req.HttpMethod switch
    //    {
    //        "POST" => Ok("Order placed"),
    //        "GET" => Ok("Order history"),
    //        _ => MethodNotAllowed()
    //    };
    //}

    //private APIGatewayProxyResponse Users(APIGatewayProxyRequest req)
    //    => Ok("User registration / profile");

    //private APIGatewayProxyResponse Cart(APIGatewayProxyRequest req)
    //    => Ok("Cart updated");

    //private APIGatewayProxyResponse Search(APIGatewayProxyRequest req)
    //    => Ok("Search results");

    ////
    //private APIGatewayProxyResponse Ok(string msg) =>
    //    new()
    //    {
    //        StatusCode = (int)HttpStatusCode.OK,
    //        Body = JsonSerializer.Serialize(new { message = msg }),
    //        Headers = Headers()
    //    };

    //private APIGatewayProxyResponse NotFound() =>
    //    new()
    //    {
    //        StatusCode = 404,
    //        Body = "Endpoint not found",
    //        Headers = Headers()
    //    };

    //private APIGatewayProxyResponse MethodNotAllowed() =>
    //    new()
    //    {
    //        StatusCode = 405,
    //        Body = "Method not allowed",
    //        Headers = Headers()
    //    };

    //private Dictionary<string, string> Headers() =>
    //    new()
    //    {
    //        ["Content-Type"] = "application/json"
    //    };    
}

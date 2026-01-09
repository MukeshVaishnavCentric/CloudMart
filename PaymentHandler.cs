using Amazon.Lambda.Core;

namespace CloudMart
{
    public class PaymentHandler
    {
        public object HandleRequest(object input, ILambdaContext context)
        {
            context.Logger.LogInformation("Processing payment...");

            // Always succeed for now
            return new
            {
                paymentId = Guid.NewGuid().ToString(),
                status = "SUCCESS",
                amountProcessed = 99.99
            };
        }
    }
}

using Amazon.Lambda.Core;

namespace CloudMart
{
    public class FraudHandler
    {
        public object HandleRequest(object input, ILambdaContext context)
        {
            context.Logger.LogInformation("Checking for fraud...");

            // a flag the state machine can read
            return new
            {
                isFraud = false,
                confidenceScore = 0.98
            };
        }
    }
}

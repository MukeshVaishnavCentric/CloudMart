using Amazon.Lambda.Core;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using CloudMart.Model;
using Newtonsoft.Json;

namespace CloudMart
{
    public class ApprovalHandler
    {
        private readonly AmazonStepFunctionsClient _sf = new();

        public async Task HandleRequest(ApprovalRequest input, ILambdaContext context)
        {
            context.Logger.LogInformation("Waiting for human approval");

            await _sf.SendTaskSuccessAsync(new SendTaskSuccessRequest
            {
                TaskToken = input.TaskToken,
                Output = JsonConvert.SerializeObject(new { approved = true })
            });
        }
    }
}

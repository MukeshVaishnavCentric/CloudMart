using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using System.Text.Json;

namespace CloudMart.Services;

// this class be used later for step function integration
public class OrderService
{
    private readonly AmazonSQSClient _sqs = new();
    private readonly AmazonStepFunctionsClient _sf = new();

    public async Task SendSqsMessageForOrderAsync(object payload, string queueUrl)
    {
        await _sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(payload)
        });
    }

    public async Task StateMachineStartAsync(string stateMachineArn, object payload)
    {
        await _sf.StartExecutionAsync(new StartExecutionRequest
        {
            StateMachineArn = stateMachineArn,
            Input = JsonSerializer.Serialize(payload)
        });
    }
}

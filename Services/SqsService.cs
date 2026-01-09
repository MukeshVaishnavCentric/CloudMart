using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CloudMart.Services
{
    public class SqsService
    {
        public SqsService() { }

        public async Task SendOrder(string body)
        {
            var sqs = new AmazonSQSClient();
            //var queueUrl = Environment.GetEnvironmentVariable("ORDER_QUEUE_URL");

            await sqs.SendMessageAsync("QUEUE_URL", body);
        }
    }   
}

using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;

namespace WordRankerWorkerSQS.Controllers
{
    public class SQSActions
    {

        public ReceiveMessageResponse GetMessages(string queueURL)
        {

            var sqsConfig = new AmazonSQSConfig();
            sqsConfig.ServiceURL = "http://sqs.us-west-2.amazonaws.com";
            var sqsClient = new AmazonSQSClient(sqsConfig);

            var receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = queueURL;

            var receiveMessageResponse = sqsClient.ReceiveMessage(receiveMessageRequest);
            return receiveMessageResponse;
        }

        public bool DeleteMessage(string queueURL, string recieptHandle)
        {
            var sqsConfig = new AmazonSQSConfig();
            sqsConfig.ServiceURL = "http://sqs.us-west-2.amazonaws.com";
            var sqsClient = new AmazonSQSClient(sqsConfig);

            var deleteMessageRequest = new DeleteMessageRequest();
            deleteMessageRequest.QueueUrl = queueURL;
            deleteMessageRequest.ReceiptHandle = recieptHandle;

            var response = sqsClient.DeleteMessage(deleteMessageRequest);

            if (response.HttpStatusCode == HttpStatusCode.Accepted) return true;

            return false;
        }
        
    }
}
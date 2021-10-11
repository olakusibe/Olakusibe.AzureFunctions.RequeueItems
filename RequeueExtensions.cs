using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public static class RequeueExtensions
    {
        public static async Task RequeueItems(QueueClient sourceQueue, QueueClient destinationQueue)
        {
            int messageCount = 32; // from microsoft documentation, max message count is 32, allowed for a Azure Storage Queue

            var sourceQueueProps = await GetQueuePropertiesAsync(sourceQueue);
            int queuelength = sourceQueueProps.ApproximateMessagesCount; // get number of messages on the queue

            //Console.WriteLine($"Queue length is {queuelength:N0}");

            if (queuelength > 0)
            {
                var iterate = Math.Ceiling((double)queuelength / messageCount);
                do // ensures that's it will always run once, even if iterate equals zero
                {
                    //Console.WriteLine($"{iterate} Iteraton");
                    QueueMessage[] _messages = await sourceQueue.ReceiveMessagesAsync(messageCount);
                    if (_messages.Any())
                    {
                        foreach (var item in _messages)
                        {
                            await destinationQueue.SendMessageAsync(item.Body);
                            await sourceQueue.DeleteMessageAsync(item.MessageId, item.PopReceipt);
                            //Console.WriteLine($"Message (ID: {item.MessageId}) Re-queued");
                        }
                    }
                    iterate--;
                } while (iterate > 0);
            }
        }

        public static async Task<QueueClient> GetQueueClientAsync(string azureStorageUri, string queueName)
        {
            QueueClient queueClient = new QueueClient(azureStorageUri, queueName);
            await queueClient.CreateIfNotExistsAsync();
            return queueClient;
        }

        public static async Task<QueueProperties> GetQueuePropertiesAsync(QueueClient queueClient)
        {
            return await queueClient.GetPropertiesAsync();
        }
    }
}

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public static class TriggerSourceQueueToFail
    {
        [FunctionName(nameof(TriggerSourceQueueToFail))]
        public static void Run([QueueTrigger(RequeueSampleData.sourceQueueName)]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function {nameof(TriggerSourceQueueToFail)} processed: {myQueueItem}");

            try
            {
                throw new Exception("simulating a process failure for a dequeue to poison queue");
            }
            catch (Exception ex)
            {
                //log.LogCritical($"C# Queue trigger function {nameof(TriggerSourceQueueToFail)} failed to process: {myQueueItem}, Error Message: {ex.Message}");
                throw; // where the dequeue count limit is reached and queue item is sent to it's poison queue
            }
        }
    }
}

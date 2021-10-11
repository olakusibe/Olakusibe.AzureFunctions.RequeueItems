using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public class RequeueUsingDI
    {
        private readonly IConfiguration configuration;

        public RequeueUsingDI(IConfiguration configuration)
        {
            this.configuration = configuration;

            // TODO: inject other DI services here
        }

        [FunctionName(nameof(RequeueUsingDI))]
        public async Task Run([TimerTrigger("0 0 10-20/2 * * Mon-Fri")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function {nameof(RequeueUsingStatic)} executed at: {DateTime.Now:dd-MMM-yyyy}");

            try
            {
                var azure_storage_Uri = configuration["AzureWebJobsStorage"];

                QueueClient sourceQueue = await RequeueExtensions.GetQueueClientAsync(azure_storage_Uri, RequeueSampleData.sourceQueueName);
                QueueClient poisonQueue = await RequeueExtensions.GetQueueClientAsync(azure_storage_Uri, RequeueSampleData.poisonQueueName);

                await RequeueExtensions.RequeueItems(poisonQueue, sourceQueue);

            }
            catch (Exception ex)
            {
                // throw; // NOTE: not sure how safe it is to throw a TimeTrigger Exception, but you can never go wrong logging it somewhere
                log.LogCritical($"C# Timer trigger function {nameof(RequeueUsingStatic)} failed at: {DateTime.Now:dd-MMM-yyyy}, Error Message: {ex.Message}");
            }
        }
    }
}
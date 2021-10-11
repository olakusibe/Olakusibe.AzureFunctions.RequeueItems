using System;
using System.Collections.Generic;
using System.Text;

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public static class RequeueSampleData
    {
        public const string sourceQueueName = "main-queue";
        public const string poisonQueueName = "main-queue-poison"; // azure storage queue will auto create a poison queue with the suffix '-poison'
        public const string messageOnQueue = "just a string message for illustration, but it could also be a POCO or a JSON";
    }
}

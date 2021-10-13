# Azure Functions: Requeue Items
Sample code on how to move all queue items on to another queue in Azure Functions.

Here is the link to the article on [hashnode](https://dev.olakusibe.com/azure-functions-requeue-items) or [dev.to](https://dev.to/theolakusibe/azure-functions-requeue-items-226a).

The repo is a standard Azure Function App and you can run it locally (interact with it using Postman).

### Requirement: Local Env
1. Azure Function Core Tools.
2. Azure Storage Emulator.
3. Azure Storage Explorer.

### Steps
1. Start the Azure Function App.
2. Run `PopulateSourceQueue` http trigger, to put sample items on the queue.
3. The queue trigger `TriggerSourceQueueToFail`, will process the queue items, while also raising an exception to simulate an error that will cause the queue items to move on to the poison queue.
4. Check with Azure Storage Explorer that the queue named *main-queue-poison* has items in it.
5. Stop the Azure Function App.
6. Comment out ` throw new Exception("simulating a process failure for a dequeue to poison queue");` in `TriggerSourceQueueToFail.cs`, to simulate that no error is raised while processing queue items.
7. Restart the Azure Function App.
5. Use either of the time trigger `RequeueUsingDI` or `RequeueUsingStatic` to requeue the items from poison queue back to the main queue.
6. The queue trigger `TriggerSourceQueueToFail` will run again, this time the queue items will be processed and both *main-queue* and *main-queue-poison* should be empty.

### Notes
One of the Time trigger `RequeueUsingDI` or `RequeueUsingStatic` can only be active at a time, they do same task, and you don't want both to be running at the same time. They are in the repo to illustrate that requeue-ing can be done with either Dependency Injection (DI) or Static azure function.

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Olakusibe.AzureFunctions.RequeueItems.Startup))]

namespace Olakusibe.AzureFunctions.RequeueItems
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // TODO: Register your DI services here
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            // TODO: Setup your custom appsettings configuration builder here
        }
    }
}

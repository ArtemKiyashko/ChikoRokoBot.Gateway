using System;
using Azure.Data.Tables;
using Azure.Identity;
using ChikoRokoBot.Gateway.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

[assembly: FunctionsStartup(typeof(ChikoRokoBot.Gateway.Startup))]
namespace ChikoRokoBot.Gateway
{
	public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        private GatewayOptions _gatewayOptions = new();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<GatewayOptions>(_functionConfig.GetSection("GatewayOptions"));
            _functionConfig.GetSection("GatewayOptions").Bind(_gatewayOptions);

            builder.Services.AddAzureClients(clientBuilder => {
                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddTableServiceClient(_gatewayOptions.StorageAccount);
            });

            builder.Services.AddScoped<TableClient>((factory) => {
                var service = factory.GetRequiredService<TableServiceClient>();
                var client = service.GetTableClient(_gatewayOptions.UsersTableName);
                client.CreateIfNotExists();
                return client;
            });

            builder.Services.AddScoped<ITelegramBotClient>(factory => new TelegramBotClient(_gatewayOptions.TelegramBotToken));
        }
    }
}


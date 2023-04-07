using System;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Queues;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Managers;
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
                clientBuilder
                    .AddQueueServiceClient(_gatewayOptions.StorageAccount)
                    .ConfigureOptions((options) => { options.MessageEncoding = QueueMessageEncoding.Base64; });
            });

            builder.Services.AddScoped<BotStatusManager>();
            builder.Services.AddScoped<MessageManager>();
            builder.Services.AddSingleton<IManagerFactory, ManagerFactory>();

            builder.Services.AddScoped<QueueClient>((factory) => {
                var service = factory.GetRequiredService<QueueServiceClient>();
                var client = service.GetQueueClient(_gatewayOptions.NotificationQueueName);
                client.CreateIfNotExists();
                return client;
            });

            builder.Services.AddScoped<ITelegramBotClient>(factory => new TelegramBotClient(_gatewayOptions.TelegramBotToken));
        }
    }
}


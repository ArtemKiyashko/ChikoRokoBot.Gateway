using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using ChikoRokoBot.Gateway.Infrastructure;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Commands
{
	public class CurrentDropsCommand : ICommand
	{
        private readonly TableClient _dropsTableClient;
        private readonly QueueClient _queueClient;
        private readonly ILogger<CurrentDropsCommand> _logger;
        private readonly GatewayOptions _options;

        public CurrentDropsCommand(
            TableServiceClient tableServiceClient,
            QueueClient queueClient,
            IOptions<GatewayOptions> options,
            ILogger<CurrentDropsCommand> logger)
		{
            _dropsTableClient = tableServiceClient.GetTableClient(options.Value.DropsTableName);
            _dropsTableClient.CreateIfNotExists();

            _queueClient = queueClient;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<IActionResult> ProcessCommand(Update tgUpdate)
        {
            var currentDrops = _dropsTableClient.QueryAsync<DropTableEntity>($"PartitionKey eq '{_options.DropPartitionKey}' and Finish gt datetime'{DateTime.UtcNow.ToString("o")}'");

            var uniqueDrops = new HashSet<DropTableEntity>(new DropEqualityComparer());

            await foreach (var drop in currentDrops)
                uniqueDrops.Add(drop);

            foreach (var uniqueDrop in uniqueDrops)
                try
                {
                    var userDrop = new UserDrop(
                        tgUpdate.Message.Chat.Id,
                        tgUpdate.Message.MessageThreadId,
                        JsonSerializer.Deserialize<Drop>(uniqueDrop.DropJson));

                    await _queueClient.SendMessageAsync(JsonSerializer.Serialize(userDrop));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending user drop to queue. " +
                        $"ChatId: {tgUpdate.Message.Chat.Id}, " +
                        $"TopicId: {tgUpdate.Message.MessageThreadId}, " +
                        $"DropId: {uniqueDrop.RowKey}");
                }
            return new OkResult();
        }
    }
}


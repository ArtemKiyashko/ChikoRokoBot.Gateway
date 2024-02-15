using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using ChikoRokoBot.Gateway.Infrastructure;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Commands
{
	public class CurrentDropsCommand : ICommand
	{
        private readonly TableClient _dropsTableClient;
        private readonly QueueClient _queueClient;
        private readonly ILogger<CurrentDropsCommand> _logger;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly GatewayOptions _options;

        public CurrentDropsCommand(
            TableServiceClient tableServiceClient,
            QueueClient queueClient,
            IOptions<GatewayOptions> options,
            ILogger<CurrentDropsCommand> logger,
            ITelegramBotClient telegramBotClient)
		{
            _dropsTableClient = tableServiceClient.GetTableClient(options.Value.DropsTableName);
            _dropsTableClient.CreateIfNotExists();

            _queueClient = queueClient;
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _options = options.Value;
        }

        public async Task<IActionResult> ProcessCommand(Update tgUpdate)
        {
            if (!await TryNotifyProgress(tgUpdate, "Working..."))
                return new OkResult();

            var currentDrops = _dropsTableClient.QueryAsync<DropTableEntity>($"PartitionKey eq '{_options.DropPartitionKey}' and Finish gt datetime'{DateTime.UtcNow.ToString("o")}'");

            var uniqueDrops = new HashSet<DropTableEntity>(new DropEqualityComparer());

            await foreach (var drop in currentDrops)
                uniqueDrops.Add(drop);

            foreach (var uniqueDrop in uniqueDrops)
                try
                {
                    var drop = JsonSerializer.Deserialize<Drop>(uniqueDrop.DropJson);

                    drop.Toy.ModelUrlUsdz = uniqueDrop.ModelUrlUsdz;
                    drop.Toy.ModelUrlGlb = uniqueDrop.ModelUrlGlb;

                    var userDrop = new UserDrop(
                        tgUpdate.Message.Chat.Id,
                        tgUpdate.Message.MessageThreadId,
                        drop);

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

        private async Task<bool> TryNotifyProgress(Update tgUpdate, string message)
        {
            try
            {
                await _telegramBotClient.SendTextMessageAsync(tgUpdate.Message.Chat.Id, message, tgUpdate.Message.MessageThreadId);
                return true;
            }
            catch (ApiRequestException ex) when (ex.ErrorCode == 403)
            {
                _logger.LogError(ex, $"User restricted bot access. ChatId: {tgUpdate.Message.Chat.Id}");
            }
            return false;
        }
    }
}


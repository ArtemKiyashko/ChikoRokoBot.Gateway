using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChikoRokoBot.Gateway.Managers
{
	public class MessageManager : IMessageManager
    {
        private const string PARTIOTION_NAME = "primary";

        private readonly TableClient _usersTableClient;
        private readonly TableClient _dropsTableClient;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly QueueClient _queueClient;
        private ILogger _logger;

        public MessageManager(
            TableServiceClient tableServiceClient,
            ITelegramBotClient telegramBotClient,
            QueueClient queueClient,
            IOptions<GatewayOptions> options)
        {
            _usersTableClient = tableServiceClient.GetTableClient(options.Value.UsersTableName);
            _usersTableClient.CreateIfNotExists();

            _dropsTableClient = tableServiceClient.GetTableClient(options.Value.DropsTableName);
            _dropsTableClient.CreateIfNotExists();

            _telegramBotClient = telegramBotClient;
            _queueClient = queueClient;
        }

        public async Task<IActionResult> ProcessMessage(Update tgUpdate, ILogger logger)
        {
            _logger = logger;
            var botCommand = tgUpdate.Message.Entities?.FirstOrDefault(entity => entity.Type == MessageEntityType.BotCommand);

            if (botCommand is null) return new OkResult();

            var commandText = await GetCommandText(tgUpdate.Message, botCommand);

            switch (commandText) {
                case "/start": return await RegisterNewUser(tgUpdate);
                case "/currentdrops": return await SendCurrentDrops(tgUpdate);
                default: return new OkResult();
            }
        }

        private async Task<IActionResult> RegisterNewUser(Update tgUpdate)
        {
            var currentUserEntity = await _usersTableClient.GetEntityIfExistsAsync<UserTableEntity>(PARTIOTION_NAME, tgUpdate.Message.Chat.Id.ToString());
            if (currentUserEntity.HasValue) return new OkResult();

            var newUserEntity = new UserTableEntity
            {
                PartitionKey = PARTIOTION_NAME,
                RowKey = tgUpdate.Message.Chat.Id.ToString(),
                ChatId = tgUpdate.Message.Chat.Id,
                TopicId = tgUpdate.Message.MessageThreadId
            };

            await _usersTableClient.AddEntityAsync(newUserEntity);

            await _telegramBotClient.SendTextMessageAsync(tgUpdate.Message.Chat.Id, "Alright! You are now subscribed to Chiko and Roko drops notifications! If you wish to see currently active drops, use command /currentdrops");

            return new OkResult();
        }

        private async Task<string> GetCommandText(Message message, MessageEntity botCommandEntity)
        {
            var botUser = await _telegramBotClient.GetMeAsync();
            var fullCommand = message.Text.Substring(botCommandEntity.Offset, botCommandEntity.Length);

            return fullCommand.Replace($"@{botUser.Username}", string.Empty).ToLower().Trim();
        }

        private async Task<IActionResult> SendCurrentDrops(Update tgUpdate)
        {
            var currentDrops = _dropsTableClient.QueryAsync<DropTableEntity>($"PartitionKey eq '{PARTIOTION_NAME}' and Finish gt datetime'{DateTime.UtcNow.ToString("o")}'");

            await foreach (var drop in currentDrops)
            {
                try
                {
                    var userDrop = new UserDrop(
                        tgUpdate.Message.Chat.Id,
                        tgUpdate.Message.MessageThreadId,
                        JsonSerializer.Deserialize<Drop>(drop.DropJson));

                    await _queueClient.SendMessageAsync(JsonSerializer.Serialize(userDrop));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending user drop to queue. " +
                        $"ChatId: {tgUpdate.Message.Chat.Id}, " +
                        $"TopicId: {tgUpdate.Message.MessageThreadId}, " +
                        $"DropId: {drop.RowKey}");
                }
            }
            return new OkResult();
        }
    }
}
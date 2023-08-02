using System.Threading.Tasks;
using Azure.Data.Tables;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Commands
{
	public class NewUserCommand : ICommand
	{
        private readonly TableClient _usersTableClient;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly GatewayOptions _options;

        public NewUserCommand(
            TableServiceClient tableServiceClient,
            ITelegramBotClient telegramBotClient,
            IOptions<GatewayOptions> options)
		{
            _usersTableClient = tableServiceClient.GetTableClient(options.Value.UsersTableName);
            _usersTableClient.CreateIfNotExists();

            _telegramBotClient = telegramBotClient;
            _options = options.Value;
        }

        public async Task<IActionResult> ProcessCommand(Update tgUpdate)
        {
            var currentUserEntity = await _usersTableClient.GetEntityIfExistsAsync<UserTableEntity>(_options.UserPartitionKey, tgUpdate.Message.Chat.Id.ToString());
            if (currentUserEntity.HasValue) return new OkResult();

            var newUserEntity = new UserTableEntity
            {
                PartitionKey = _options.UserPartitionKey,
                RowKey = tgUpdate.Message.Chat.Id.ToString(),
                ChatId = tgUpdate.Message.Chat.Id,
                TopicId = tgUpdate.Message.MessageThreadId
            };

            await _usersTableClient.AddEntityAsync(newUserEntity);

            await _telegramBotClient.SendTextMessageAsync(tgUpdate.Message.Chat.Id, "Alright! You are now subscribed to Chiko and Roko drops notifications! If you wish to see currently active drops, use command /currentdrops");

            return new OkResult();
        }
    }
}


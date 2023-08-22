using System.Threading.Tasks;
using Azure.Data.Tables;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.ChatEvents
{
	public class ChatMigrateEvent : IChatEvent
	{
        private readonly GatewayOptions _options;
        private readonly TableClient _usersTableClient;

        public ChatMigrateEvent(
            TableServiceClient tableServiceClient,
            IOptions<GatewayOptions> options)
		{
            _options = options.Value;
            _usersTableClient = tableServiceClient.GetTableClient(_options.UsersTableName);
            _usersTableClient.CreateIfNotExists();

        }

        public async Task<IActionResult> ProcessEvent(Update tgUpdate)
        {

            var user = await _usersTableClient
                .GetEntityIfExistsAsync<UserTableEntity>(_options.UserPartitionKey, tgUpdate.Message.MigrateFromChatId.ToString());

            if (!user.HasValue)
                return new OkResult();

            await _usersTableClient.DeleteEntityAsync(_options.UserPartitionKey, user.Value.RowKey);

            user.Value.ChatId = tgUpdate.Message.Chat.Id;
            user.Value.RowKey = tgUpdate.Message.Chat.Id.ToString();
            await _usersTableClient.AddEntityAsync(user.Value);

            return new OkResult();
        }
    }
}


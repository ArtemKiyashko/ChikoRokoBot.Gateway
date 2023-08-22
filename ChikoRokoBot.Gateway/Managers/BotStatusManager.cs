using System.Threading.Tasks;
using Azure.Data.Tables;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Managers
{
	public class BotStatusManager : IMessageManager
    {
        private readonly TableClient _usersTableClient;
        private readonly GatewayOptions _options;
        private ILogger _logger;

        public BotStatusManager(
            TableServiceClient tableServiceClient,
            IOptions<GatewayOptions> options,
            ILogger<BotStatusManager> logger)
		{
            _options = options.Value;
            _logger = logger;
            _usersTableClient = tableServiceClient.GetTableClient(options.Value.UsersTableName);
            _usersTableClient.CreateIfNotExists();
        }

        public async Task<IActionResult> ProcessMessage(Update tgUpdate)
        {
            if (tgUpdate.MyChatMember.NewChatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Kicked)
            {
                await _usersTableClient.DeleteEntityAsync(
                    _options.UserPartitionKey,
                    tgUpdate.MyChatMember.Chat.Id.ToString());
            }

            return new OkResult();
        }
    }
}


using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Managers
{
	public class BotStatusManager : IMessageManager
    {
        private const string PARTIOTION_NAME = "primary";

        private readonly TableClient _usersTableClient;
        private ILogger _logger;

        public BotStatusManager(TableServiceClient tableServiceClient, IOptions<GatewayOptions> options)
		{
            _usersTableClient = tableServiceClient.GetTableClient(options.Value.UsersTableName);
            _usersTableClient.CreateIfNotExists();
        }

        public async Task<IActionResult> ProcessMessage(Update tgUpdate, ILogger logger)
        {
            _logger = logger;
            if (tgUpdate.MyChatMember.NewChatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Kicked)
                await _usersTableClient.DeleteEntityAsync(
                    PARTIOTION_NAME,
                    tgUpdate.MyChatMember.Chat.Id.ToString());
            return new OkResult();
        }
    }
}


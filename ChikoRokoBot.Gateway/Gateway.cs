using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using Azure.Data.Tables;
using ChikoRokoBot.Gateway.Models;

namespace ChikoRokoBot.Gateway
{
    public class Gateway
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly TableClient _tableClient;
        private const string PARTIOTION_NAME = "primary";

        public Gateway(ITelegramBotClient telegramBotClient, TableClient tableClient)
        {
            _telegramBotClient = telegramBotClient;
            _tableClient = tableClient;
        }

        [FunctionName("Gateway")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update tgUpdate,
            ILogger log)
        {
            switch (tgUpdate.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.MyChatMember: return await BotStatusChanged(tgUpdate);
                case Telegram.Bot.Types.Enums.UpdateType.Message: return await RegisterNewUser(tgUpdate);
                default: return new OkResult();
            };
        }

        private async Task<IActionResult> BotStatusChanged(Update tgUpdate)
        {
            if (tgUpdate.MyChatMember.NewChatMember.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Kicked)
                await _tableClient.DeleteEntityAsync(
                    PARTIOTION_NAME,
                    tgUpdate.MyChatMember.Chat.Id.ToString());
            return new OkResult();
        }

        private async Task<IActionResult> RegisterNewUser(Update tgUpdate)
        {
            if ("/start".Equals(tgUpdate.Message.Text, StringComparison.OrdinalIgnoreCase))
            {
                var currentUserEntity = await _tableClient.GetEntityIfExistsAsync<UserTableEntity>(PARTIOTION_NAME, tgUpdate.Message.Chat.Id.ToString());
                if (currentUserEntity.HasValue) return new OkResult();

                var newUserEntity = new UserTableEntity {
                    PartitionKey = PARTIOTION_NAME,
                    RowKey = tgUpdate.Message.Chat.Id.ToString(),
                    ChatId = tgUpdate.Message.Chat.Id,
                    TopicId = tgUpdate.Message.MessageThreadId
                };

                await _tableClient.AddEntityAsync(newUserEntity);

                await _telegramBotClient.SendTextMessageAsync(tgUpdate.Message.Chat.Id, "Окей вы подписаны на оповещения о новой дичи!");
            }
            return new OkResult();
        }
    }
}


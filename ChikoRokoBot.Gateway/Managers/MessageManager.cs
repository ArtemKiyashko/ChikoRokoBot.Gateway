using System.Threading.Tasks;
using ChikoRokoBot.Gateway.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Managers
{
	public class MessageManager : IMessageManager
    {
        private readonly IBotCommandFactory _botCommandFactory;

        public MessageManager(
            IBotCommandFactory botCommandFactory)
        {
            _botCommandFactory = botCommandFactory;
        }

        public async Task<IActionResult> ProcessMessage(Update tgUpdate, ILogger logger)
        {
            var command = await _botCommandFactory.GetCommand(tgUpdate);

            if (command is null) return new OkResult();

            return await command.ProcessCommand(tgUpdate);
        }
    }
}
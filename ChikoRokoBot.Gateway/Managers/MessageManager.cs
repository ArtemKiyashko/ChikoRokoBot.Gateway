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
        private readonly IChatEventFactory _chatEventFactory;
        private readonly ILogger<MessageManager> _logger;

        public MessageManager(
            IBotCommandFactory botCommandFactory,
            IChatEventFactory chatEventFactory,
            ILogger<MessageManager> logger)
        {
            _botCommandFactory = botCommandFactory;
            _chatEventFactory = chatEventFactory;
            _logger = logger;
        }

        public async Task<IActionResult> ProcessMessage(Update tgUpdate)
        {
            var command = await _botCommandFactory.GetCommand(tgUpdate);

            if (command is not null)
                return await command.ProcessCommand(tgUpdate);

            var chatEvent = await _chatEventFactory.GetEvent(tgUpdate);

            if (chatEvent is not null)
                return await chatEvent.ProcessEvent(tgUpdate);

            return new OkResult();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChikoRokoBot.Gateway.Commands;
using ChikoRokoBot.Gateway.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChikoRokoBot.Gateway.Managers
{
	public class BotCommandFactory : IBotCommandFactory
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IEnumerable<ICommand> _commands;

        public BotCommandFactory(
            ITelegramBotClient telegramBotClient,
            IEnumerable<ICommand> commands)
        {
            _telegramBotClient = telegramBotClient;
            _commands = commands;
        }

        public async Task<ICommand> GetCommand(Update tgUpdate)
		{
            var botCommand = tgUpdate.Message.Entities?.FirstOrDefault(entity => entity.Type == MessageEntityType.BotCommand);

            if (botCommand is null) return default;

            var commandText = await GetCommandText(tgUpdate.Message, botCommand);

            switch (commandText)
            {
                case "/start": return _commands.OfType<NewUserCommand>().FirstOrDefault();
                case "/currentdrops": return _commands.OfType<CurrentDropsCommand>().FirstOrDefault();
                default: return default;
            }
        }

        private async Task<string> GetCommandText(Message message, MessageEntity botCommandEntity)
        {
            var botUser = await _telegramBotClient.GetMeAsync();
            var fullCommand = message.Text.Substring(botCommandEntity.Offset, botCommandEntity.Length);

            return fullCommand.Replace($"@{botUser.Username}", string.Empty).ToLower().Trim();
        }
    }
}


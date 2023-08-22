using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChikoRokoBot.Gateway.ChatEvents;
using ChikoRokoBot.Gateway.Interfaces;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Managers
{
	public class ChatEventFactory : IChatEventFactory
	{
        private readonly IEnumerable<IChatEvent> _chatEvents;

        public ChatEventFactory(
            IEnumerable<IChatEvent> chatEvents)
		{
            _chatEvents = chatEvents;
        }

        public async Task<IChatEvent> GetEvent(Update tgUpdate)
        {
            if (tgUpdate.Message.MigrateFromChatId.HasValue)
                return _chatEvents.OfType<ChatMigrateEvent>().FirstOrDefault();
            return default;
        }
    }
}


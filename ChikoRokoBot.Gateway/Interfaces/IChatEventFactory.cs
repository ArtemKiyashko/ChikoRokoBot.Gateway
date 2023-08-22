using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Interfaces
{
	public interface IChatEventFactory
	{
        Task<IChatEvent> GetEvent(Update tgUpdate);
    }
}


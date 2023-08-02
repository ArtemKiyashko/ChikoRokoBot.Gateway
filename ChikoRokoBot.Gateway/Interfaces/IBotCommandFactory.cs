using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Interfaces
{
	public interface IBotCommandFactory
	{
        Task<ICommand> GetCommand(Update tgUpdate);
    }
}


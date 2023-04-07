using Telegram.Bot.Types.Enums;

namespace ChikoRokoBot.Gateway.Interfaces
{
    public interface IManagerFactory
    {
        IMessageManager GetManager(UpdateType updateType);
    }
}
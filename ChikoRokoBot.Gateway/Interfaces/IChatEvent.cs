using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Interfaces
{
	public interface IChatEvent
	{
        Task<IActionResult> ProcessEvent(Update tgUpdate);
    }
}


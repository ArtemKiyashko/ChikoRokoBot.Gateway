using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ChikoRokoBot.Gateway.Interfaces
{
	public interface ICommand
	{
        Task<IActionResult> ProcessCommand(Update tgUpdate);
    }
}


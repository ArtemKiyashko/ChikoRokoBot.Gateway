using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;

namespace ChikoRokoBot.Gateway.Interfaces
{
	public interface IMessageManager
	{
        Task<IActionResult> ProcessMessage(Update tgUpdate);
    }
}


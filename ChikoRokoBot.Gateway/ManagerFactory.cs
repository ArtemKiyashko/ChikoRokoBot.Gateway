using System;
using ChikoRokoBot.Gateway.Interfaces;
using ChikoRokoBot.Gateway.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;

namespace ChikoRokoBot.Gateway
{
    public class ManagerFactory : IManagerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ManagerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessageManager GetManager(UpdateType updateType) => updateType switch
        {
            UpdateType.MyChatMember => _serviceProvider.GetService<BotStatusManager>(),
            UpdateType.Message => _serviceProvider.GetService<MessageManager>(),
            _ => default
        };
    }
}


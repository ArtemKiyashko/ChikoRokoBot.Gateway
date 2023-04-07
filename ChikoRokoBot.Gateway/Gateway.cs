using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using Azure.Data.Tables;
using ChikoRokoBot.Gateway.Models;
using ChikoRokoBot.Gateway.Interfaces;

namespace ChikoRokoBot.Gateway
{
    public class Gateway
    {
        private readonly IManagerFactory _managerFactory;

        public Gateway(IManagerFactory managerFactory)
        {
            _managerFactory = managerFactory;
        }

        [FunctionName("Gateway")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update tgUpdate,
            ILogger log)
        {
            var manager = _managerFactory.GetManager(tgUpdate.Type);

            if (manager is null) return new OkResult();

            return await manager.ProcessMessage(tgUpdate, log);
        }
    }
}


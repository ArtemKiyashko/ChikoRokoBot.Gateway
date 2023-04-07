using System;
namespace ChikoRokoBot.Gateway.Options
{
	public class GatewayOptions
	{
		public string UsersTableName { get; set; } = "users";
        public string DropsTableName { get; set; } = "drops";
        public string StorageAccount { get; set; } = "UseDevelopmentStorage=true";
        public string NotificationQueueName { get; set; } = "notifydrops";
        public string TelegramBotToken { get; set; }
    }
}


using System;
namespace ChikoRokoBot.Gateway.Options
{
	public class GatewayOptions
	{
		public string UsersTableName { get; set; } = "users";
		public string StorageAccount { get; set; } = "UseDevelopmentStorage=true";
		public string TelegramBotToken { get; set; }
    }
}


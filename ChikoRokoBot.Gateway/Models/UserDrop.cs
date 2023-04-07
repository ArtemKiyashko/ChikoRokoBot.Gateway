using System;
namespace ChikoRokoBot.Gateway.Models
{
	public record UserDrop(
        long? ChatId,
        int? TopicId,
        Drop Drop);
}


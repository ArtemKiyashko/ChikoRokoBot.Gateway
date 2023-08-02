using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChikoRokoBot.Gateway.Models;

namespace ChikoRokoBot.Gateway.Infrastructure
{
	public class DropEqualityComparer : IEqualityComparer<DropTableEntity>
    {
        public bool Equals(DropTableEntity x, DropTableEntity y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;


            return ReferenceEquals(x, y)
                || (x.Toyid.HasValue && y.Toyid.HasValue && x.Toyid == y.Toyid)
                || (x.BlindBoxId.HasValue && y.BlindBoxId.HasValue && x.BlindBoxId == y.BlindBoxId);
        }

        public int GetHashCode([DisallowNull] DropTableEntity obj)
        {
            return (obj.Toyid ^ obj.BlindBoxId).GetHashCode();
        }
    }
}


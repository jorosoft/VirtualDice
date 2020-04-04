using System;
using System.Collections.Generic;
using VirtualDice.Models;

namespace VirtualDice.Hubs
{
    public static class UsersHandler
    {
        static UsersHandler()
        {
            ServerStartDate = DateTime.UtcNow;
        }

        public static List<NickName> Users { get; } = new List<NickName>();

        public static List<PlayerStats> PlayerStats { get; } = new List<PlayerStats>();

        public static DateTime ServerStartDate { get; }
    }
}

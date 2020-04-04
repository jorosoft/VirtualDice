using System;
using System.Collections.Generic;
using VirtualDice.Models;

namespace VirtualDice.Hubs
{
    public static class UsersHandler
    {
        public static List<NickName> Users { get; } = new List<NickName>();

        public static List<PlayerStats> PlayerStats { get; } = new List<PlayerStats>();

        public static DateTime ServerStartDate { get; set; } = DateTime.UtcNow;
    }
}

using System.Collections.Generic;
using VirtualDice.Models;

namespace VirtualDice.Hubs
{
    public static class UsersHandler
    {
        public static List<NickName> Users { get; } = new List<NickName>();
    }
}

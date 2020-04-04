using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDice.Models
{
    public class PlayerStats
    {
        public string NickName { get; set; }

        public int BestScore { get; set; }

        public int TotalScore { get; set; }

        public int TotalGamesPlayed { get; set; }
    }
}

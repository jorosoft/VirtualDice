using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using VirtualDice.Models;

namespace VirtualDice.Hubs
{
    public class DashbordHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("Enter", UsersHandler.Users.ToArray());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = UsersHandler.Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            Clients.All.SendAsync("Quit", user);

            UsersHandler.Users.Remove(user);

            return base.OnDisconnectedAsync(exception);
        }

        public void UpdateUser(string userName)
        {
            var user = UsersHandler.Users.FirstOrDefault(x => x.Name == userName);

            if (!string.IsNullOrWhiteSpace(userName) && user != null)
            {
                user.ConnectionId = Context.ConnectionId;
            }
        }

        public async Task Send()
        {
            var user = UsersHandler.Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (user != null && user.Score == null)
            {
                int[] totalScoresToNow = UsersHandler.Users?
                    .Where(x => x.Score != null)
                    .Select(x => x.Score[0] + x.Score[1] + x.Score[2])
                    .ToArray();
                int totalScore;
                int[] score;

                do
                {
                    score = RollDices();

                    totalScore = score[0] + score[1] + score[2];

                } while (Array.IndexOf(totalScoresToNow, totalScore) >= 0);

                user.Score = score;

                UpdateStats(user.Name, user.Score[0] + user.Score[1] + user.Score[2]);

                await Clients.All.SendAsync("Send", user);
            }
        }

        private static int[] RollDices()
        {
            var scores = new int[3];

            for (int i = 0; i < 3; i++)
            {
                Random r = new Random();

                scores[i] = r.Next(1, 6);
            }

            return scores;
        }

        private static void UpdateStats(string nickName, int score)
        {
            var playerStats = UsersHandler.PlayerStats.FirstOrDefault(x => x.NickName == nickName);

            if (playerStats != null)
            {
                if (score > playerStats.BestScore)
                {
                    playerStats.BestScore = score;
                }

                playerStats.TotalScore += score;
                playerStats.TotalGamesPlayed++;
            }
            else
            {
                playerStats = new PlayerStats
                {
                    NickName = nickName,
                    BestScore = score
                };

                playerStats.TotalScore += score;
                playerStats.TotalGamesPlayed++;

                UsersHandler.PlayerStats.Add(playerStats);
            }
        }

    }
}

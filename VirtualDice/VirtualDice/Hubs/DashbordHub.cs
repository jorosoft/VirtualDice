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
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string user;

        public DashbordHub(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.user = this.httpContextAccessor.HttpContext.Request.Cookies["user"];
        }

        public override Task OnConnectedAsync()
        {
            var user = new NickName
            {
                Name = this.user
            };

            if (UsersHandler.Users.Count == 0)
            {
                user.isAdmin = true;
            }

            UsersHandler.Users.Add(user);

            Clients.All.SendAsync("Enter", UsersHandler.Users.ToArray());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = UsersHandler.Users.FirstOrDefault(x => x.Name == this.user);

            Clients.All.SendAsync("Quit", user);

            UsersHandler.Users.Remove(user);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send()
        {
            var user = UsersHandler.Users.FirstOrDefault(x => x.Name == this.user);

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

    }
}

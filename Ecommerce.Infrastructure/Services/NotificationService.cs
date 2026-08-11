using Ecommerce.Application.Interfaces;
using Google.Apis.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> hubContext;

        public NotificationService(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task SendNotificationAsync(string userId, object message)
        {
            await hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification",message);
        }
    }
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
